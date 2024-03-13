using System.Collections.Generic;

namespace OOP.Models
{
    public enum KomisjaTypes
    {
        WYDZIALOWA,
        UCZELNIANA,
        BIBLIOTECZNA,
        ODWOLAWCZA
    }
    public class Komisja
    {
        public int KomisjaID { get; set; }
        public KomisjaTypes KomisjaType { get; set; }
        public virtual ICollection<Ocena> Oceny { get; set; }
        public virtual ICollection<Pracownik> Pracowniks { get; set; }
    }
}