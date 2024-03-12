using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OOP.Models
{
    public class Dzial
    {
        public int DzialID { get; set; }
        public string Nazwa { get; set; }

        public string ApplicationUserID { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual ICollection<Osiagniecie> Osiagniecia { get; set; }
        public virtual ICollection<Ocena> Oceny { get; set; }
        public virtual ICollection<StronaAnkiety> StronaAnkietiey { get; set; }

    }
}