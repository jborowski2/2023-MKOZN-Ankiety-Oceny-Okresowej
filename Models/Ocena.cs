using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OOP.Models
{
    public class Ocena
    {
        public int OcenaID { get; set; }
        public DateTime Data { get; set; }
        public string Stopien { get; set; }
        public int DzialID { get; set; }
        public int KomisjaID { get; set; }
        public int PracownikID { get; set; }
        public int HistoriaOcenID { get; set; }

        public virtual Pracownik Pracownik { get; set; }
        public virtual Komisja Komisja { get; set; }
        public virtual Dzial Dzial { get; set; }
    }
}