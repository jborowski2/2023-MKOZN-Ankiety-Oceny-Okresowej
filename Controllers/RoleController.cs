using OOP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OOP.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        // GET: Role
        public string Create()
        {
            IdentityManager im = new IdentityManager();

            im.CreateRole("Admin");
            im.CreateRole("Pracownik");
            im.CreateRole("Dziekan");
            im.CreateRole("Rektor");

            return "OK";
        }


        public string AddToRole()
        {
            IdentityManager im = new IdentityManager();

            return "OK";
        }
    }
}