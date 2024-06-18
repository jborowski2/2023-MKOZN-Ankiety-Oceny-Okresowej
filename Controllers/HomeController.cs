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
          
            string employeeName = GetEmployeeNameByUserId(userId);
            ViewBag.EmployeeName = employeeName;
            return View(await osiagniecia.ToListAsync());
        }

   
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            var userId = User.Identity.GetUserId();

            string employeeName = GetEmployeeNameByUserId(userId);
            ViewBag.EmployeeName = employeeName;
            return View();
        }

        private string GetEmployeeNameByUserId(string userId)
        {
            var employee = db.Pracownicy.FirstOrDefault(e => e.ApplicationUserID== userId);
            return employee != null ? employee.Imie : "Nieznany użytkownik";
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            var userId = User.Identity.GetUserId();
            string employeeName = GetEmployeeNameByUserId(userId);
            ViewBag.EmployeeName = employeeName;
            return View();
        }

        public ActionResult Report()
        {
            ViewBag.Message = "Your application description page.";
            var userId = User.Identity.GetUserId();

            string employeeName = GetEmployeeNameByUserId(userId);
            ViewBag.EmployeeName = employeeName;
            return View();
        }
    }
}