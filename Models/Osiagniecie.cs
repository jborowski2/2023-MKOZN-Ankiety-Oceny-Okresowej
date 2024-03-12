using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
namespace OOP.Models
{
    public class Osiagniecie
    {
        public int OsiagniecieID { get; set; }
       [DataType(DataType.Date)]
       [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
       public DateTime Data { get; set; }
        public string Szczegoly { get; set; }
        public int DzialID { get; set; }
        public int PracownikID { get; set; }
        public string Nazwa { get; set; }
        public virtual Dzial Dzial { get; set; }
        public virtual Pracownik Pracownik { get; set; }

    }

}