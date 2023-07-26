
using Microsoft.Extensions.Options;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Staj1.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using PagedList;
using PagedList.Mvc;


namespace Staj1.Controllers
{
    public class AdminController : Controller
    {
        Staj1DB context = new Staj1DB();
        List<DateTime> ResmiTatil = new List<DateTime>();
        // GET: Admin

        public class kullanicilModel
        {
            public List<Models.Kullanici> kullanici { get; set; }
            public List<StajDurum> stajdurum { get; set; }

        }

        [Authorize(Roles = "Admin,SuperAdmin,Kullanici,Komisyon,Eğitim Elemanı")]
        public ActionResult Index()

        {

            

{

                var kullanicilar = context.Kullanici.Where(x => x.OgretmenId == 2419).ToList();

                List<KullaniciRol> kullaniciRolList = new List<KullaniciRol>();

                foreach (var item in kullanicilar)
                {
                    var dataKRol = context.KullaniciRol.Where(x => x.KullaniciID == item.KullaniciID).FirstOrDefault();
                    kullaniciRolList.Add(dataKRol);
                }



                return View(kullaniciRolList);
            }
        }

        public void OgrenciKaydiSil(int id)
        {
            var data = context.Kullanici.Where(x => x.KullaniciID == id).FirstOrDefault();

            data.Status = true;

            context.Entry(data).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();
        }

        [HttpPost]
        public JsonResult StajBasvuruBelgesiGetir(int id)
        {
            var dosya = context.StajBasvuruBelgeleri.Include("Kullanici").Where(m => m.KullaniciID == id).OrderByDescending(x => x.Tarih).Take(5);

            List<StajBasvuruBelgeleri> son = new List<StajBasvuruBelgeleri>();

            ViewBag.liste = context.StajDurum.ToList();

            foreach (var item in dosya)
            {
                son.Add(new StajBasvuruBelgeleri
                {
                    ID = item.ID,
                    BelgeAdi = item.BelgeAdi,
                    KullaniciID = item.KullaniciID,
                    Tarih = item.Tarih
                });
            }
            return Json(son);
        }


        [Authorize(Roles = "Admin,Eğitim Elemanı,SuperAdmin")]
        public ActionResult DosyaYukle()
        {
            var kullaniciresult = context.Kullanici.Where(x => x.OnaylandiMi == false).ToList();

            kullanicilModel result = new kullanicilModel
            {
                kullanici = kullaniciresult
            };

            return View(result);
        }

