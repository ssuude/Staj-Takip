using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Staj1.Models;

namespace StajTakip.Controllers
{
    public class StajDefteriController : Controller
    {
        Staj1DB context = new Staj1DB();
        // GET: StajDefteri

        [Authorize(Roles = "Kullanici")]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Kullanici")]
        public ActionResult TakipCizelgesi()
        {
            string numara = User.Identity.Name;
            int kullaniciId = context.Kullanici.Where(x => x.Numara == numara).Select(x => x.KullaniciID).FirstOrDefault();

            var data = context.Staj.Where(x => x.KullaniciID == kullaniciId).OrderBy(x => x.Tarih).ToList();

            return View(data);
        }

        [Authorize(Roles = "Kullanici")]
        public ActionResult YapilanIsler()
        {
            string numara = User.Identity.Name;
            int kullaniciId = context.Kullanici.Where(x => x.Numara == numara).Select(x => x.KullaniciID).FirstOrDefault();

            var veri = context.Staj.Where(x => x.KullaniciID == kullaniciId && x.Kullanici.StajDurumID == 1).OrderBy(x => x.Tarih).ToList();
            ViewBag.data = veri.Count();

            //var data = context.Staj.OrderBy(x => x.Tarih).ToList();
            return View(veri);
        }


        //public class gunlukModel
        //{
        //    public int id{ get; set; }
        //    public string baslik { get; set; }
        //    public string tarih  { get; set; }
        //    public string ckeditor { get; set; }
        //}


        [HttpGet]
        [Authorize(Roles = "Kullanici")]
        public ActionResult GunlukYaz(int? id = 0)
        {
            string numara = User.Identity.Name;
            int kullaniciId = context.Kullanici.Where(x => x.Numara == numara).Select(x => x.KullaniciID).FirstOrDefault();

            Staj Gmodel = new Staj();
            if (id != 0)
            {
                // Eski kaydı getir güncelleme için tamammı
                var gunluk = context.Staj.Where(x => x.StajID == id).FirstOrDefault();

                Gmodel = new Staj()
                {

                    StajID = gunluk.StajID,
                    Baslik = gunluk.Baslik,
                    Tarih = gunluk.Tarih,
                    Icerik = gunluk.Icerik,
                    KullaniciID = gunluk.KullaniciID
                    //GunNo = gunluk.GunNo
                };

            }

            return View(Gmodel);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult GunlukYaz(Staj model)
        {
            string numara = User.Identity.Name;
            int kullaniciId = context.Kullanici.Where(x => x.Numara == numara).Select(x => x.KullaniciID).FirstOrDefault();
            Staj s = new Staj();

            var veri = context.Kullanici.Where(x => x.KullaniciID == kullaniciId).Select(x => x.StajDurum.StajDurumID == 1).FirstOrDefault();

            ViewBag.data = veri;
            s.StajID = model.StajID;
            s.Baslik = model.Baslik;
            s.Icerik = model.Icerik;
            s.Tarih = Convert.ToDateTime(model.Tarih);
            s.KullaniciID = kullaniciId;


            if (model.StajID != 0)
            {

                context.Entry(s).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
                return Redirect("/StajDefteri/YapilanIsler/");



                ///StajDefteri/GunlukYaz/60
            }  /// id geliyorsa güncelleme yapıyor
            else
            { context.Staj.Add(s); }                                               /// id gelmiyorsa yeni kayıt ekliyor


            context.SaveChanges();

            //ViewBag.Mesaj1 = "Staj defterinizin" + " " + model.GunNo + " numaralı ve " + model.Tarih.ToString().TrimEnd('0', ':') + " tarihli günü sisteme kaydedilmiştir. Defterinizin son durumunu sistem üzerinden kontrol edebilirsiniz.";
            return Redirect("/StajDefteri/YapilanIsler/");
        }



  
        public void GunSil(int id)
        {
            var data = context.Staj.Where(m => m.StajID == id).FirstOrDefault();

            //if(System.IO.File.Exists(Server.MapPath("~/Resimler/" + data.)))

            context.Staj.Remove(data);
            context.Entry(data).State = System.Data.Entity.EntityState.Deleted;
            context.SaveChanges();
        }

        [HttpPost]
        public JsonResult Upload(HttpPostedFileBase upload)
        {
            string gd = Guid.NewGuid().ToString().Substring(0, 8);
            string savePath = "/StajDefterResimleri/images/";
            string dirPath = Server.MapPath(savePath);

            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);


            var fileName = Path.GetFileName(gd + "_" + upload.FileName);


            upload.SaveAs(dirPath + "/" + fileName);

            return Json(new
            {
                uploaded = 1,
                fileName = fileName,
                url = savePath + fileName
            });
        }

