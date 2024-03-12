using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OOP.Models;
using Microsoft.AspNet.Identity;

namespace OOP.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public async Task<ActionResult> Index()
        {
            var userId = User.Identity.GetUserId();
            var pracownik = await db.Pracownicy.Where(p=>p.ApplicationUserID == userId).FirstAsync();
            var osiagniecia = db.Osiagniecia.Include(o => o.Dzial).Where(o => o.PracownikID == pracownik.PracownikID);
            return View(await osiagniecia.ToListAsync());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}