using iTextSharp.text;
using iTextSharp.text.pdf;
using Org.BouncyCastle.Bcpg;
using Staj1.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Services.Description;
using System.Web.UI.WebControls;
using System.Xml.Serialization;
using Image = System.Web.UI.WebControls.Image;

namespace Staj1.FormReport
{
    public class OgrenciReport
    {


        int toplamSutun = 4;
        Document document;
        Font fontstyle;
        private Font fontStyle;
        PdfPTable pdfTable = new PdfPTable(4);
        PdfPTable pdfTable2 = new PdfPTable(3);
        PdfPCell pdfPCell;
        MemoryStream memoryStream = new MemoryStream();
        StajBasvuruForm ogrenciler = new StajBasvuruForm();



        public byte[] ReportPdf(StajBasvuruForm form)
        {


            ogrenciler = form;
            document = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            document.SetPageSize(PageSize.A4);
            document.SetMargins(20f, 20f, 20f, 20f);
            pdfTable.WidthPercentage = 100;
            pdfTable2.WidthPercentage = 100;
            pdfTable.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfTable2.HorizontalAlignment = Element.ALIGN_CENTER;
            fontStyle = FontFactory.GetFont("Times New Roman", 8f, 1);
            PdfWriter.GetInstance(document, memoryStream);
            document.Open();
            pdfTable.SetWidths(new float[] { 30f, 80f, 30f, 80f });
            pdfTable2.SetWidths(new float[] { 120f, 120f, 80f });




            this.ReportPdfHeader();
            this.ReportPdfBody();
            this.ReportPdfFoot();

            pdfTable.HeaderRows = 2;
            document.Add(pdfTable);
            document.Add(pdfTable2);
            document.Close();
            return memoryStream.ToArray();
        }