        [Authorize(Roles = "Kullanici")]
        public ActionResult StajDefterim()
        {
            string numara = User.Identity.Name;
            int kullaniciId = context.Kullanici.Where(x => x.Numara == numara).Select(x => x.KullaniciID).FirstOrDefault();

            var veri = context.Staj.Where(x => x.KullaniciID == kullaniciId && x.Kullanici.StajDurumID == 1).OrderBy(x => x.Tarih).ToList();
            ViewBag.data = veri.Count();

            return View(veri);
        }

        [Authorize(Roles = "Kullanici")]
        public ActionResult StajDefteriniYukle()
        {
            string numara = User.Identity.Name;
            int kullaniciId = context.Kullanici.Where(x => x.Numara == numara).Select(x => x.KullaniciID).FirstOrDefault();
            var listele = context.StajDefteriTeslim.Where(x => x.KullaniciID == kullaniciId).ToList();

            return View(listele);
        }

        [HttpPost]
        public ActionResult StajDefteriniYukle(IEnumerable<HttpPostedFileBase> files, StajDefteriTeslim sdt)
        {
            string numara = User.Identity.Name;
            int kullaniciId = context.Kullanici.Where(x => x.Numara == numara).Select(x => x.KullaniciID).FirstOrDefault();
            var listele = context.StajDefteriTeslim.Where(x => x.KullaniciID == kullaniciId).ToList();
            Kullanici kl = context.Kullanici.Where(x => x.KullaniciID == kullaniciId).FirstOrDefault();

            string gd = Guid.NewGuid().ToString().Substring(0, 8);
            string fName = "";

            if (files != null)
            {
                foreach (var file in files)
                {
                    fName = file.FileName;
                    if (file != null && file.ContentLength > 0)
                    {
                        var originalDirectory = new DirectoryInfo(string.Format("{0}StajDefterleri\\", Server.MapPath(@"\")));

                        string pathString = System.IO.Path.Combine(originalDirectory.ToString(), "Defterler");

                        var fileName1 = gd + "_" + Path.GetFileName(file.FileName);

                        bool isExists = System.IO.Directory.Exists(pathString);

                        if (!isExists)
                            System.IO.Directory.CreateDirectory(pathString);

                        var path = string.Format("{0}\\{1}", pathString, fileName1);
                        file.SaveAs(path);

                        sdt.DosyaAdi = fileName1;
                        sdt.KullaniciID = kullaniciId;
                        sdt.Tarih = DateTime.Now;

                        context.StajDefteriTeslim.Add(sdt);
                        context.SaveChanges();
                    }
                }
            }
            return View(listele);
        }

        public FileResult Download(string file)
        {
            byte[] fileBytes = System.IO.File.ReadAllBytes(Server.MapPath("~/StajDefterleri/Defterler/" + file + ""));
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, file);
        }

        public void DosyaSil(int id)
        {
            var data = context.StajDefteriTeslim.Where(m => m.ID == id).FirstOrDefault();

            var defterAdi = context.StajDefteriTeslim.Where(m => m.ID == id).Select(x => x.DosyaAdi).FirstOrDefault();

            if (defterAdi != null)
            {
                if (System.IO.File.Exists(Server.MapPath("~/StajDefterleri/Defterler/" + defterAdi)))
                {
                    System.IO.File.Delete(Server.MapPath("~/StajDefterleri/Defterler/" + defterAdi));
                }
            }

            context.StajDefteriTeslim.Remove(data);
            context.Entry(data).State = System.Data.Entity.EntityState.Deleted;
            context.SaveChanges();
        }


        public bool DateCheck(string CurrentDate)
        {
            Boolean result = false;

            if (CurrentDate != null)
            {
                DateTime CheckDate = Convert.ToDateTime(CurrentDate);
                var staj = context.Staj.Where(x => x.Tarih == CheckDate).FirstOrDefault();
                if (staj != null) result = true;
            }
            return result;
        }
    }
}