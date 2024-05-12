using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OOP.Models
{
    public enum AnkietaState
    {
        DO_WYPELNIENIA,
        WYPELNIENIA,
        DO_POPRAWY,
        SPRAWDZONA_PRZEZ_PRZELOŻONEGO,
        SPRAWDZONA_PRZEZ_DZIAL,
        W_KOMISJI,
        U_DZIEKANA
    }
    public class Ankieta
    {
        public int AnkietaID { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public AnkietaState AnkietaState { get; set; }
        public DateTime Data { get; set; }
        public int PracownikID { get; set; }
        public virtual List<StronaAnkiety> StronyAnkiet { get; set; }
        public virtual Pracownik Pracownik { get; set; }
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