        [HttpPost]
        public ActionResult DosyaYukle(IEnumerable<HttpPostedFileBase> files, GeriGonderilenBelgeler ggb, string aciklama, int KullaniciID)
        {
            string numara = User.Identity.Name;
            int kullaniciId = context.Kullanici.Where(x => x.Numara == numara).Select(x => x.KullaniciID).FirstOrDefault();

            string gd = Guid.NewGuid().ToString().Substring(0, 8);
            string fName = "";

            var kullaniciresult = context.Kullanici.Where(x => x.OnaylandiMi == false).ToList();

            kullanicilModel result = new kullanicilModel
            {
                kullanici = kullaniciresult
            };

            if (files == null)
            {
                if (aciklama != null)
                {
                    ggb.Aciklama = aciklama;
                    ggb.BelgeAdi = "";
                    ggb.Tarih = DateTime.Now;
                    ggb.KullaniciID = KullaniciID;

                    context.GeriGonderilenBelgeler.Add(ggb);
                    context.SaveChanges();

                    return View(result);
                }
            }

            if (files != null)
            {
                foreach (var file in files)
                {
                    fName = file.FileName;
                    if (file != null && file.ContentLength > 0)
                    {
                        var originalDirectory = new DirectoryInfo(string.Format("{0}GeriGonderilenEvraklar\\", Server.MapPath(@"\")));

                        string pathString = System.IO.Path.Combine(originalDirectory.ToString(), "Belgeler");

                        var fileName1 = gd + "_" + Path.GetFileName(file.FileName);

                        bool isExists = System.IO.Directory.Exists(pathString);

                        if (!isExists)
                            System.IO.Directory.CreateDirectory(pathString);

                        var path = string.Format("{0}\\{1}", pathString, fileName1);
                        file.SaveAs(path);

                        ggb.BelgeAdi = fileName1;
                        ggb.KullaniciID = KullaniciID;
                        ggb.Tarih = DateTime.Now;
                        ggb.Aciklama = aciklama;

                        context.GeriGonderilenBelgeler.Add(ggb);
                        context.SaveChanges();
                    }
                }
            }
            return View(result);
        }

        [Authorize(Roles = "Admin,Kullanici,Eğitim Elemanı,SuperAdmin,Komisyon")]
        public FileResult Download(string file)
        {
            byte[] fileBytes = System.IO.File.ReadAllBytes(Server.MapPath("~/GeriGonderilenEvraklar/Belgeler/" + file + ""));
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, file);
        }

        [Authorize(Roles = "Admin,Eğitim Elemanı")]
        public ActionResult Goster(string dosya)
        {
            FileInfo file = new FileInfo(Server.MapPath("~/StajBasvuruBelgeleri/Staj1/" + dosya + ""));
            FileInfo file1 = new FileInfo(Server.MapPath("~/GeriGonderilenEvraklar/Belgeler/" + dosya + ""));
            FileInfo file2 = new FileInfo(Server.MapPath("~/StajDefterleri/Defterler/" + dosya + ""));

            if (file.Exists)
            {
                Response.ContentType = "application/pdf";
                Response.Clear();
                Response.TransmitFile(file.FullName);
                Response.End();
            }


            else if (file1.Exists)
            {
                Response.ContentType = "application/pdf";
                Response.Clear();
                Response.TransmitFile(file1.FullName);
                Response.End();
            }

            else if (file2.Exists)
            {
                Response.ContentType = "application/pdf";
                Response.Clear();
                Response.TransmitFile(file2.FullName);
                Response.End();
            }

            return View();
        }

        public ActionResult KayitOlustur()
        {
            return View();
        }


        [HttpPost]


        public ActionResult KayitOlustur(Models.Kullanici kl, string Parola, string sifre1)
        {
            var insertUser = InsertUser(kl);
            return View();

            
        }

        [HttpPost]

        public bool InsertUser(Kullanici kl)
        {

            try
            {
                kl.OnaylandiMi = false;
                kl.AktifMi = false;
                kl.KayıtTarihi = DateTime.Now;
                kl.StajDurumID = 5;
                kl.StajBaslatilsinMi = false;
                kl.Status = false;

                int length = 8;
                const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
                StringBuilder sifre = new StringBuilder();
                Random rnd = new Random();
                while (0 < length--)
                {
                    sifre.Append(valid[rnd.Next(valid.Length)]);
                }

                kl.Parola = sifre.ToString();
                int nmr = Convert.ToInt32(kl.Numara);
                context.Kullanici.Add(kl);
                context.SaveChanges();
                ViewBag.Mesaj = "Üyelik işleminiz başarıyla gerçekleştirilmiştir.";

                Rol kullanici = context.Rol.FirstOrDefault(x => x.RolAdi == "Kullanici");
                KullaniciRol kr = new KullaniciRol();

                kr.RolID = kl.Numara.Length > 9999 ? kullanici.RolID : 2;
                kr.KullaniciID = Convert.ToInt32(kl.Numara);
                kr.Kullanici = kl;

                //if (kl.Status == 0)
                //{
                //    // git şifre sıfırla ve Status alanını 1 ile değiştir.
                //}
                


                context.KullaniciRol.Add(kr);
                context.SaveChanges();
                bool mailStatus = new EmailgondermeController().SendMail(kl);
                if (!mailStatus)
                {
                    ViewBag.Mesaj = "Mail gönderilemedi.";
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }




        [Authorize(Roles = "Admin,Kullanici,Eğitim Elemanı")]
        public ActionResult DosyaSil(string adi)
        {
            var dosya = context.GeriGonderilenBelgeler.Where(m => m.BelgeAdi == adi).SingleOrDefault();
            return View(dosya);
        }

        [HttpPost]
        public ActionResult DosyaSil(string adi, FormCollection collection)
        {
            var dosya = context.GeriGonderilenBelgeler.Where(m => m.BelgeAdi == adi).FirstOrDefault();

            if (dosya == null)
            {
                return RedirectToAction("DosyaYukle", "Admin");
            }
            if (System.IO.File.Exists(Server.MapPath("~/GeriGonderilenEvraklar/Belgeler/" + dosya.BelgeAdi)))
            {
                System.IO.File.Delete(Server.MapPath("~/GeriGonderilenEvraklar/Belgeler/" + dosya.BelgeAdi));

            }

            context.GeriGonderilenBelgeler.Remove(dosya);
            context.Entry(dosya).State = System.Data.Entity.EntityState.Deleted;
            context.SaveChanges();

            return RedirectToAction("DosyaYukle", "Admin");
        }

        [HttpPost]
        public void StajBasvuruBelgesiSil(int id)
        {
            var dosya = context.StajBasvuruBelgeleri.Where(m => m.ID == id).FirstOrDefault();

            if (dosya != null && System.IO.File.Exists(Server.MapPath("~/StajBasvuruBelgeleri/Belgeler/" + dosya.BelgeAdi)))
            {
                System.IO.File.Delete(Server.MapPath("~/StajBasvuruBelgeleri/Belgeler/" + dosya.BelgeAdi));

            }

            context.StajBasvuruBelgeleri.Remove(dosya);
            context.Entry(dosya).State = System.Data.Entity.EntityState.Deleted;
            context.SaveChanges();
        }

        public void GeriGonderilenBelgeSil(int id)
        {
            var dosya = context.GeriGonderilenBelgeler.Where(m => m.ID == id).FirstOrDefault();

            if (dosya != null && System.IO.File.Exists(Server.MapPath("~/StajBasvuruBelgeleri/Belgeler/" + dosya.BelgeAdi)))
            {
                System.IO.File.Delete(Server.MapPath("~/StajBasvuruBelgeleri/Belgeler/" + dosya.BelgeAdi));

            }

            context.GeriGonderilenBelgeler.Remove(dosya);
            context.Entry(dosya).State = System.Data.Entity.EntityState.Deleted;
            context.SaveChanges();
        }


        [HttpGet]
        public ActionResult CikisYap()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.RemoveAll();
            Session.Abandon();

            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Admin,Eğitim Elemanı,Komisyon,SuperAdmin")]
        public ActionResult BasvuruDosyalari()
        {
            var kullaniciresult = context.Kullanici.Where(x => x.OnaylandiMi == false).ToList();

            kullanicilModel result = new kullanicilModel
            {
                kullanici = kullaniciresult
            };

            ViewBag.liste = context.StajDurum.Where(x => x.Gizle == true).ToList();

            //ViewBag.liste = context.StajDurum.ToList();

            return View(result);
        }

        public JsonResult BasvuruDurumu(int? id)
        {

           
            ViewBag.liste = context.StajDurum.Where(x => x.Gizle == true).Select(x => x.StajDurumAdi).ToList();

            List<Kullanici> kullaniciliste = context.Kullanici.Where(f => f.KullaniciID == id).OrderBy(f => f.StajDurum.StajDurumAdi).ToList();

            List<SelectListItem> itemlist = (from i in kullaniciliste
                                             select new SelectListItem
                                             {
                                                 Value = i.StajDurumID.ToString(),
                                                 Text = i.StajDurum.StajDurumAdi

                                             }).ToList();


            return Json(itemlist, JsonRequestBehavior.AllowGet);

            //    bool mailStatus = new EmailgondermeController().SendMail(kullaniciresult);
            //    if (!mailStatus)
            //    {
            //        ViewBag.Mesaj = "Mail gönderilemedi.";
            //    }

            //    return true;
            //}
            //    catch (Exception ex)
            //    {
            //        return false;
            //    }
        }

        [HttpPost]
        public ActionResult BasvuruDosyalari(int? stajDurumID, int? KullaniciID)
        {
            string numara = User.Identity.Name;
            int kullaniciId = context.Kullanici.Where(x => x.Numara == numara).Select(x => x.KullaniciID).FirstOrDefault();
            Kullanici kullanici = context.Kullanici.Where(x => x.KullaniciID == kullaniciId).FirstOrDefault();

            var kullaniciresult = context.Kullanici.Where(x => x.OnaylandiMi == false).ToList();

            kullanicilModel result = new kullanicilModel
            {
                kullanici = kullaniciresult
            };

            ViewBag.liste = context.StajDurum.ToList();

            Kullanici kl = context.Kullanici.Where(x => x.KullaniciID == KullaniciID).FirstOrDefault();

            if (kl != null)
            {
                kl.StajDurumID = stajDurumID;
                kl.BasvuruyuDegerlendiren = kullanici.Adi + " " + kullanici.Soyadi;
                kl.BasvuruDegerlendirmeTarihi = DateTime.Now;
                context.Entry(kl).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();

                bool mailStatus = new EmailgondermeController2().SendMail2(kl);
                if (!mailStatus)
                {
                    ViewBag.Uyari = "Mail gönderilemedi.";
                }
                return View(result);
            }

            else
            {
                ViewBag.Uyari = "Lütfen öğrenci bilgisini seçiniz.";
                return View(result);
            }
        }

        [Authorize(Roles = "Admin,SuperAdmin,Komisyon")]
        public ActionResult StajDonemiBaslatDurdur()
        {
            var data = context.StajBaslatilsinMi.Where(x => x.ID == 1).FirstOrDefault();

            return View(data);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,SuperAdmin,Komisyon")]
        public ActionResult StajDonemiBaslatDurdur(string StajBaslatilsinMi, string aciklama)
        {
            StajBaslatilsinMi data = context.StajBaslatilsinMi.Where(x => x.ID == 1).FirstOrDefault();
            data.Adi = StajBaslatilsinMi;
            data.Aciklama = aciklama;

            context.StajBaslatilsinMi.Add(data);
            context.Entry(data).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();

            if (StajBaslatilsinMi == "Seçiniz")
            {
                ViewBag.Mesaj = "Lütfen staj dönemi bilgisini seçiniz.";
                return View(data);
            }

            ViewBag.Mesaj = data.Adi + "ılmıştır";

            return View(data);
        }




        [Authorize(Roles = "Admin,Eğitim Elemanı,Komisyon,SuperAdmin")]
        public ActionResult StajBasvuruListesi()
        {
            var data = context.StajyerOgrenciBaslatma.Where(x => x.Kullanici.StajDurumID == 1).ToList();

            return View(data);
        }

        public void BasvuruListesi()
        {
            
        }

        public void BasvurusuOnaylananlarListe()
        {
            
        }

        [Authorize(Roles = "Admin,Eğitim Elemanı,SuperAdmin")]
        public ActionResult TatilGunleri()
        {
            var data = context.ResmiTatiller.OrderBy(x => x.ResmiTatil).ToList();

            return View(data);
        }

        [HttpPost]
        public ActionResult TatilGunleri(ResmiTatiller rt, DateTime? resmitatiller, string Aciklama)
        {
            rt.ResmiTatil = resmitatiller;
            rt.Aciklama = Aciklama;
            context.ResmiTatiller.Add(rt);
            context.SaveChanges();
            Response.Redirect("TatilGunleri", true);

            var data = context.ResmiTatiller.ToList();

            return View(data);
        }


        public void GunSil(int id)
        {
            var data = context.ResmiTatiller.Where(m => m.ID == id).FirstOrDefault();

            context.ResmiTatiller.Remove(data);
            context.Entry(data).State = System.Data.Entity.EntityState.Deleted;
            context.SaveChanges();
        }

        public static int CalismaHesapla(DateTime basTarih, DateTime bitTarih)//bu metod ile iki tarih arasındaki çalışma günlerini sayıyoruz
        {
            DateTime geciciTarih = basTarih;
            int gunSayi = 0;
            string gun = string.Empty;
            while (geciciTarih <= bitTarih)
            {
                gun = geciciTarih.ToString("dddd");
                if (gun != "Cumartesi" && gun != "Pazar")
                {
                    gunSayi++;
                }
                geciciTarih = geciciTarih.AddDays(1);
            }
            return gunSayi;
        }

        [Authorize(Roles = "Admin,Eğitim Elemanı,Komisyon,SuperAdmin")]
        public ActionResult StajyerOgrenciStajaBaslatmaBilgileri()
        {
            var data = context.StajyerOgrenciBaslatma.OrderBy(x => x.StajBaslangicTarihi).ToList();

            return View(data);
        }

        [HttpPost]
        public ActionResult StajyerOgrenciStajaBaslatmaBilgileri(DateTime? baslangicTarih, DateTime? bitisTarih)
        {
            var result = context.StajyerOgrenciBaslatma.Where(entry => entry.StajBaslangicTarihi >= baslangicTarih.Value).Where(entry => entry.StajBaslangicTarihi <= bitisTarih.Value).OrderBy(x => x.StajBaslangicTarihi).ToList();

            if (result.Count() == 0)
            {
                ViewBag.Mesaj = "Seçili tarihler arasında kayıt bulunamadı.";
                return View();
            }

            ViewBag.Mesaj1 = baslangicTarih.Value.Date.ToString().TrimEnd('0', ':') + " ve " + bitisTarih.Value.Date.ToString().TrimEnd('0', ':') + " " + " staj başlangıç tarihli öğrencilerin kayıtları listelenmiştir.";
            return View(result);
        }

        public void StajyerOgrenciStajaBaslatmaBilgileriSil(int id)
        {
            var data = context.StajyerOgrenciBaslatma.Where(m => m.ID == id).FirstOrDefault();

            context.StajyerOgrenciBaslatma.Remove(data);
            context.Entry(data).State = System.Data.Entity.EntityState.Deleted;
            context.SaveChanges();
        }

        public void StajyerOgrenciStajaBaslatmaFormu(DateTime? baslangicTarih, DateTime? bitisTarih)
        {
            
        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        public ActionResult YetkiVer()
        {
            var data = context.KullaniciRol.Where(x => x.Kullanici.OnaylandiMi == false).ToList();

            return View(data);
        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        public ActionResult YetkiBelirle(int id)
        {
            int kullaniciId = context.Kullanici.Where(x => x.KullaniciID == id).Select(x => x.KullaniciID).FirstOrDefault();
            var data = context.KullaniciRol.Where(x => x.Kullanici.KullaniciID == kullaniciId).FirstOrDefault();

            return View(data);
        }

        [HttpPost]
        public ActionResult YetkiBelirle(string data, int id)
        {
            if (data == "Seçiniz")
            {
                ViewBag.Mesaj = "Lütfen yetkilendirme bilgisini seçiniz";
                return View("YetkiBelirle");
            }

            KullaniciRol kl = context.KullaniciRol.Where(x => x.KullaniciID == id).FirstOrDefault();
            var rol = context.Rol.Where(x => x.RolAdi == data).Select(x => x.RolID).FirstOrDefault();

            context.Entry(kl).State = System.Data.Entity.EntityState.Deleted;
            context.SaveChanges();

            kl.RolID = rol;
            kl.KullaniciID = id;

            context.KullaniciRol.Add(kl);
            context.SaveChanges();

            ViewBag.Mesaj1 = kl.Kullanici.Adi + " " + kl.Kullanici.Soyadi + " " + "'ın yetkilendirmesi " + data + " olarak seçilmiştir.";
            return View(kl);
        }

        [Authorize(Roles = "Admin,Eğitim Elemanı,Komisyon,SuperAdmin")]
        public ActionResult StajyerOgrenciStajDefterleri()
        {
            var data = context.StajDefteriTeslim.ToList();

            return View(data);
        }

        public FileResult Download1(string file)
        {
            byte[] fileBytes = System.IO.File.ReadAllBytes(Server.MapPath("~/StajDefterleri/Defterler/" + file + ""));
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, file);
        }

        [Authorize(Roles = "Admin,Eğitim Elemanı,Komisyon,SuperAdmin")]
        public ActionResult OgrenciStajDefteri(int? id)
        {
            var veriVarmi = context.Staj.Where(x => x.KullaniciID == id).OrderBy(x => x.Tarih).ToList();

            if (veriVarmi.Count() == 0)
            {
                ViewBag.Mesaj = "Sisteme kayıtlı staj defteri bulunmamaktadır.";
                return View();
            }

            int kullaniciId = context.Kullanici.Where(x => x.KullaniciID == id).Select(x => x.KullaniciID).FirstOrDefault();
            var ogrenciAd = context.Staj.Where(x => x.Kullanici.KullaniciID == kullaniciId).Select(x => x.Kullanici.Adi).FirstOrDefault();
            var ogrenciSinif = context.Staj.Where(x => x.Kullanici.KullaniciID == kullaniciId).Select(x => x.Kullanici.Sinif).FirstOrDefault();
            var ogrenciSoyad = context.Staj.Where(x => x.Kullanici.KullaniciID == kullaniciId).Select(x => x.Kullanici.Soyadi).FirstOrDefault();
            var ogrenciNumara = context.Staj.Where(x => x.Kullanici.KullaniciID == kullaniciId).Select(x => x.Kullanici.Numara).FirstOrDefault();
            var stajbaslangic = context.StajyerOgrenciBaslatma.Where(x => x.KullaniciID == kullaniciId).Select(x => x.StajBaslangicTarihi).FirstOrDefault();
            var stajbitis = context.StajyerOgrenciBaslatma.Where(x => x.KullaniciID == kullaniciId).Select(x => x.StajBitisTarihi).FirstOrDefault();
            var calismasuresi = context.StajyerOgrenciBaslatma.Where(x => x.KullaniciID == kullaniciId).Select(x => x.ToplamCalismaSüresi).FirstOrDefault();

            ViewBag.veriVarmi = veriVarmi;
            ViewBag.ogrenciAd = ogrenciAd;
            ViewBag.ogrenciSoyad = ogrenciSoyad;
            ViewBag.ogrenciNumara = ogrenciNumara;
            ViewBag.ogrenciSinif = ogrenciSinif;
            ViewBag.ogrenciStajbaslangic = stajbaslangic;
            ViewBag.ogrenciStajbitis = stajbitis;
            ViewBag.ogrenciCalismasuresi = calismasuresi;

            return View(veriVarmi);
        }

        [Authorize(Roles = "Admin,Eğitim Elemanı,Komisyon,SuperAdmin")]
        public ActionResult BasvurusuTamamlananlarınListesi()
        {
            var data = context.Kullanici.Where(x => x.StajDurumID == 8).ToList();

            return View(data);
        }

        [HttpPost]
        public ActionResult BasvurusuTamamlananlarınListesi(DateTime? baslangicTarih, DateTime? bitisTarih)
        {
            var result = context.Kullanici.Where(entry => entry.BasvuruDegerlendirmeTarihi >= baslangicTarih.Value).Where(entry => entry.BasvuruDegerlendirmeTarihi <= bitisTarih.Value).Where(x => x.StajDurum.StajDurumID == 8).ToList();

            if (result.Count() == 0)
            {
                ViewBag.Mesaj = "Seçili tarihler arasında kayıt bulunamadı.";
                return View();
            }

            ViewBag.Mesaj1 = baslangicTarih.Value.Date.ToString().TrimEnd('0', ':') + " ve " + bitisTarih.Value.Date.ToString().TrimEnd('0', ':') + " " + " tarihleri arasındaki kayıtlar listelenmiştir.";
            return View(result);
        }
    }
}


       
