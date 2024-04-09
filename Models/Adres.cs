using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OOP.Models
{
    public class Adres
    {
        public int AdresID { get; set; }
        public string Ulica { get; set; }
        public string Numer { get; set; }
        public string KodPocztowy { get; set; }
        public string Miasto { get; set; }
        public string ApplicationUserID { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}