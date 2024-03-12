using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OOP.Models
{
    public class StronaAnkiety
    {
        public int StronaAnkietyID { get; set; }
        public int DzialID { get; set; }
        public int AnkietaID { get; set; }

        public virtual List<PoleAnkiety> PolaAnkiety { get; set; }
        public virtual Ankieta Ankieta { get; set; }
        public virtual Dzial Dzial { get; set; }
        public int CalculateDidacticPoints()
        {
            int sum = 0;
            foreach (var item in PolaAnkiety.Where(p => p.Organizacyjne))
            {
                sum += item.LiczbaPunktow;
            }
            return sum;
        }
        public int CalculateNukalPoints()
        {
            int sum = 0;
            foreach (var item in PolaAnkiety.Where(p => !p.Organizacyjne))
            {
                sum += item.LiczbaPunktow;
            }
            return sum;
        }
    }
}