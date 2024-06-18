using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Reflection;
using System.ComponentModel;

namespace OOP.Models
{
    public enum AnkietaState
    {
        [Description("Do wypełnienia")]
        DO_WYPELNIENIA,
        [Description("Wypełnienia")]
        WYPELNIANA,
        [Description("Do poprawy")]
        DO_POPRAWY,
        [Description("Sprawdzona przez przełożonego")]
        SPRAWDZONA_PRZEZ_PRZELOŻONEGO,
        [Description("Sprawdzona przez dział")]
        SPRAWDZONA_PRZEZ_DZIAL,
        [Description("W komisji")]
        W_KOMISJI,
        [Description("U dziekana")]
        U_DZIEKANA,
        [Description("Zatwierdzona")]
        ZATWIERDZONA
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