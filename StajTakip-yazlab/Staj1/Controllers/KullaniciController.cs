﻿using Staj1.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
using Staj1.FormReport;
using Staj1.Controllers;
using System.Web.Routing;

namespace StajTakip.Controllers
{
    public class KullaniciController : Controller
    {
        Staj1DB context = new Staj1DB();

        // GET: Kullanici
        [Authorize(Roles = "Admin,Eğitim Elemanı,Kullanici,SuperAdmin,Komisyon")]
        public ActionResult Index()
        {
            string numara = User.Identity.Name;
            int kullaniciId = context.Kullanici.Where(x => x.Numara == numara).Select(x => x.KullaniciID).FirstOrDefault();
            var listele = context.StajBasvuruBelgeleri.Where(x => x.KullaniciID == kullaniciId).ToList();
            Kullanici kl = context.Kullanici.Where(x => x.KullaniciID == kullaniciId).FirstOrDefault();

            if (listele.Count() == 0)
            {
                kl.StajDurumID = 5;
                context.Entry(kl).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
                return View(listele);
            }

            return View(listele);
        }

        public ActionResult GirisYap()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GirisYap(Staj1.Models.Kullanici kl)
        {
            var kullaniciVarmi = context.Kullanici.FirstOrDefault(x => x.Numara == kl.Numara && x.Parola == kl.Parola);


            if (kullaniciVarmi == null)
            {
                ViewBag.Uyari = "Kullanıcı ad veya şifreniz hatalıdır. Lütfen tekrar deneyiniz. ";
                return View();
            }

            if (!kullaniciVarmi.Status)
            {
                return RedirectToAction("SifremiYenile", new RouteValueDictionary(new { controller = "Kullanici", action = "SifremiYenile", Id = kullaniciVarmi.KullaniciID }));
            }

            var rolid = context.KullaniciRol.Where(x => x.KullaniciID == kullaniciVarmi.KullaniciID).Select(x => x.Rol.RolID == 2).FirstOrDefault();
            var kapat = context.StajBaslatilsinMi.Select(x => x.Adi).FirstOrDefault();

            if (rolid == true)
            {
                if (kapat == "Staj Dönemi Kapat")
                {
                    return RedirectToAction("Uyari", "Kullanici");
                }

                else
                {
                    var stajTamamlandiMi = context.Kullanici.Where(x => x.StajDurum.StajDurumAdi != "Staj 1 Tamamlandı").FirstOrDefault(x => x.Numara == kl.Numara && x.Parola == kl.Parola);

                    if (stajTamamlandiMi == null)
                    {
                        ViewBag.Uyari = "Staj 1 dönemini başarıyla tamamladığınızdan tekrar giriş yapamazsınız";
                        return View();
                    }

                    var kullaniciVarmi1 = context.Kullanici.FirstOrDefault(x => x.Numara == kl.Numara && x.Parola == kl.Parola);

                    if (kullaniciVarmi1 != null)

                    {
                        Session["uyeid"] = kullaniciVarmi1.KullaniciID;

                        FormsAuthentication.SetAuthCookie(kullaniciVarmi1.Numara, kl.BeniHatirla);

                        return RedirectToAction("Index", "Admin");
                    }
                }
            }
            else
            {
                var stajTamamlandiMi = context.Kullanici.Where(x => x.StajDurum.StajDurumAdi != "Staj 1 Tamamlandı").FirstOrDefault(x => x.Numara == kl.Numara && x.Parola == kl.Parola);

                if (stajTamamlandiMi == null)
                {
                    ViewBag.Uyari = "Staj 1 dönemini başarıyla tamamladığınızdan tekrar giriş yapamazsınız";
                    return View();
                }

                var kullaniciVarmi1 = context.Kullanici.FirstOrDefault(x => x.Numara == kl.Numara && x.Parola == kl.Parola);

                if (kullaniciVarmi1 != null)

                {
                    Session["uyeid"] = kullaniciVarmi1.KullaniciID;

                    FormsAuthentication.SetAuthCookie(kullaniciVarmi1.Numara, kl.BeniHatirla);

                    return RedirectToAction("Index", "Admin");
                }
            }
            return View();

        }




