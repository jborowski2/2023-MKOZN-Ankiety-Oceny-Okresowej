using OOP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OOP.Controllers
{
    /// <summary>
    /// Kontroler odpowiedzialny za zarządzanie rolami użytkowników.
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        /// <summary>
        /// Tworzy domyślne role w systemie.
        /// </summary>
        /// <returns>Informacja o wyniku operacji.</returns>
        /// <remarks>
        /// Role tworzone przez tę metodę to:
        /// - Admin
        /// - Pracownik
        /// - Dziekan
        /// - Rektor
        /// </remarks>
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

        /// <summary>
        /// Dodaje użytkownika do roli.
        /// </summary>
        /// <returns>Informacja o wyniku operacji.</returns>
        /// <remarks>
        /// Aktualnie metoda nie implementuje żadnej logiki dodawania użytkownika do roli.
        /// </remarks>
        public string AddToRole()
        {
            IdentityManager im = new IdentityManager();

            return "OK";
        }
    }
}