using KBCC.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KBCC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public JsonResult ImportExcel()
        {
            List<Order> excelData = new List<Order>();
            if (Request.Files.Count > 0)
            {
                try
                {
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFileBase file = files[i];
                        string fname;
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = file.FileName;
                        }
                        var newName = fname.Split('.');
                        fname = newName[0] + "_" + DateTime.Now.Ticks.ToString() + "." + newName[1];
                        var uploadRootFolderInput = AppDomain.CurrentDomain.BaseDirectory + "Uploaded\\Excel";
                        Directory.CreateDirectory(uploadRootFolderInput);
                        var directoryFullPathInput = uploadRootFolderInput;
                        fname = Path.Combine(directoryFullPathInput, fname);
                        file.SaveAs(fname);
                        string xlsFile = fname;
                        excelData = ReadExcel(fname);
                    }
                    if (excelData.Count > 0)
                    {
                        return Json(excelData, JsonRequestBehavior.AllowGet);
                    }
                    else
                        return Json(false, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
        public List<Order> ReadExcel(string FilePath)
        {
            try
            {
                List<Order> excelData = new List<Order>();
                FileInfo existingFile = new FileInfo(FilePath);
                using (ExcelPackage package = new ExcelPackage(existingFile))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                    int rowCount = worksheet.Dimension.End.Row;
                    for (int row = 2; row <= rowCount; row++)
                    {
                        excelData.Add(new Order()
                        {
                            CustomerPhone = worksheet.Cells[row, 1].Value.ToString().Trim(),
                            CustomerName = worksheet.Cells[row, 2].Value.ToString().Trim(),
                            CustomerAddress = worksheet.Cells[row, 3].Value.ToString().Trim(),
                            InternalNote = worksheet.Cells[row, 4].Value.ToString().Trim(),
                        });
                    }
                }
                return excelData;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}