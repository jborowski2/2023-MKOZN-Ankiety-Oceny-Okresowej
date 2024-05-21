using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OOP.Models
{
    public enum KomisjaTypes
    {
        [Display(Name = "Wydziałowa")]
        WYDZIALOWA,
        UCZELNIANA,
        BIBLIOTECZNA,
        ODWOLAWCZA
    }
    public class Komisja
    {
        public int KomisjaID { get; set; }
        public string KomisjaName { get; set; }
        public KomisjaTypes KomisjaType { get; set; }
        public virtual ICollection<Pracownik> Pracowniks { get; set; }
    }
    public class KomisjaViewModel
    {
        [Required]
        public string KomisjaName { get; set; }
        [Required]
        public KomisjaTypes KomisjaType { get; set; }
        [Required(ErrorMessage = "Musisz wybrać przynajmniej jednego pracownika.")]
        public List<int> SelectedPracownicyIds { get; set; }
    }

}