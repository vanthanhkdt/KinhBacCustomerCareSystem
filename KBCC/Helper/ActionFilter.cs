using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Routing;
using System.Text;
using System.Web.Mvc.Async;
using KBCC.Models;

namespace KBCC.Helper
{
    public class SyntaxDataActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session["FullName"] == null)
            {
                Debug.WriteLine("=> Session Expired\r\n");
                filterContext.Result = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary(
                        new { controller = "Account", action = "Login" }
                    ));
            }
        }
    }
    public class HistoryActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            UpdateActionHistory(context: filterContext);
        }
        private void UpdateActionHistory(ActionExecutingContext context)
        {
            using (var db = new ApplicationDbContext())
            {
                var user = db.Users.FirstOrDefault(x => x.UserName == context.HttpContext.User.Identity.Name);

                db.ActionHistories.Add(new ActionHistories
                {
                    Controller = context.ActionDescriptor.ControllerDescriptor.ControllerName,
                    Action = context.ActionDescriptor.ActionName,
                    Anonymouse = IsAllowAnonymouseAction(context).ToString(),
                    ReturnType = ActionReturnType(context),
                    UserId = user == null ? "" : user.Id,
                    UserName = user == null ? "" : string.Format("{0} ({1})", user.Name, user.UserName),
                    DateTime = DateTime.Now,
                    Permission = IsAuthorized(context).ToString(),
                    RouteData = GetRouteDataString(context),
                    Remark = context.HttpContext.Request.UserHostAddress
                });
                db.SaveChanges();
            }
        }
        private string ActionReturnType(ActionExecutingContext filtercontext)
        {
            var actionDescriptor = filtercontext.ActionDescriptor;
            if (actionDescriptor is TaskAsyncActionDescriptor)
            {
                var name = ((TaskAsyncActionDescriptor)actionDescriptor).MethodInfo.ReturnType.FullName;
                return name;
            }
            else
            {
                return ((ReflectedActionDescriptor)filtercontext.ActionDescriptor).MethodInfo.ReturnType.Name.ToString();
            }

        }
        private bool IsAuthorized(ActionExecutingContext context)
        {
            string flag = ConfigurationManager.AppSettings["IsRequiredAuthentication"];

            if (flag == "No")
            {
                return true;
            }

            string userName = context.HttpContext.User.Identity.Name;
            string action = context.ActionDescriptor.ActionName;
            string controller = context.ActionDescriptor.ControllerDescriptor.ControllerName;

            using (var db = new ApplicationDbContext())
            {
                ApplicationUser user = db.Users.FirstOrDefault(x => x.UserName == context.HttpContext.User.Identity.Name);
                if (user != null)
                {
                    var roles = db.UserRoles.Where(x => x.UserId == user.Id).Select(x => x.RoleId).Distinct().ToList();
                    bool IsAuthor = db.RoleActions.Any(x => roles.Contains(x.RoleId) && x.Controller.Equals(controller) && x.Action.Equals(action));
                    return IsAuthor;
                }
            }
            return false;
        }

        private bool IsAllowAnonymouseAction(ActionExecutingContext filterContext)
        {
            if (filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true))
            {
                return true;
            }
            return false;
        }

        private string GetRouteDataString(ActionExecutingContext data)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in data.ActionParameters)
            {
                sb.Append(string.Format("{0}={1}|", item.Key, item.Value));
            }
            return sb.ToString();
        }
    }
    public class KBCCActionFilter : ActionFilterAttribute
    {
        public KBCCActionFilter()
        { }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                base.OnActionExecuting(filterContext);
            }
            else
            {
                if (UserNeedChangePwd(filterContext))
                {
                    filterContext.Result = new RedirectToRouteResult(
                        new System.Web.Routing.RouteValueDictionary(
                            new { controller = "Manage", action = "ChangePassword" }
                        ));
                }

                string userName = filterContext.HttpContext.User.Identity.Name;
                string action = filterContext.ActionDescriptor.ActionName;
                string controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;

                Debug.WriteLine("\\====================================================\r\n" + userName);
                Debug.WriteLine(ActionReturnType(filterContext));
                Debug.WriteLine(action);
                Debug.WriteLine(controller + "\r\n\\====================================================");


                if (!isJsonResult(filterContext) && !IsAllowAnonymouseAction(filterContext))
                {
                    if (!IsAuthorized(filterContext))
                    {
                        //UpdateActionHistory(context: filterContext);
                        filterContext.Result = new RedirectToRouteResult(
                        new System.Web.Routing.RouteValueDictionary(
                            new { controller = "Home", action = "PermissionError" }
                        ));
                    }
                    else
                    {
                        //UpdateActionHistory(context: filterContext);
                        base.OnActionExecuting(filterContext);
                    }

                }
                else
                {
                    //UpdateActionHistory(context: filterContext);
                    base.OnActionExecuting(filterContext);
                }
            }
        }

        private void UpdateActionHistory(ActionExecutingContext context)
        {
            using (var db = new ApplicationDbContext())
            {
                var user = db.Users.FirstOrDefault(x => x.UserName == context.HttpContext.User.Identity.Name);

                db.ActionHistories.Add(new ActionHistories
                {
                    Controller = context.ActionDescriptor.ControllerDescriptor.ControllerName,
                    Action = context.ActionDescriptor.ActionName,
                    Anonymouse = IsAllowAnonymouseAction(context).ToString(),
                    ReturnType = ActionReturnType(context),
                    UserId = user.Id,
                    UserName = string.Format("{0} ({1})", user.Name, user.UserName),
                    DateTime = DateTime.Now,
                    Permission = IsAuthorized(context).ToString(),
                    RouteData = GetRouteDataString(context),
                    Remark = context.HttpContext.Request.UserHostAddress
                });
                db.SaveChanges();
            }
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            base.OnResultExecuted(filterContext);
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            base.OnResultExecuting(filterContext);
        }

        private bool isJsonResult(ActionExecutingContext filtercontext)
        {
            return (ActionReturnType(filtercontext).Contains("JsonResult"));
        }

        private string ActionReturnType(ActionExecutingContext filtercontext)
        {
            var actionDescriptor = filtercontext.ActionDescriptor;
            if (actionDescriptor is TaskAsyncActionDescriptor)
            {
                var name = ((TaskAsyncActionDescriptor)actionDescriptor).MethodInfo.ReturnType.FullName;
                return name;
            }
            else
            {
                return ((ReflectedActionDescriptor)filtercontext.ActionDescriptor).MethodInfo.ReturnType.Name.ToString();
            }

        }

        private bool IsAuthorized(ActionExecutingContext context)
        {
            string flag = ConfigurationManager.AppSettings["IsRequiredAuthentication"];

            if (flag == "No")
            {
                return true;
            }

            string userName = context.HttpContext.User.Identity.Name;
            if (userName == "vanthanhkdt@gmail.com")
            {
                return true;
            }
            string action = context.ActionDescriptor.ActionName;
            string controller = context.ActionDescriptor.ControllerDescriptor.ControllerName;

            using (var db = new ApplicationDbContext())
            {
                ApplicationUser user = db.Users.FirstOrDefault(x => x.UserName == context.HttpContext.User.Identity.Name);
                if (user != null)
                {
                    var roles = db.UserRoles.Where(x => x.UserId == user.Id).Select(x => x.RoleId).Distinct().ToList();
                    bool IsAuthor = db.RoleActions.Any(x => roles.Contains(x.RoleId) && x.Controller.Equals(controller) && x.Action.Equals(action));
                    return IsAuthor;
                }
            }
            return false;
        }

        private bool IsAllowAnonymouseAction(ActionExecutingContext filterContext)
        {
            if (filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true))
            {
                return true;
            }
            return false;
        }

        private string GetRouteDataString(ActionExecutingContext data)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in data.ActionParameters)
            {
                sb.Append(string.Format("{0}={1}|", item.Key, item.Value));
            }
            return sb.ToString();
        }

        private bool UserNeedChangePwd(ActionExecutingContext filter)
        {
            using (var db = new ApplicationDbContext())
            {
                var user = db.Users.FirstOrDefault(x => x.UserName == filter.HttpContext.User.Identity.Name);
                if (user != null && user.IsPasswordExpired)
                {
                    return true;
                }
            }
            return false;
        }
    }
}