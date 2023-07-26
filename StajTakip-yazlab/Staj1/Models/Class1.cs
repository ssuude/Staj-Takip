using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Staj1.Models
{
    [Table("OgretmenAtama")]
    public class Atama
    {
        public int ID { get; set; }

        
        public string BelgeAdi { get; set; }

        public DateTime? Tarih { get; set; }

        public int? KullaniciID { get; set; }

        public int? StajDonemNO { get; set; }

        public virtual Kullanici Kullanici { get; set; }
    }
}