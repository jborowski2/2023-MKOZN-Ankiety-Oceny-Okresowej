﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OOP.Models;
using Microsoft.AspNet.Identity.Owin;
using System.Web.Security;
using Microsoft.AspNet.Identity;

namespace OOP.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PracowniksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Pracowniks
        public async Task<ActionResult> Index(string searchString)
        {
            var userId = User.Identity.GetUserId();
            var pracownicy = db.Pracownicy.Include(p => p.Przelozony);
            string employeeName = GetEmployeeNameByUserId(userId);
            ViewBag.EmployeeName = employeeName;
            if (!string.IsNullOrEmpty(searchString))
            {
                pracownicy = pracownicy.Where(a => (a.Imie + " " + a.Nazwisko).Contains(searchString));
            }

            ViewBag.ShowUserRoles = new Func<string, string>(ShowUserRoles);

            return View(await pracownicy.ToListAsync());
        }

        public string ShowUserRoles(string userId)
        {
            var userManager = Request.GetOwinContext().GetUserManager<ApplicationUserManager>();

            var roles = userManager.GetRoles(userId).First();
            return roles;
        }



        // GET: Pracowniks/Details/5
        public async Task<ActionResult> Details(int? id)
        {
          
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pracownik pracownik = await db.Pracownicy.FindAsync(id);
            
            if (pracownik == null)
            {
                return HttpNotFound();
            }
            return View(pracownik);
        }

        private string GetEmployeeNameByUserId(string userId)
        {
            var employee = db.Pracownicy.FirstOrDefault(e => e.ApplicationUserID == userId);
            return employee != null ? employee.Imie : "Nieznany użytkownik";
        }

        // GET: Pracowniks/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Pracowniks/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "PracownikID,Imie,Nazwisko,Stanowisko,Tytul,NumerTelefonu,PrzelozonyID,ApplicationUserID")] Pracownik pracownik)
        {
            if (ModelState.IsValid)
            {
                db.Pracownicy.Add(pracownik);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(pracownik);
        }

        // GET: Pracowniks/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pracownik pracownik = await db.Pracownicy.FindAsync(id);
            if (pracownik == null)
            {
                return HttpNotFound();
            }
            var roles = db.Roles.ToList();
            ViewBag.Roles = new SelectList(roles, "Id", "Name");
            ViewBag.SupervisorID = new SelectList(db.Pracownicy, "PracownikID", "ImieNazwisko", pracownik.PrzelozonyID);

            return View(pracownik);
        }

        // POST: Pracowniks/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "PracownikID,Imie,Nazwisko,Stanowisko,Tytul,NumerTelefonu,PrzelozonyID,Stanowisko,Status,Grupa,ApplicationUserID")] Pracownik pracownik)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pracownik).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SupervisorID = new SelectList(db.Pracownicy, "PracownikID", "Imie","Nazwisko", pracownik.PrzelozonyID);
            ViewBag.Roles = new SelectList(db.Roles, "Id", "Name");
            return View(pracownik);

        }

        // GET: Pracowniks/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pracownik pracownik = await db.Pracownicy.FindAsync(id);
            if (pracownik == null)
            {
                return HttpNotFound();
            }
            return View(pracownik);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Pracownik pracownik = await db.Pracownicy.FindAsync(id);
            ApplicationUser userToDelete = db.Users.Find(pracownik.ApplicationUserID);
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    db.Users.Remove(userToDelete);
                    db.Pracownicy.Remove(pracownik);

                    await db.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            }

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
