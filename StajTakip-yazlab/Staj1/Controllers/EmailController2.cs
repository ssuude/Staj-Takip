using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Microsoft.Office.Interop.Excel;
using Staj1.Models;


namespace Staj1.Controllers
{
    public class EmailgondermeController2 : Controller
    {
        // GET: Emailgonderme
        Staj1DB context = new Staj1DB();
        public bool SendMail2(Kullanici model)
        {
            try
            {

                //model.Parola = sifre.ToString();
                var StajDurum = context.StajDurum.Where(x => x.StajDurumID == model.StajDurumID).FirstOrDefault();
                MailMessage sifremail = new MailMessage();
                sifremail.To.Add(model.Mail); // kime , kullanıcı maili çekilecek
                sifremail.From = new MailAddress("koustsmail@gmail.com"); // kimden, değişmeyecek
                sifremail.Subject = "KOU Staj Takip Sistemi Not Durumu";
                sifremail.Body = "Sayın " + model.Adi + " " + model.Soyadi + "<br>" + "<br> <br>" + "Staj Durumunuz: " + StajDurum.StajDurumAdi;
                sifremail.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient();
                smtp.Credentials = new NetworkCredential("koustsmail@gmail.com", "vqrsjwkmzahicyum");
                smtp.Port = 587;
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;



                smtp.Send(sifremail);
                TempData["Message"] = " kullanıcıya iletilmiştir.";

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
    }
}