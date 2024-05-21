using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OOP.Models
{
    public class Pracownik
    {
     
        public enum PracownikGrupa
        {
            BADAWCZO_DYDAKTYCZNA = 0, // 50% naukowo badawcza i 50% dydaktyczno-organizacyjna
            DYDAKTYCZNA = 1, // 100% dydaktyczno-organizacyjna, 50% można uzupełnić z naukowo-badawczej
            BADAWCZA = 2// 90% z naukowo-badawczej 10% z dydaktyczno organizacyjnej
        }

        public enum PracownikStanowisko
        {
            Asystent = 0, //80
            Wykładowca = 1, //80
            Lektor = 2, // 80
            Instruktor = 3, // 80
            Adiunkt = 4, // 100
            Profesor = 5, // 120
            [Display(Name ="Profesor uczelni")]
            ProfesorUczelni = 6// 120

        }

        public int PracownikID { get; set; }
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        public string Tytul { get; set; }
        public int NumerTelefonu { get; set; }
     
        public PracownikGrupa Grupa { get; set; }
        public PracownikStanowisko Stanowisko { get; set; }
       // public PracownikGrupa group;
        public virtual List<Ankieta> Ankieta { get; set; }
        public virtual ICollection<PracaDyplomowa> PracaDyplomowas { get; set; }
        public virtual ICollection<Osiagniecie> Osiagniecie { get; set; }
        public int? KomisjaID { get; set; }
        public virtual Komisja Komisja { get; set; }
        public int PrzelozonyID { get; set; }
        public virtual Pracownik Przelozony { get; set; }

        public string ApplicationUserID { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

    }

    public class PracownikViewModel
    {
        // inne właściwości

        public string ShowUserRoles(string userId)
        {
            var userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var roles = userManager.GetRoles(userId).FirstOrDefault();
            return roles;
        }
    }
}