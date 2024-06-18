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
    [Authorize(Roles = "Admin")]
    public class OsiagnieciesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Osiagniecies
        public async Task<ActionResult> Index()
        {
            var userId = User.Identity.GetUserId();
            var osiagniecia = db.Osiagniecia.Include(o => o.Dzial).Include(o => o.Pracownik);
            string employeeName = GetEmployeeNameByUserId(userId);
            ViewBag.EmployeeName = employeeName;
            return View(await osiagniecia.ToListAsync());
        }

        // GET: Osiagniecies/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Osiagniecie osiagniecie = await db.Osiagniecia.FindAsync(id);
            if (osiagniecie == null)
            {
                return HttpNotFound();
            }
            return View(osiagniecie);
        }

        private string GetEmployeeNameByUserId(string userId)
        {
            var employee = db.Pracownicy.FirstOrDefault(e => e.ApplicationUserID == userId);
            return employee != null ? employee.Imie : "Nieznany użytkownik";
        }

        // GET: Osiagniecies/Create
        public ActionResult Create()
        {
            ViewBag.DzialID = new SelectList(db.Dzials, "DzialID", "Nazwa");
            ViewBag.PracownikID = new SelectList(db.Pracownicy, "PracownikID", "Imie");
            return View();
        }

        // POST: Osiagniecies/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "OsiagniecieID,Data,Szczegoly,DzialID,PracownikID,Nazwa")] Osiagniecie osiagniecie)
        {
            if (ModelState.IsValid)
            {
                db.Osiagniecia.Add(osiagniecie);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.DzialID = new SelectList(db.Dzials, "DzialID", "Nazwa", osiagniecie.DzialID);
            ViewBag.PracownikID = new SelectList(db.Pracownicy, "PracownikID", "Imie", osiagniecie.PracownikID);
            return View(osiagniecie);
        }

        // GET: Osiagniecies/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Osiagniecie osiagniecie = await db.Osiagniecia.FindAsync(id);
            if (osiagniecie == null)
            {
                return HttpNotFound();
            }
            ViewBag.DzialID = new SelectList(db.Dzials, "DzialID", "Nazwa", osiagniecie.DzialID);
            ViewBag.PracownikID = new SelectList(db.Pracownicy, "PracownikID", "Imie", osiagniecie.PracownikID);
            return View(osiagniecie);
        }

        // POST: Osiagniecies/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "OsiagniecieID,Data,Szczegoly,DzialID,PracownikID,Nazwa")] Osiagniecie osiagniecie)
        {
            if (ModelState.IsValid)
            {
                db.Entry(osiagniecie).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.DzialID = new SelectList(db.Dzials, "DzialID", "Nazwa", osiagniecie.DzialID);
            ViewBag.PracownikID = new SelectList(db.Pracownicy, "PracownikID", "Imie", osiagniecie.PracownikID);
            return View(osiagniecie);
        }

        // GET: Osiagniecies/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Osiagniecie osiagniecie = await db.Osiagniecia.FindAsync(id);
            if (osiagniecie == null)
            {
                return HttpNotFound();
            }
            return View(osiagniecie);
        }

        // POST: Osiagniecies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Osiagniecie osiagniecie = await db.Osiagniecia.FindAsync(id);
            db.Osiagniecia.Remove(osiagniecie);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