        [Authorize(Roles = "Kullanici")]
        public ActionResult DosyaYukle()
        {
            string numara = User.Identity.Name;
            int kullaniciId = context.Kullanici.Where(x => x.Numara == numara).Select(x => x.KullaniciID).FirstOrDefault();
            var listele = context.StajBasvuruBelgeleri.Where(x => x.KullaniciID == kullaniciId).OrderByDescending(x => x.Tarih).Take(5);
            Kullanici kl = context.Kullanici.Where(x => x.KullaniciID == kullaniciId).FirstOrDefault();

            if (listele.Count() == 0)
            {
                kl.StajDurumID = 5;
                context.Entry(kl).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
                return View(listele);
            }

            return View(listele);
        }

        [HttpPost]
        public ActionResult DosyaYukle(IEnumerable<HttpPostedFileBase> files, StajBasvuruBelgeleri sb)
        {
            string numara = User.Identity.Name;
            int kullaniciId = context.Kullanici.Where(x => x.Numara == numara).Select(x => x.KullaniciID).FirstOrDefault();
            var listele = context.StajBasvuruBelgeleri.Where(x => x.KullaniciID == kullaniciId).OrderByDescending(x => x.Tarih).Take(5);
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
                        var originalDirectory = new DirectoryInfo(string.Format("{0}StajBasvuruBelgeleri\\", Server.MapPath(@"\")));

                        string pathString = System.IO.Path.Combine(originalDirectory.ToString(), "Staj1");

                        var fileName1 = gd + "_" + Path.GetFileName(file.FileName);

                        bool isExists = System.IO.Directory.Exists(pathString);

                        if (!isExists)
                            System.IO.Directory.CreateDirectory(pathString);

                        var path = string.Format("{0}\\{1}", pathString, fileName1);
                        file.SaveAs(path);

                        sb.BelgeAdi = fileName1;
                        sb.KullaniciID = kullaniciId;
                        sb.Tarih = DateTime.Now;

                        context.StajBasvuruBelgeleri.Add(sb);
                        context.SaveChanges();

                        kl.StajDurumID = 3;
                        kl.BasvuruYapildiMi = true;
                        context.Entry(kl).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();

                    }
                }
            }
            return View(listele);
        }

        [Authorize(Roles = "Kullanici")]
        public FileResult Download(string file)
        {
            byte[] fileBytes = System.IO.File.ReadAllBytes(Server.MapPath("~/StajBasvuruBelgeleri/Belgeler/" + file + ""));
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, file);
        }

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



        [HttpPost]
        public ActionResult CikisYap()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.RemoveAll();
            Session.Abandon();

            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Kullanici")]
        public ActionResult GonderilenBelgelerim()
        {
            string numara = User.Identity.Name;
            int kullaniciId = context.Kullanici.Where(x => x.Numara == numara).Select(x => x.KullaniciID).FirstOrDefault();
            var listele = context.GeriGonderilenBelgeler.Where(x => x.KullaniciID == kullaniciId).ToList();

            if (listele.Count() != 0)
            {
                ViewBag.Mesaj = "Belge gönderim işleminiz başarıyla gerçekleştirilmiştir. Geri gönderilen belgelerinizi kontrol etmeyi unutmayınız!!";
                return View(listele);
            }

            ViewBag.Mesaj1 = "Geri gönderilmiş belgeniz bulunmamaktadır.";
            return View(listele);

        }

        [Authorize(Roles = "Kullanici")]
        public ActionResult BasvuruBelgelerim()
        {
            string numara = User.Identity.Name;
            int kullaniciId = context.Kullanici.Where(x => x.Numara == numara).Select(x => x.KullaniciID).FirstOrDefault();
            var listele = context.StajBasvuruBelgeleri.Where(x => x.KullaniciID == kullaniciId).OrderByDescending(x => x.Tarih).Take(5);
            Kullanici kl = context.Kullanici.Where(x => x.KullaniciID == kullaniciId).FirstOrDefault();

            if (listele.Count() == 0)
            {
                kl.StajDurumID = 5;
                context.Entry(kl).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
                ViewBag.Mesaj = "Staj başvuru belgeniz bulunmamaktadır.";
                return View(listele);
            }

            ViewBag.Mesaj1 = "Staj başvuru belgeleriniz sisteme kaydedilmiştir.";
            return View(listele);

        }

