using Staj1.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Excel = Microsoft.Office.Interop.Excel;


namespace Staj1.Controllers
{
    public class ExcelController : Controller
    {
        // GET: Excel
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);
        Staj1DB context = new Staj1DB();

        OleDbConnection Econ;
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ExcelPage()
        {
            return View();
        }

        [HttpPost]

        public ActionResult Index(HttpPostedFileBase file)
        {

            InsertExceldata(file);
            return RedirectToAction("KayitOlustur", "Admin");
            ViewBag.Mesaj = "Üyelik işleminiz başarıyla gerçekleştirilmiştir.";

        }

        private void ExcelConn(string filepath)
        {
            string constr = string.Format(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=""Excel 12.0 Xml;HDR=YES;""", filepath);
            Econ = new OleDbConnection(constr);

        }

        private ActionResult InsertExceldata(HttpPostedFileBase excelFile)
        {

            if (excelFile == null
            || excelFile.ContentLength == 0)
            {
                ViewBag.Error = "Lütfen dosya seçimi yapınız.";

                return View();
            }
            else
            {
                //Dosyanın uzantısı xls ya da xlsx ise;
                if (excelFile.FileName.EndsWith("xls")
                || excelFile.FileName.EndsWith("xlsx"))
                {
                    //Seçilen dosyanın nereye yükleneceği seçiliyor.
                    string path = Server.MapPath("~/Content/" + excelFile.FileName);

                    //Dosya kontrol edilir, varsa silinir.
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }

                    //Excel path altına kaydedilir.
                    excelFile.SaveAs(path);

                    Excel.Application application = new Excel.Application();
                    Excel.Workbook workbook = application.Workbooks.Open(path);
                    Excel.Worksheet worksheet = (Excel.Worksheet)workbook.ActiveSheet;
                    Excel.Range range = worksheet.UsedRange;

                    List<Kullanici> localList = new List<Kullanici>();

                    for (int i = 2; i <= range.Rows.Count; i++)
                    {
                        Kullanici lm = new Kullanici();

                        lm.Numara = (string)((Excel.Range)range.Cells[i, 1]).Text;
                        lm.Adi = (string)((Excel.Range)range.Cells[i, 2]).Text;
                        lm.Soyadi = (string)((Excel.Range)range.Cells[i, 3]).Text;
                        lm.Mail = (string)((Excel.Range)range.Cells[i, 4]).Text;
                    }

                    application.Quit();

                    ViewBag.ListDetay = localList;


                    foreach (var item in localList)
                    {
                        //Rol kullanici = context.Rol.FirstOrDefault(x => x.RolAdi == "Kullanici");
                        //KullaniciRol kr = new KullaniciRol();
                        bool kayitOlustur = new AdminController().InsertUser(item);
                        
                    }

                    return View();
                }
                else
                {
                    ViewBag.Error = "Dosya tipiniz yanlış, lütfen '.xls' yada '.xlsx' uzantılı dosya yükleyiniz.";

                    return View();
                }
            }
        }
        
    }
}