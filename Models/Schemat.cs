using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OOP.Models
{
    public class Schemat
    {
        public int SchematID { get; set; }
        public string Name { get; set; }
        public string MaxPoints { get; set; }
        public bool IsOrganizational { get; set; }
        public int DzialID { get; set; }
        public virtual Dzial Dzial { get; set; }
    }
}