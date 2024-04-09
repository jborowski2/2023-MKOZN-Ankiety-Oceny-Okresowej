using System.Collections.Generic;

namespace OOP.Models
{
    public class Pracownik
    {
        public int PracownikID { get; set; }
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        public string Stopien { get; set; }
        public string Tytul { get; set; }
        public int NumerTelefonu { get; set; }
        public virtual List<Ankieta> Ankieta{ get; set; }
        public virtual ICollection<PracaDyplomowa> PracaDyplomowas { get; set; }
        public virtual ICollection<Osiagniecie> Osiagniecie { get; set; }
        public int? KomisjaID { get; set; }
        public virtual Komisja Komisja { get; set; }
        public int PrzelozonyID { get; set; }
        public virtual Pracownik Przelozony { get; set; }

        public string ApplicationUserID { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}