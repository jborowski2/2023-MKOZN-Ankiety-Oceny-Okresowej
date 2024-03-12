using System.Collections.Generic;

namespace OOP.Models
{
    public class Komisja
    {
        public int KomisjaID { get; set; }

        public virtual ICollection<Ocena> Oceny { get; set; }
        public virtual ICollection<Pracownik> Pracowniks { get; set; }
    }
}