        public void ReportPdfHeader()
        {

            fontStyle = FontFactory.GetFont("Times New Roman", 11f, 1);
            pdfPCell = new PdfPCell(new Phrase("T.C.\r\nKOCAELİ ÜNİVERSİTESİ\r\nTEKNOLOJİ FAKÜLTESİ\r\n(Staj Başvuru ve Kabul formu)\r\n \r\n \r\n \r\n ", fontStyle));
            pdfPCell.Colspan = toplamSutun;
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.Border = 0;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            pdfTable.AddCell(pdfPCell);
            pdfTable.CompleteRow();

            fontStyle = FontFactory.GetFont("Times New Roman", 9f, 0);
            pdfPCell = new PdfPCell(new Phrase("Tarih: ../ .. / ......", fontStyle));
            pdfPCell.Colspan = toplamSutun;
            pdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            pdfPCell.Border = 0;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            pdfTable.AddCell(pdfPCell);
            pdfTable.CompleteRow();

            fontStyle = FontFactory.GetFont("Times New Roman", 9f, 1);
            pdfPCell = new PdfPCell(new Phrase("Ilgili Makama", fontStyle));
            pdfPCell.Colspan = toplamSutun;
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.Border = 0;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            pdfTable.AddCell(pdfPCell);
            pdfTable.CompleteRow();

            fontStyle = FontFactory.GetFont("Times New Roman", 8f, 0);
            pdfPCell = new PdfPCell(new Phrase("Teknoloji Fakültesi Bilisim Sistemleri Mühendisligi Bölümü  _ _ _ _ _ _ _ _ _ numaralı öğrencisiyim.\r\n" +
                "            Kurumunuzda staj yapmamın uygun görülmesi halinde bu formun alttaki kısmını doldurularak fakültemiz ilgili bölüm başkanlığına \r\n" +
                " gönderilmesini saygılarımla arz ederim.\r\nIsyeri uygulaması süresi içerisinde alınan rapor, istirahat vb. belgelerin aslını alınan gün içerisinde bölüm başkanlığına\r\n bildireceğimi beyan ve taahhüt ederim.\r\n", fontStyle));
            pdfPCell.Colspan = toplamSutun;
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.Border = 0;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            pdfTable.AddCell(pdfPCell);
            pdfTable.CompleteRow();


            fontStyle = FontFactory.GetFont("Times New Roman", 6f, 0);
            pdfPCell = new PdfPCell(new Phrase(" ", fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            pdfPCell.Border = 0;
            pdfPCell.Colspan = 4;
            pdfTable.AddCell(pdfPCell);
            pdfTable.CompleteRow();

        }


        public void ReportPdfBody()
        {

            fontStyle = FontFactory.GetFont("Times New Roman", 8f, 1);
            pdfPCell = new PdfPCell(new Phrase("Ad Soyad", fontStyle));
            pdfPCell.Colspan = 1;
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            pdfPCell.ExtraParagraphSpace = 0;
            pdfTable.AddCell(pdfPCell);

            fontStyle = FontFactory.GetFont("Times New Roman", 8f, 0);
            pdfPCell = new PdfPCell(new Phrase(ogrenciler.Adi + " " + ogrenciler.Soyadi, fontStyle));
            pdfPCell.Colspan = 1;
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            pdfTable.AddCell(pdfPCell);

            fontStyle = FontFactory.GetFont("Times New Roman", 8f, 1);
            pdfPCell = new PdfPCell(new Phrase("TC Kimlik No", fontStyle));
            pdfPCell.Colspan = 1;
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            pdfPCell.ExtraParagraphSpace = 0;
            pdfTable.AddCell(pdfPCell);

            fontStyle = FontFactory.GetFont("Times New Roman", 8f, 0);
            pdfPCell = new PdfPCell(new Phrase(ogrenciler.TcKimlik_Numara, fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            pdfTable.AddCell(pdfPCell);

            fontStyle = FontFactory.GetFont("Times New Roman", 8f, 1);
            pdfPCell = new PdfPCell(new Phrase("Uyruğu", fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            pdfPCell.ExtraParagraphSpace = 0;
            pdfPCell.Colspan = 1;
            pdfTable.AddCell(pdfPCell);

            fontStyle = FontFactory.GetFont("Times New Roman", 8f, 0);
            pdfPCell = new PdfPCell(new Phrase(ogrenciler.Uyruk, fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            pdfTable.AddCell(pdfPCell);

            fontStyle = FontFactory.GetFont("Times New Roman", 8f, 1);
            pdfPCell = new PdfPCell(new Phrase("Ev Tel/GSM", fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            pdfPCell.ExtraParagraphSpace = 0;
            pdfPCell.Colspan = 1;
            pdfTable.AddCell(pdfPCell);

            fontStyle = FontFactory.GetFont("Times New Roman", 8f, 0);
            pdfPCell = new PdfPCell(new Phrase(ogrenciler.Ev_Numara, fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            pdfTable.AddCell(pdfPCell);

            fontStyle = FontFactory.GetFont("Times New Roman", 8f, 1);
            pdfPCell = new PdfPCell(new Phrase("Cep Telefonu", fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            pdfPCell.ExtraParagraphSpace = 0;
            pdfPCell.Colspan = 1;
            pdfTable.AddCell(pdfPCell);


            fontStyle = FontFactory.GetFont("Times New Roman", 8f, 0);
            pdfPCell = new PdfPCell(new Phrase(ogrenciler.Cep_Numara.ToString(), fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            pdfTable.AddCell(pdfPCell);


            fontStyle = FontFactory.GetFont("Times New Roman", 8f, 1);
            pdfPCell = new PdfPCell(new Phrase("E-posta", fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            pdfTable.AddCell(pdfPCell);



            fontStyle = FontFactory.GetFont("Times New Roman", 8f, 0);
            pdfPCell = new PdfPCell(new Phrase(ogrenciler.Mail, fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            pdfPCell.Colspan = 1;
            pdfTable.AddCell(pdfPCell);

            fontStyle = FontFactory.GetFont("Times New Roman", 8f, 1);
            pdfPCell = new PdfPCell(new Phrase("Adres", fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            pdfPCell.ExtraParagraphSpace = 0;
            pdfPCell.Colspan = 1;
            pdfTable.AddCell(pdfPCell);


            fontStyle = FontFactory.GetFont("Times New Roman", 8f, 0);
            pdfPCell = new PdfPCell(new Phrase(ogrenciler.Adres, fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 1;
            pdfPCell.Colspan = 3;
            pdfTable.AddCell(pdfPCell);

            fontStyle = FontFactory.GetFont("Times New Roman", 9f, 1);
            pdfPCell = new PdfPCell(new Phrase(" ", fontStyle));
            pdfPCell.Colspan = toplamSutun;
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.Border = 0;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            pdfTable.AddCell(pdfPCell);
            pdfTable.CompleteRow();


            fontStyle = FontFactory.GetFont("Times New Roman", 8f, 1);
            pdfPCell = new PdfPCell(new Phrase("Staj Dönem", fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            pdfPCell.ExtraParagraphSpace = 0;
            pdfTable.AddCell(pdfPCell);


            fontStyle = FontFactory.GetFont("Times New Roman", 8f, 0);
            pdfPCell = new PdfPCell(new Phrase(ogrenciler.Soru1, fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            pdfPCell.Colspan = 1;
            pdfTable.AddCell(pdfPCell);

            fontStyle = FontFactory.GetFont("Times New Roman", 8f, 1);
            pdfPCell = new PdfPCell(new Phrase("Is Günü", fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            pdfPCell.ExtraParagraphSpace = 0;
            pdfTable.AddCell(pdfPCell);


            fontStyle = FontFactory.GetFont("Times New Roman", 8f, 0);
            pdfPCell = new PdfPCell(new Phrase(ogrenciler.IsGunu.ToString(), fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            pdfPCell.Colspan = 1;
            pdfTable.AddCell(pdfPCell);

            fontStyle = FontFactory.GetFont("Times New Roman", 8f, 1);
            pdfPCell = new PdfPCell(new Phrase("Başlama Tarihi", fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            pdfPCell.ExtraParagraphSpace = 0;
            pdfTable.AddCell(pdfPCell);


            fontStyle = FontFactory.GetFont("Times New Roman", 8f, 0);
            pdfPCell = new PdfPCell(new Phrase(ogrenciler.stajBaslangic.ToLongDateString(), fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            pdfPCell.Colspan = 1;
            pdfTable.AddCell(pdfPCell);

            fontStyle = FontFactory.GetFont("Times New Roman", 8f, 1);
            pdfPCell = new PdfPCell(new Phrase("Bitiş Tarihi", fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            pdfPCell.ExtraParagraphSpace = 0;
            pdfTable.AddCell(pdfPCell);



            fontStyle = FontFactory.GetFont("Times New Roman", 8f, 0);
            pdfPCell = new PdfPCell(new Phrase(ogrenciler.stajBitis.ToLongDateString(), fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            pdfPCell.Colspan = 1;
            pdfTable.AddCell(pdfPCell);
            pdfTable.CompleteRow();

            fontStyle = FontFactory.GetFont("Times New Roman", 9f, 1);
            pdfPCell = new PdfPCell(new Phrase(" ", fontStyle));
            pdfPCell.Colspan = toplamSutun;
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.Border = 0;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            pdfTable.AddCell(pdfPCell);
            pdfTable.CompleteRow();

            fontStyle = FontFactory.GetFont("Times New Roman", 8f, 0);
            pdfPCell = new PdfPCell(new Phrase("", fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            pdfPCell.Border = 0;
            pdfPCell.Colspan = 4;
            pdfTable.AddCell(pdfPCell);
            pdfTable.CompleteRow();


            fontStyle = FontFactory.GetFont("Times New Roman", 8f, 1);
            pdfPCell = new PdfPCell(new Phrase("Ailemden,  Kendimden veya  Anne-Baba Üzarinden Genel Sağlık Sigortası Kapsamında Sağlık Hizmeti Alıyorum", fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            pdfPCell.ExtraParagraphSpace = 1;
            pdfPCell.Colspan = 3;
            pdfTable.AddCell(pdfPCell);


            fontStyle = FontFactory.GetFont("Times New Roman", 8f, 1);
            pdfPCell = new PdfPCell(new Phrase(ogrenciler.Soru2, fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            pdfPCell.Colspan = 1;
            pdfTable.AddCell(pdfPCell);

            fontStyle = FontFactory.GetFont("Times New Roman", 8f, 1);
            pdfPCell = new PdfPCell(new Phrase("Genel Sağlık Sigortası (GSS) (Gelir Testi Yaptırdım Pirim Ödüyorum)", fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            pdfPCell.ExtraParagraphSpace = 0;
            pdfPCell.Colspan = 3;
            pdfTable.AddCell(pdfPCell);


            fontStyle = FontFactory.GetFont("Times New Roman", 8f, 1);
            pdfPCell = new PdfPCell(new Phrase(ogrenciler.Soru3, fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            pdfPCell.Colspan = 1;
            pdfTable.AddCell(pdfPCell);

            fontStyle = FontFactory.GetFont("Times New Roman", 8f, 1);
            pdfPCell = new PdfPCell(new Phrase("25 Yaşını Doldurdum", fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            pdfPCell.ExtraParagraphSpace = 0;
            pdfPCell.Colspan = 3;
            pdfTable.AddCell(pdfPCell);


            fontStyle = FontFactory.GetFont("Times New Roman", 8f, 1);
            pdfPCell = new PdfPCell(new Phrase(ogrenciler.Soru4, fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            pdfPCell.Colspan = 1;
            pdfTable.AddCell(pdfPCell);

            fontStyle = FontFactory.GetFont("Times New Roman", 6f, 0);
            pdfPCell = new PdfPCell(new Phrase(" ", fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            pdfPCell.Border = 0;
            pdfPCell.Colspan = 4;
            pdfTable.AddCell(pdfPCell);
            pdfTable.CompleteRow();


            fontStyle = FontFactory.GetFont("Times New Roman", 8f, 0);
            pdfPCell = new PdfPCell(new Phrase(" ", fontStyle));
            pdfPCell.Colspan = toplamSutun;
            pdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            pdfPCell.Border = 0;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            pdfPCell.Colspan = 2;
            pdfTable.AddCell(pdfPCell);

            fontStyle = FontFactory.GetFont("Times New Roman", 8f, 0);
            pdfPCell = new PdfPCell(new Phrase("Tarih: .. / .. / ......", fontStyle));
            pdfPCell.Colspan = toplamSutun;
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.Border = 0;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            pdfPCell.Colspan = 2;
            pdfTable.AddCell(pdfPCell);
            pdfTable.CompleteRow();

            fontStyle = FontFactory.GetFont("Times New Roman", 8f, 0);
            pdfPCell = new PdfPCell(new Phrase("", fontStyle));
            pdfPCell.Colspan = toplamSutun;
            pdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            pdfPCell.Border = 0;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            pdfPCell.Colspan = 2;
            pdfTable.AddCell(pdfPCell);

            fontStyle = FontFactory.GetFont("Helvetica", 8f, 0);
            pdfPCell = new PdfPCell(new Phrase("Ad Soyad:          \r\n" +
                                               "Imza:              ", fontStyle));
            pdfPCell.Colspan = toplamSutun;
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.Border = 0;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 2;
            pdfPCell.Colspan = 2;
            pdfTable.AddCell(pdfPCell);
            pdfTable.CompleteRow();

            fontStyle = FontFactory.GetFont("Times New Roman", 6f, 0);
            pdfPCell = new PdfPCell(new Phrase(" ", fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            pdfPCell.Border = 0;
            pdfPCell.Colspan = 4;
            pdfTable.AddCell(pdfPCell);
            pdfTable.CompleteRow();


            fontStyle = FontFactory.GetFont("Times New Roman", 8f, 0);
            pdfPCell = new PdfPCell(new Phrase(" ", fontStyle));
            pdfPCell.Colspan = toplamSutun;
            pdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            pdfPCell.Border = 1;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            pdfPCell.Colspan = 4;
            pdfTable.AddCell(pdfPCell);


            fontStyle = FontFactory.GetFont("Times New Roman", 8f, 1);
            pdfPCell = new PdfPCell(new Phrase("Staj Yapılacak Kurum Bilgileri", fontStyle));
            pdfPCell.Colspan = toplamSutun;
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            pdfPCell.ExtraParagraphSpace = 0;
            pdfPCell.Colspan = 4;
            pdfTable.AddCell(pdfPCell);

            fontStyle = FontFactory.GetFont("Times New Roman", 8f, 1);
            pdfPCell = new PdfPCell(new Phrase("Firma Adı", fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            pdfPCell.ExtraParagraphSpace = 0;
            pdfPCell.Colspan = 1;
            pdfTable.AddCell(pdfPCell);


            fontStyle = FontFactory.GetFont("Times New Roman", 8f, 0);
            pdfPCell = new PdfPCell(new Phrase(ogrenciler.Firma, fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            pdfPCell.Colspan = 3;
            pdfTable.AddCell(pdfPCell);



            fontStyle = FontFactory.GetFont("Times New Roman", 8f, 1);
            pdfPCell = new PdfPCell(new Phrase("E-posta", fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            pdfPCell.ExtraParagraphSpace = 0;
            pdfPCell.Colspan = 1;
            pdfTable.AddCell(pdfPCell);


            fontStyle = FontFactory.GetFont("Times New Roman", 8f, 0);
            pdfPCell = new PdfPCell(new Phrase(ogrenciler.Firma_Mail, fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            pdfPCell.Colspan = 3;
            pdfTable.AddCell(pdfPCell);

            fontStyle = FontFactory.GetFont("Times New Roman", 8f, 1);
            pdfPCell = new PdfPCell(new Phrase("Telefon:", fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            pdfPCell.ExtraParagraphSpace = 0;
            pdfPCell.Colspan = 1;
            pdfTable.AddCell(pdfPCell);


            fontStyle = FontFactory.GetFont("Times New Roman", 8f, 0);
            pdfPCell = new PdfPCell(new Phrase(ogrenciler.Firma_Telefon, fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            pdfPCell.Colspan = 1;
            pdfTable.AddCell(pdfPCell);

            fontStyle = FontFactory.GetFont("Times New Roman", 8f, 1);
            pdfPCell = new PdfPCell(new Phrase("Fax:", fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            pdfPCell.ExtraParagraphSpace = 0;
            pdfPCell.Colspan = 1;
            pdfTable.AddCell(pdfPCell);


            fontStyle = FontFactory.GetFont("Times New Roman", 8f, 0);
            pdfPCell = new PdfPCell(new Phrase(ogrenciler.Faks, fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            pdfPCell.Colspan = 1;
            pdfTable.AddCell(pdfPCell);

            fontStyle = FontFactory.GetFont("Times New Roman", 8f, 1);
            pdfPCell = new PdfPCell(new Phrase("Adres", fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            pdfPCell.ExtraParagraphSpace = 0;
            pdfPCell.Colspan = 1;
            pdfTable.AddCell(pdfPCell);


            fontStyle = FontFactory.GetFont("Times New Roman", 8f, 0);
            pdfPCell = new PdfPCell(new Phrase(ogrenciler.Firma_Adres, fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 1;
            pdfPCell.Colspan = 3;
            pdfTable.AddCell(pdfPCell);

            fontStyle = FontFactory.GetFont("Times New Roman", 6f, 0);
            pdfPCell = new PdfPCell(new Phrase(" ", fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            pdfPCell.Border = 0;
            pdfPCell.Colspan = 4;
            pdfTable.AddCell(pdfPCell);
            pdfTable.CompleteRow();



            fontStyle = FontFactory.GetFont("Times New Roman", 8f, 1);
            pdfPCell = new PdfPCell(new Phrase("Kurum olarak 3308 sayılı kanundaki devlet katkısından yararlanmak istiyor musunuz?", fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            pdfPCell.ExtraParagraphSpace = 0;
            pdfPCell.Colspan = 3;
            pdfTable.AddCell(pdfPCell);


            fontStyle = FontFactory.GetFont("Times New Roman", 8f, 0);
            pdfPCell = new PdfPCell(new Phrase(ogrenciler.Soru4, fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            pdfPCell.Colspan = 1;
            pdfTable.AddCell(pdfPCell);

            fontStyle = FontFactory.GetFont("Times New Roman", 6f, 0);
            pdfPCell = new PdfPCell(new Phrase("Yukarıda adı geçen öğrencinin ilgili tarihlerde staj uygulamasını kurumumuzda yapması uygun görülmüştür.", fontStyle));
            pdfPCell.Colspan = toplamSutun;
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.Border = 0;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            pdfTable.AddCell(pdfPCell);
            pdfTable.CompleteRow();

            fontStyle = FontFactory.GetFont("Times New Roman", 8f, 0);
            pdfPCell = new PdfPCell(new Phrase(" ", fontStyle));
            pdfPCell.Colspan = toplamSutun;
            pdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            pdfPCell.Border = 0;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            pdfPCell.Colspan = 2;
            pdfTable.AddCell(pdfPCell);


            fontStyle = FontFactory.GetFont("Times New Roman", 8f, 0);
            pdfPCell = new PdfPCell(new Phrase("Tarih: .. / .. / ......", fontStyle));
            pdfPCell.Colspan = toplamSutun;
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.Border = 0;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 2;
            pdfPCell.Colspan = 2;
            pdfTable.AddCell(pdfPCell);
            pdfTable.CompleteRow();

            fontStyle = FontFactory.GetFont("Times New Roman", 8f, 0);
            pdfPCell = new PdfPCell(new Phrase(" ", fontStyle));
            pdfPCell.Colspan = toplamSutun;
            pdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            pdfPCell.Border = 0;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            pdfPCell.Colspan = 2;
            pdfTable.AddCell(pdfPCell);

            fontStyle = FontFactory.GetFont("Helvetica", 8f, 0);
            pdfPCell = new PdfPCell(new Phrase("Firma Yetkilisinin Adı Soyadı:          \r\n" +
                                               "Unvanı:                                   \r\n" +
                                               "Imza ve Kaşe                               ", fontStyle));
            pdfPCell.Colspan = toplamSutun;
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.Border = 0;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 2;
            pdfPCell.Colspan = 2;
            pdfTable.AddCell(pdfPCell);
            pdfTable.CompleteRow();

            fontStyle = FontFactory.GetFont("Times New Roman", 8f, 0);
            pdfPCell = new PdfPCell(new Phrase(" ", fontStyle));
            pdfPCell.Colspan = toplamSutun;
            pdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            pdfPCell.Border = 1;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            pdfPCell.Colspan = 4;
            pdfTable.AddCell(pdfPCell);

            fontStyle = FontFactory.GetFont("Times New Roman", 8f, 0);
            pdfPCell = new PdfPCell(new Phrase("", fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            pdfPCell.Border = 0;
            pdfPCell.Colspan = 4;
            pdfTable.AddCell(pdfPCell);
            pdfTable.CompleteRow();

            fontStyle = FontFactory.GetFont("Times New Roman", 6f, 0);
            pdfPCell = new PdfPCell(new Phrase("*3308 sayılı Meslekî Eğitim Kanunu ve 5510 sayılı Sosyal Sigortalar ve Genel Sağlık Sigortası Kanununun 5 inci maddesinin(b) bendi gereğince zorunlu staja tabi tüm öğrencilere \"İş Kazası ve Meslek Hastalığı Sigortası\" yapılması ve sigorta primlerinin Üniversite tarafından ödenmesi gerekmektedir.  Staj süresi boyunca üniversitemiz tarafından öğrencimizin SGK’ya kaydı yaptırılacaktır.\r\n\r\n* *Staja SGK sicil numarası alındıktan sonra başlayacaktır. Farklı firmalarda yapılacak stajlar için ayrı form doldurulacaktır. Öğrenci bu evraktan 2 nüsha düzenleyip firmaca onaylandıktan sonra bir tanesini belirlenen staj döneminden en az 1 ay önce ilgili bölüm başkanlığına teslim etmek zorundadır.\r\n\r\n***Yanıtınız Evet ise Ek - 1 formunu doldurunuz.", fontStyle));
            pdfPCell.Colspan = toplamSutun;
            pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell.Border = 0;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            pdfTable.AddCell(pdfPCell);
            pdfTable.CompleteRow();

            fontStyle = FontFactory.GetFont("Times New Roman", 8f, 0);
            pdfPCell = new PdfPCell(new Phrase(" ", fontStyle));
            pdfPCell.Colspan = toplamSutun;
            pdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            pdfPCell.Border = 1;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            pdfPCell.Colspan = 4;
            pdfTable.AddCell(pdfPCell);

        }

        public void ReportPdfFoot()
        {
            fontStyle = FontFactory.GetFont("Times New Roman", 8f, 1);
            pdfPCell = new PdfPCell(new Phrase("T.C. Kocaeli Üniversitesi Teknoloji Fakültesi\r\nBölüm İş Yeri ve Staj Komisyonu Onayı", fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            pdfPCell.ExtraParagraphSpace = 4;
            pdfPCell.Colspan = 1;
            pdfTable2.AddCell(pdfPCell);


            fontStyle = FontFactory.GetFont("Times New Roman", 8f, 0);
            pdfPCell = new PdfPCell(new Phrase("Yukarıda adı geçen öğrencinin ilgili tarihlerde \r\nstaj uygulamasını ilgili kurumda yapması; \r\n" +
                                                  " UYGUNDUR                UYGUN DEGILDIR", fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 4;
            pdfPCell.Colspan = 1;
            pdfTable2.AddCell(pdfPCell);

            fontStyle = FontFactory.GetFont("Times New Roman", 8f, 1);
            pdfPCell = new PdfPCell(new Phrase("ONAY", fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 4;
            pdfPCell.Colspan = 1;
            pdfTable2.AddCell(pdfPCell);
            pdfTable2.CompleteRow();

            fontStyle = FontFactory.GetFont("Times New Roman", 8f, 1);
            pdfPCell = new PdfPCell(new Phrase("Not:", fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            pdfPCell.Colspan = 3;
            pdfTable2.AddCell(pdfPCell);

        }

    }
}