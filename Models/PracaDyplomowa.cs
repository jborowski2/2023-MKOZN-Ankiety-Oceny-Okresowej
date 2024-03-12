using System;

namespace OOP.Models
{
    public class PracaDyplomowa
    {
        public int PracaDyplomowaID { get; set; }
        public DateTime Data { get; set; }
        public string Temat { get; set; }
        public int PracownikID { get; set; }

        public virtual Pracownik Pracownik { get; set; }
    }
}