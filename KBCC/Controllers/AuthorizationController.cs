using KBCC.Helper;
using KBCC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static KBCC.Helper.Helpers;

namespace KBCC.Controllers
{
    [Authorize, KBCCActionFilter, SyntaxDataActionFilter, HistoryActionFilter]
    public class AuthorizationController : Controller
    {
        // GET: Authorization
        public ActionResult Index()
        {
            return View();
        }
        public ViewResult ActionHistories()
        {
            return View();
        }
        public ViewResult UserManagement()
        {
            return View();
        }

        public ViewResult UserApproval()
        {
            return View();
        }

        public ViewResult RoleActionManagement()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="des"></param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Get)]
        public async Task<JsonResult> AddRole(string name, string des)
        {
            using (var db = new ApplicationDbContext())
            {
                var role = db.KBCCRoles.FirstOrDefault(x => x.Name == name);
                if (role != null)
                {
                    return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                }

                var user = db.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);

                db.KBCCRoles.Add(new Role
                {
                    Name = name,
                    Description = des,
                    CreatedTime = DateTime.Now,
                    CreatedBy = string.Format("{0} ({1})", user.Name, user.UserName)
                });
                await db.SaveChangesAsync();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public async Task<JsonResult> DeleteRole(int id)
        {
            using (var db = new ApplicationDbContext())
            {
                var role = db.KBCCRoles.FirstOrDefault(x => x.Id == id);
                if (role != null)
                {
                    db.KBCCRoles.Remove(role);
                }
                await db.SaveChangesAsync();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetRoles(int page, int rows)
        {
            using (var db = new ApplicationDbContext())
            {
                var ls = db.KBCCRoles.OrderBy(x => x.Name).ToList();
                page = page > ls.Count ? ls.Count : page;
                return Json(new
                {
                    pageinfo = new { current = page, total = (int)Math.Ceiling((decimal)ls.Count / rows) },
                    list = ls.Skip((page - 1) * rows).Take(rows).Select(x => new
                    {
                        x.Id,
                        x.Name,
                        x.Description,
                        x.CreatedBy,
                        CreatedTime = x.CreatedTime.Value.ToString("yyyy/MM/dd HH:mm:ss")
                    })
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public async Task<JsonResult> AddAction(int role, string act, string c, string menu)
        {
            using (var db = new ApplicationDbContext())
            {
                var rc = db.RoleActions.FirstOrDefault(x => x.RoleId == role && x.Action == act && x.Controller == c);
                if (rc != null)
                {
                    return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                }

                var user = db.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);
                db.RoleActions.Add(new RoleAction
                {
                    RoleId = role,
                    Action = act,
                    Controller = c,
                    CreatedBy = string.Format("{0} ({1})", user.Name, user.UserName),
                    CreatedTime = DateTime.Now,
                    Menu = menu
                });
                await db.SaveChangesAsync();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public async Task<JsonResult> DeleteAction(int id)
        {
            using (var db = new ApplicationDbContext())
            {
                var rc = db.RoleActions.FirstOrDefault(x => x.Id == id);
                if (rc != null)
                {
                    db.RoleActions.Remove(rc);
                }
                await db.SaveChangesAsync();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetActions(int role, int page, int rows)
        {
            using (var db = new ApplicationDbContext())
            {
                var ls = db.RoleActions.Where(x => x.RoleId == role)
                    .OrderBy(x => x.Controller)
                    .ThenBy(x => x.Action).ToList();
                page = page > ls.Count ? ls.Count : page;
                return Json(new
                {
                    pageinfo = new { current = page, total = (int)Math.Ceiling((decimal)ls.Count / rows) },
                    list = ls.Skip((page - 1) * rows).Take(rows).Select(x => new
                    {
                        x.Id,
                        x.Action,
                        x.Controller,
                        x.Menu,
                        x.CreatedBy,
                        CreatedTime = x.CreatedTime.Value.ToString("yyyy/MM/dd HH:mm:ss")
                    })
                }, JsonRequestBehavior.AllowGet);
            }
        }

        private string GetControllerName(string fullname)
        {
            try
            {
                int index = fullname.LastIndexOf(".");
                return fullname.Substring(index + 1, fullname.Length - index - 1).Replace("Controller", "");
            }
            catch (Exception)
            {

            }
            return "NA";
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetController()
        {
            Assembly asm = Assembly.GetExecutingAssembly();

            var ls = asm.GetTypes()
                .Where(x => x.BaseType.Name == "Controller")
                .OrderBy(x => x.Name)
                .Select(x => new
                {
                    Text = x.Name.Replace("Controller", ""),
                    Value = x.FullName
                });

            return Json(ls, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetControllerActions(string c)
        {
            var t = Assembly.GetExecutingAssembly().GetType(c);
            var methods = t.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(m => m.ReturnType.FullName.Contains("ActionResult") || m.ReturnType.FullName.Contains("ViewResult"))
                .OrderBy(m => m.Name)
                .Select(m => m.Name).Distinct();
            return Json(methods, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult getUsers(int role, string part, string patt)
        {
            using (var db = new ApplicationDbContext())
            {
                var crnUsers = db.UserRoles.Where(x => x.RoleId == role).Select(x => x.UserId).ToList();

                var userlist = db.Users.Where(x => !crnUsers.Contains(x.Id) && x.Department.Equals(part) && x.Name.Contains(patt))
                    .OrderBy(x => x.Name)
                    .Select(x => new
                    {
                        x.UserName,
                        x.Id,
                        x.Name,
                        x.Department
                    }).ToList();

                return Json(userlist, JsonRequestBehavior.AllowGet);
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<JsonResult> AddUserRole(int role, string user)
        {
            using (var db = new ApplicationDbContext())
            {
                var chk = db.UserRoles.FirstOrDefault(x => x.RoleId == role && x.UserId == user);
                if (chk != null)
                {
                    return Json(new { success = false, msg = "Error: User already exists in role !" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    db.UserRoles.Add(new UserRole
                    {
                        RoleId = role,
                        UserId = user,
                        CreatedBy = string.Format("{0} ({1})", Session["FullName"], User.Identity.Name),
                        CreatedTime = DateTime.Now
                    });
                    await db.SaveChangesAsync();
                    return Json(new { success = true, msg = "User has added to role !" }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult getUsersInRole(int role, int page = 1, int rows = 10)
        {
            using (var db = new ApplicationDbContext())
            {
                var crnUsers = db.UserRoles.Where(x => x.RoleId == role).Select(x => x.UserId).ToList();
                var users = (from x in db.Users
                             where crnUsers.Contains(x.Id)
                             join y in db.UserRoles on x.Id equals y.UserId
                             where y.RoleId == role
                             select new
                             {
                                 x.Id,
                                 x.UserName,
                                 x.Name,
                                 x.Department,
                                 x.Premises,
                                 x.IsBlocked,
                                 y.CreatedBy,
                                 y.CreatedTime
                             }).ToList();
                page = page > users.Count ? users.Count : page;

                return Json(new
                {
                    pageinfo = new { current = page, total = (int)Math.Ceiling((decimal)users.Count / rows) },
                    user = users.Skip((page - 1) * rows).Take(rows).Select(x => new
                    {
                        x.Id,
                        x.UserName,
                        x.Name,
                        x.Department,
                        x.Premises,
                        x.IsBlocked,
                        x.CreatedBy,
                        CreatedTime = x.CreatedTime.Value.ToString("yyyy-MM-dd HH:mm:ss")
                    })
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<JsonResult> DelUserFromRole(int role, string user)
        {
            using (var db = new ApplicationDbContext())
            {
                var chk = db.UserRoles.FirstOrDefault(x => x.RoleId == role && x.UserId == user);
                if (chk != null)
                {
                    db.UserRoles.Remove(chk);
                    await db.SaveChangesAsync();
                    return Json(new { success = true, msg = "Delete user successfull !" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, msg = "Error: No user to delete in role !" }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult getActionHistories(string controller, string action, string user, string date, int page = 1, int rows = 20)
        {
            using (var db = new ApplicationDbContext())
            {
                var ls = db.ActionHistories.Where(x =>
                            x.Controller.Contains(controller) &&
                            x.Action.Contains(action) &&
                            x.UserName.Contains(user))
                        .OrderByDescending(x => x.DateTime)
                        .ToList();
                var ls1 = ls.Where(x => CompareDate(x.DateTime, date)).Select(x => x);

                var totalPages = (int)Math.Ceiling((decimal)ls1.Count() / rows);
                page = page > totalPages ? totalPages : page;

                return Json(new
                {
                    pageinfo = new { current = page, total = totalPages },
                    list = ls1.Skip((page - 1) * rows).Take(rows).Select(x => new
                    {
                        x.Controller,
                        x.Action,
                        ReturnType = GetRealReturnType(x.ReturnType),
                        x.Anonymouse,
                        x.RouteData,
                        x.Remark,
                        x.Permission,
                        x.UserName,
                        DateTime = x.DateTime.ToString("yyyy-MM-dd HH:mm:ss")
                    })
                });
            }
        }
        private bool CompareDate(DateTime date1, string date2)
        {
            return date1.ToString("yyyy-MM-dd").Contains(date2);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult getActions()
        {
            using (var db = new ApplicationDbContext())
            {
                var list = db.ActionHistories.Select(x => x.Action).OrderBy(x => x).Distinct().ToList();
                return Json(list);
            }
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult getControllers()
        {
            using (var db = new ApplicationDbContext())
            {
                var list = db.ActionHistories.Select(x => x.Controller).OrderBy(x => x).Distinct().ToList();
                return Json(list);
            }
        }
    }

}