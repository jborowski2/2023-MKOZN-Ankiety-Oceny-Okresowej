using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using OOP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.AspNet.Identity.Owin;

namespace OOP.Controllers
{
   
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


        public string AddToRole(string userId, string roleName)
        {
            
            IdentityManager im = new IdentityManager();
            im.ClearUserRoles(userId);
            if (im.AddUserToRole(userId, roleName))
            {
                return "OK";
            }
            else
            {
                return "Failed to add user to role.";
            }
        }

        public string ShowUserRoles(string userId)
        {
            var userManager = Request.GetOwinContext().GetUserManager<ApplicationUserManager>();

            var roles = userManager.GetRoles(userId).First();
            return roles;
        }

        [HttpPost]
        public ActionResult ClearUserRoles(string userId)
        {
            
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                var user = userManager.FindById(userId);

                if (user != null)
                {
                
                    var roles = userManager.GetRoles(userId);
                    foreach (var role in roles)
                    {
                        userManager.RemoveFromRole(userId, role);
                    }

                    return Content("OK");
                }
                else
                {
                    return Content("Użytkownik o podanym identyfikatorze nie został znaleziony.");
                }
            
        
        }

    }
}