        public ActionResult Uyari()
        {
            var data = context.StajBaslatilsinMi.FirstOrDefault();

            return View(data);
        }



        [Authorize(Roles = "Kullanici")]
        public ActionResult StajBaslamaFormu()
        {
            string numara = User.Identity.Name;
            int kullaniciId = context.Kullanici.Where(x => x.Numara == numara).Select(x => x.KullaniciID).FirstOrDefault();
            var listele = context.Kullanici.Where(x => x.KullaniciID == kullaniciId).FirstOrDefault();
            StajBasvuruForm form = new StajBasvuruForm
            {
                Numara = listele.Numara,
                Adi = listele.Adi,
                Soyadi = listele.Soyadi
            };

            //return View(listele);
            return View(form);

        }

        [HttpPost]
        public FileContentResult Form(StajBasvuruForm form)
        {
            try
            {
                string numara = User.Identity.Name;
                var kullanici = context.Kullanici.Where(x => x.Numara == numara).FirstOrDefault();

                var stajAdi = context.StajAdi.Where(x => x.Adi == form.Soru1).FirstOrDefault();

                form.Adi = kullanici.Adi;
                form.Soyadi = kullanici.Soyadi;

                OgrenciReport ogrenciReport = new OgrenciReport();
                byte[] abytes = ogrenciReport.ReportPdf(form);


                StajyerOgrenciBaslatma ogrenci = new StajyerOgrenciBaslatma
                {
                    IsyeriAdi = form.Firma,
                    KullaniciID = kullanici.KullaniciID,
                    ToplamCalismaSüresi = form.IsGunu,
                    StajBaslangicTarihi = form.stajBaslangic,
                    StajBitisTarihi = form.stajBitis,
                    Tarih = DateTime.Now,
                    IsyeriAdresi = form.Firma_Adres,
                    Kullanici = kullanici,
                    StajAdi = stajAdi,
                    StajAdiID = stajAdi.ID
                };

                var data = context.StajyerOgrenciBaslatma.Add(ogrenci);
                context.SaveChanges();


                return File(abytes, "application/pdf");
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public List<StajBasvuruForm> GetOgrenciler()
        {
            List<StajBasvuruForm> ogreciler = new List<StajBasvuruForm>();
            StajBasvuruForm ogrenci = new StajBasvuruForm();
            return ogreciler;
        }

        [HttpPost]
        public ActionResult StajBaslamaFormu(string isAdresi, string isAdi, DateTime? stajBaslangic, DateTime? stajBitis, string CumartesiCalisiyorMu, string stajAdi, StajyerOgrenciBaslatma sob)
        {
            //Çalışma Günü Hesaplama
            var ResmiTatil1 = context.ResmiTatiller.Select(x => x.ResmiTatil).ToList();

            DateTime ilkT = Convert.ToDateTime(stajBaslangic);
            DateTime sonT = Convert.ToDateTime(stajBitis);
            int resmiTatil = 0;

            if (CumartesiCalisiyorMu == "Hayır")
            {
                foreach (DateTime rTatil in ResmiTatil1) //Resmi tatil listemizi foreach ile geziyoruz
                {
                    //resmi tatiller hafta sonuna denk geliyorsa aşagıdaki metod ile hafta sonlarını çıkarttığımızdan tekrar saymasına gerek yok
                    //hafta içine denk gelen resmi tatilleri sayıyoruz.
                    if ((rTatil.ToString("dddd") != "Cumartesi" && rTatil.ToString("dddd") != "Pazar") && (rTatil >= ilkT && rTatil <= sonT))
                    {
                        resmiTatil++;
                    }
                }
            }

            if (CumartesiCalisiyorMu == "Evet")
            {
                foreach (DateTime rTatil in ResmiTatil1) //Resmi tatil listemizi foreach ile geziyoruz
                {
                    //resmi tatiller hafta sonuna denk geliyorsa aşagıdaki metod ile hafta sonlarını çıkarttığımızdan tekrar saymasına gerek yok
                    //hafta içine denk gelen resmi tatilleri sayıyoruz.
                    if ((rTatil.ToString("dddd") != "Pazar") && (rTatil >= ilkT && rTatil <= sonT))
                    {
                        resmiTatil++;
                    }
                }
            }


            string numara = User.Identity.Name;
            var kullaniciId = context.Kullanici.Where(x => x.Numara == numara).Select(x => x.KullaniciID).FirstOrDefault();
            var listele = context.Kullanici.Where(x => x.KullaniciID == kullaniciId).FirstOrDefault();

            int data = context.StajAdi.Where(x => x.Adi == stajAdi).Select(x => x.ID).FirstOrDefault();

            if (CumartesiCalisiyorMu == "Hayır")
            {
                int sonuc = CalismaHesapla(ilkT, sonT);
                int tsonuc = resmiTatil;
                int calismasuresi = sonuc - tsonuc;



                sob.HaftaIciGunSayisi = sonuc;
                sob.ResmiTatilSayisi = tsonuc;
                sob.ToplamCalismaSüresi = calismasuresi;
            }

            if (CumartesiCalisiyorMu == "Evet")
            {
                int sonuc = CalismaHesapla1(ilkT, sonT);
                int tsonuc = resmiTatil;
                int calismasuresi = sonuc - tsonuc;



                sob.HaftaIciGunSayisi = sonuc;
                sob.ResmiTatilSayisi = tsonuc;
                sob.ToplamCalismaSüresi = calismasuresi;
            }

            sob.IsyeriAdi = isAdi;
            sob.StajBaslangicTarihi = stajBaslangic;
            sob.StajBitisTarihi = stajBitis;
            sob.CumartesiCalisiyorMu = CumartesiCalisiyorMu;
            sob.StajAdiID = data;
            sob.KullaniciID = kullaniciId;
            sob.Tarih = DateTime.Now;
            sob.IsyeriAdresi = isAdresi;

            context.StajyerOgrenciBaslatma.Add(sob);
            context.SaveChanges();

            ViewBag.Mesaj = "Staj Başvuru Formunuz Başarıyla Kaydedilmiştir.";



            return View(listele);
        }

        private int ResmiTatilHesapla(DateTime? stajBaslangic, DateTime? stajBitis, string CumartesiCalisiyorMu)
        {
            var ResmiTatil1 = context.ResmiTatiller.Select(x => x.ResmiTatil).ToList();

            DateTime ilkT = Convert.ToDateTime(stajBaslangic);
            DateTime sonT = Convert.ToDateTime(stajBitis);
            int resmiTatil = 0;

            if (CumartesiCalisiyorMu == "Hayır")
            {
                foreach (DateTime rTatil in ResmiTatil1) //Resmi tatil listemizi foreach ile geziyoruz
                {
                    //resmi tatiller hafta sonuna denk geliyorsa aşagıdaki metod ile hafta sonlarını çıkarttığımızdan tekrar saymasına gerek yok
                    //hafta içine denk gelen resmi tatilleri sayıyoruz.
                    if ((rTatil.ToString("dddd") != "Cumartesi" && rTatil.ToString("dddd") != "Pazar") && (rTatil >= ilkT && rTatil <= sonT))
                    {
                        resmiTatil++;
                    }
                }
            }

            if (CumartesiCalisiyorMu == "Evet")
            {
                foreach (DateTime rTatil in ResmiTatil1) //Resmi tatil listemizi foreach ile geziyoruz
                {
                    //resmi tatiller hafta sonuna denk geliyorsa aşagıdaki metod ile hafta sonlarını çıkarttığımızdan tekrar saymasına gerek yok
                    //hafta içine denk gelen resmi tatilleri sayıyoruz.
                    if ((rTatil.ToString("dddd") != "Pazar") && (rTatil >= ilkT && rTatil <= sonT))
                    {
                        resmiTatil++;
                    }
                }
            }

            return resmiTatil;
        }

        public int CalismaHesapla(DateTime basTarih, DateTime bitTarih)//bu metod ile iki tarih arasındaki çalışma günlerini sayıyoruz. Cumartesi çalışmıyorsa
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

        public int CalismaHesapla1(DateTime basTarih, DateTime bitTarih)//bu metod ile iki tarih arasındaki çalışma günlerini sayıyoruz. Cumartesi çalışıyorsa.
        {
            DateTime geciciTarih = basTarih;
            int gunSayi = 0;
            string gun = string.Empty;
            while (geciciTarih <= bitTarih)
            {
                gun = geciciTarih.ToString("dddd");

                if (gun != "Pazar")
                {
                    gunSayi++;
                }

                geciciTarih = geciciTarih.AddDays(1);
            }
            return gunSayi;
        }

        [Authorize(Roles = "Kullanici")]
        public ActionResult StajBaslangicFormum()
        {
            string numara = User.Identity.Name;
            int kullaniciId = context.Kullanici.Where(x => x.Numara == numara).Select(x => x.KullaniciID).FirstOrDefault();

            var data = context.StajyerOgrenciBaslatma.Where(x => x.Kullanici.KullaniciID == kullaniciId).OrderByDescending(x => x.Tarih).FirstOrDefault();

            if (data == null)
            {
                ViewBag.Mesaj = "Başvurunuz bulunmamaktadır. Lütfen staj başvuru formunu doldurup kaydediniz.";
                return View();
            }

            ViewBag.Mesaj1 = "Staj başvuru formunuz sisteme kaydedilmiştir. Kaydedilen başvuru/başvurularınızı aşağıda görebilirsiniz.";
            return View(data);
        }

        [Authorize(Roles = "Kullanici")]
        public ActionResult StajBaslangicFormunuGuncelle()
        {
            string numara = User.Identity.Name;
            int kullaniciId = context.Kullanici.Where(x => x.Numara == numara).Select(x => x.KullaniciID).FirstOrDefault();

            var data = context.StajyerOgrenciBaslatma.Where(x => x.Kullanici.KullaniciID == kullaniciId).FirstOrDefault();

            return View(data);
        }

        [HttpPost]
        public ActionResult StajBaslangicFormunuGuncelle(StajyerOgrenciBaslatma sob, string isAdresi, string isAdi, DateTime? stajBaslangic, DateTime? stajBitis, string CumartesiCalisiyorMu, string stajAdi, string Numara, string adres)
        {
            string numara = User.Identity.Name;
            int kullaniciId = context.Kullanici.Where(x => x.Numara == numara).Select(x => x.KullaniciID).FirstOrDefault();
            StajyerOgrenciBaslatma veri = context.StajyerOgrenciBaslatma.Where(x => x.Kullanici.KullaniciID == kullaniciId).FirstOrDefault();
            int data = context.StajAdi.Where(x => x.Adi == stajAdi).Select(x => x.ID).FirstOrDefault();

            var ResmiTatil1 = context.ResmiTatiller.Select(x => x.ResmiTatil).ToList();

            DateTime ilkT = Convert.ToDateTime(stajBaslangic);
            DateTime sonT = Convert.ToDateTime(stajBitis);
            int resmiTatil = 0;

            if (CumartesiCalisiyorMu == "Hayır")
            {
                foreach (DateTime rTatil in ResmiTatil1)
                {

                    if ((rTatil.ToString("dddd") != "Cumartesi" && rTatil.ToString("dddd") != "Pazar") && (rTatil >= ilkT && rTatil <= sonT))
                    {
                        resmiTatil++;
                    }
                }
            }

            if (CumartesiCalisiyorMu == "Evet")
            {
                foreach (DateTime rTatil in ResmiTatil1)
                {

                    if ((rTatil.ToString("dddd") != "Pazar") && (rTatil >= ilkT && rTatil <= sonT))
                    {
                        resmiTatil++;
                    }
                }
            }

            if (CumartesiCalisiyorMu == "Hayır")
            {
                int sonuc = CalismaHesapla(ilkT, sonT);
                int tsonuc = resmiTatil;
                int calismasuresi = sonuc - tsonuc;

                if (calismasuresi > 30 || calismasuresi < 20)
                {
                    ViewBag.Mesaj = "Lütfen staj başlangıç ve bitiş tarihinizi yeniden seçiniz. Toplam çalışma gününüz pazar günü ve resmi tatiller haricinde 20 günden az 30 günden fazla olmamalıdır.";
                    return View(veri);
                }

                veri.HaftaIciGunSayisi = sonuc;
                veri.ResmiTatilSayisi = tsonuc;
                veri.ToplamCalismaSüresi = sonuc - tsonuc;
            }

            if (CumartesiCalisiyorMu == "Evet")
            {
                int sonuc = CalismaHesapla1(ilkT, sonT);
                int tsonuc = resmiTatil;
                int calismasuresi = sonuc - tsonuc;

                if (calismasuresi > 30 || calismasuresi < 20)
                {
                    ViewBag.Mesaj = "Lütfen staj başlangıç ve bitiş tarihinizi yeniden seçiniz. Toplam çalışma gününüz pazar günü ve resmi tatiller haricinde 20 günden az 30 günden fazla olmamalıdır.";
                    return View(veri);
                }

                veri.HaftaIciGunSayisi = sonuc;
                veri.ResmiTatilSayisi = tsonuc;
                veri.ToplamCalismaSüresi = sonuc - tsonuc;
            }



            veri.IsyeriAdi = isAdi;
            veri.Kullanici.Numara = Numara;
            veri.Kullanici.Adres = adres;
            veri.StajBaslangicTarihi = stajBaslangic;
            veri.StajBitisTarihi = stajBitis;
            veri.CumartesiCalisiyorMu = CumartesiCalisiyorMu;
            veri.StajAdiID = data;
            veri.KullaniciID = kullaniciId;
            veri.Tarih = DateTime.Now;
            veri.IsyeriAdresi = isAdresi;

            context.Entry(veri).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();

            return View(veri);
        }

        public void StajBaslangicFormunuSil(int id)
        {
            var data = context.StajyerOgrenciBaslatma.Where(m => m.ID == id).FirstOrDefault();

            context.StajyerOgrenciBaslatma.Remove(data);
            context.Entry(data).State = System.Data.Entity.EntityState.Deleted;
            context.SaveChanges();
        }

        public void BasvuruBelgesiSil(int id)
        {
            var data = context.StajBasvuruBelgeleri.Where(m => m.ID == id).FirstOrDefault();
            Kullanici kl = context.Kullanici.Where(x => x.KullaniciID == data.KullaniciID).FirstOrDefault();

            var dosya = context.StajBasvuruBelgeleri.Where(m => m.ID == id).Select(x => x.BelgeAdi).FirstOrDefault();

            if (dosya != null)
            {
                if (System.IO.File.Exists(Server.MapPath("~/StajBasvuruBelgeleri/Belgeler/" + dosya)))
                {
                    System.IO.File.Delete(Server.MapPath("~/StajBasvuruBelgeleri/Belgeler/" + dosya));
                }
            }

            kl.BasvuruYapildiMi = false;
            context.Entry(kl).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();

            context.StajBasvuruBelgeleri.Remove(data);
            context.Entry(data).State = System.Data.Entity.EntityState.Deleted;
            context.SaveChanges();
        }

        public void GeriGonderilenBelgeSil(int id)
        {
            var data = context.GeriGonderilenBelgeler.Where(m => m.ID == id).FirstOrDefault();

            var dosya = context.GeriGonderilenBelgeler.Where(m => m.ID == id).Select(x => x.BelgeAdi).FirstOrDefault();

            if (dosya != null)
            {
                if (System.IO.File.Exists(Server.MapPath("~/GeriGonderilenEvraklar/Belgeler/" + dosya)))
                {
                    System.IO.File.Delete(Server.MapPath("~/GeriGonderilenEvraklar/Belgeler/" + dosya));

                }
            }
            context.GeriGonderilenBelgeler.Remove(data);
            context.Entry(data).State = System.Data.Entity.EntityState.Deleted;
            context.SaveChanges();
        }

        public ActionResult SifremiUnuttum()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SifremiUnuttum(string Mail)
        {
            var data = context.Kullanici.Where(m => m.Mail == Mail).FirstOrDefault();
            var id = context.Kullanici.Where(m => m.Mail == Mail).Select(x => x.KullaniciID).FirstOrDefault();

            if (data != null)
            {
                Random rnd = new Random();
                int kod = rnd.Next();

                System.Web.HttpContext.Current.Session["Kod"] = kod;

                WebMail.SmtpServer = "smtp.gmail.com";
                WebMail.EnableSsl = true;
                WebMail.UserName = "koustsmail@gmail.com";
                WebMail.Password = "vqrsjwkmzahicyum";

                WebMail.SmtpPort = 587;
                WebMail.Send(data.Mail, "Şifre Yenileme Doğrulama Kodu", "Doğrulama Kodunuz : " + kod);

                ViewBag.Uyari = "Doğrulama kodu mail adresinize gönderildi.";

                return Redirect("/Kullanici/SifremiYenile/" + id);
            }
            else
            {
                ViewBag.Uyari = "Girmiş olduğunuz e posta adresi sistemde bulunmamaktadır. Lütfen yeniden deneyiniz!";
            }

            return View();
        }

        public ActionResult SifremiYenile(int id)
        {
            var sifreunutan = context.Kullanici.Where(m => m.KullaniciID == id).SingleOrDefault();
            return View(sifreunutan);
        }

        [HttpPost]
        public ActionResult SifremiYenile(string sifre, string sifreTekrar, int id)
        {
            var sifreunutan = context.Kullanici.Where(m => m.KullaniciID == id).SingleOrDefault();


            if (sifre == sifreTekrar)
            {
                sifreunutan.Parola = sifre.ToString();
                sifreunutan.Status = true;
                context.SaveChanges();
                Session.Abandon();
                ViewBag.Uyari = "Şifre Değişimi Sağlandı! Giriş Yapabilirsiniz.";
                return RedirectToAction("GirisYap","Kullanici");
            }
            else
            {
                ViewBag.Uyari = "Şifreleriniz eşleşmemektedir. Lütfen yeni parolanızı tekrardan kontrol ediniz!";
            }



            return View();
        }

        [Authorize(Roles = "Kullanici")]
        public ActionResult ProfiliDegistir()
        {
            string numara = User.Identity.Name;
            int kullaniciId = context.Kullanici.Where(x => x.Numara == numara).Select(x => x.KullaniciID).FirstOrDefault();

            var data = context.Kullanici.Where(x => x.KullaniciID == kullaniciId).FirstOrDefault();

            return View(data);
        }

        [HttpPost]
        public ActionResult ProfiliDegistir(string Ad, string Soyad, string Adres, string Mail, string Numara, string TelNo, string Sinif, string Parola)
        {
            string numara = User.Identity.Name;
            int kullaniciId = context.Kullanici.Where(x => x.Numara == numara).Select(x => x.KullaniciID).FirstOrDefault();
            var data = context.Kullanici.Where(x => x.KullaniciID == kullaniciId).FirstOrDefault();
            Kullanici kl = context.Kullanici.Where(x => x.KullaniciID == kullaniciId).FirstOrDefault();

            kl.Adi = Ad;
            kl.Soyadi = Soyad;
            kl.Adres = Adres;
            kl.Mail = Mail;
            kl.Numara = numara;
            kl.TelefonNo = TelNo;
            kl.Sinif = Sinif;
            kl.Parola = Parola;

            context.Entry(kl).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();

            ViewBag.Mesaj = "Bilgileriniz güncellendi";

            return View(data);
        }

        public ActionResult GirisBilgileriAl()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GirisBilgileriAl(Staj1.Models.Kullanici model)
        {
            MailMessage sifremail = new MailMessage();
            sifremail.To.Add(model.Mail); // kime , kullanıcı maili çekilecek
            sifremail.From = new MailAddress("koustsmail@gmail.com"); // kimden, değişmeyecek
            sifremail.Subject = "KOU Staj Takip Sistemi Şifre";
            sifremail.Body = "Sayın " + model.Adi + "<br>" + model.Soyadi + "<br> <br>" + "Şifreniz: " + model.Parola + "<br> <br>" + "Kullanıcı Adınız:" + model.Numara;
            sifremail.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient();
            smtp.Credentials = new NetworkCredential("koustsmail@gmail.com", "vqrsjwkmzahicyum");

            smtp.Port = 587;
            smtp.Host = "smtp.gmail.com";
            smtp.EnableSsl = true;


            try
            {
                smtp.Send(sifremail);
                TempData["Message"] = "Şifre kullanıcıya iletilmiştir.";
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Mesaj gönderilemedi. Hata nedeni: " + ex.Message;
            }

            return View();

        }
    }
}


