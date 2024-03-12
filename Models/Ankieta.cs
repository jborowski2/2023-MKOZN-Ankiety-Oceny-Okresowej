using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OOP.Models
{
    public class Ankieta
    {
        public int AnkietaID { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Data { get; set; }
        public int PracownikID { get; set; }
        public virtual List<StronaAnkiety> StronyAnkiet { get; set; }
        public virtual Pracownik Pracownik { get; set; }

        public int a { get; set; }
        public int CalculateTotalPointsO()
        {
            int sum = 0;
            foreach (var strona in StronyAnkiet)
            {
                foreach (var pole in strona.PolaAnkiety.Where(p => p.Organizacyjne))
                {
                    sum += pole.LiczbaPunktow;
                }
            }
            return sum;
        }
        public int CalculateTotalPointsN()
        {
            int sum = 0;
            foreach (var strona in StronyAnkiet)
            {
                foreach (var pole in strona.PolaAnkiety.Where(p => !p.Organizacyjne))
                {
                    sum += pole.LiczbaPunktow;
                }
            }
            return sum;
        }

    }
}


