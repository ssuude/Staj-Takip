namespace Staj1.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("StajBasvuruForm")]
    public partial class StajBasvuruForm
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public StajBasvuruForm()
        {
            //GeriGonderilenBelgeler = new HashSet<GeriGonderilenBelgeler>();
            //Staj = new HashSet<Staj>();
            //StajBasvuruBelgeleri = new HashSet<StajBasvuruBelgeleri>();
            //StajDefteriTeslim = new HashSet<StajDefteriTeslim>();
            //StajyerOgrenciBaslatma = new HashSet<StajyerOgrenciBaslatma>();
        }

        public int FormId { get; set; }

       
        public string Adi { get; set; }

       

        public string Numara { get; set; }

        public string Mail { get; set; }


        public string Soyadi { get; set; }

       
        public string Ev_Numara { get; set; }
        public string Il { get; set; }
        public string Ilce { get; set; }


        public string TcKimlik_Numara { get; set; }

       
        public string Cep_Numara { get; set; }
        public string PostaKodu { get; set; }

        public string Uyruk { get; set; }

        public string Adres { get; set; }
        public string StajDonemi { get; set; }

        

        public int IsGunu { get; set; }
        public DateTime stajBaslangic { get; set; }

        public DateTime stajBitis { get; set; }

        public string Soru1 { get; set; }
        public string Soru2 { get; set; }
        public string Soru3 { get; set; }
        public string Soru4 { get; set; }
        public string Soru5 { get; set; }

        public string Firma { get; set; }

        public string Faaliyet_Alani { get; set; }
        public string Firma_Telefon { get; set; }

        public string Firma_Mail { get; set; }
        public string Faks { get; set; }
        public string Firma_Adres { get; set; }
        public string Firma_Il { get; set; }
        public string Firma_Ilce { get; set; }
        public string Firma_PostaKodu { get; set; }




    }
}