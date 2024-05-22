using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OOP.Models;

namespace OOP.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SchematsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Schemats
        public ActionResult Index()
        {
            var schematy = db.Schematy.Include(s => s.Dzial);
            return View(schematy.ToList());
        }

        // GET: Schemats/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Schemat schemat = db.Schematy.Find(id);
            if (schemat == null)
            {
                return HttpNotFound();
            }
            return View(schemat);
        }

        // GET: Schemats/Create
        public ActionResult Create()
        {
            ViewBag.DzialID = new SelectList(db.Dzials, "DzialID", "Nazwa");
            return View();
        }

        // POST: Schemats/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SchematID,Name,MaxPoints,IsOrganizational,DzialID")] Schemat schemat)
        {
            if (ModelState.IsValid)
            {
                db.Schematy.Add(schemat);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DzialID = new SelectList(db.Dzials, "DzialID", "Nazwa", schemat.DzialID);
            return View(schemat);
        }

        // GET: Schemats/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Schemat schemat = db.Schematy.Find(id);
            if (schemat == null)
            {
                return HttpNotFound();
            }
            ViewBag.DzialID = new SelectList(db.Dzials, "DzialID", "Nazwa", schemat.DzialID);
            return View(schemat);
        }

        // POST: Schemats/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SchematID,Name,MaxPoints,IsOrganizational,DzialID")] Schemat schemat)
        {
            if (ModelState.IsValid)
            {
                db.Entry(schemat).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DzialID = new SelectList(db.Dzials, "DzialID", "Nazwa", schemat.DzialID);
            return View(schemat);
        }

        // GET: Schemats/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Schemat schemat = db.Schematy.Find(id);
            if (schemat == null)
            {
                return HttpNotFound();
            }
            return View(schemat);
        }

        // POST: Schemats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Schemat schemat = db.Schematy.Find(id);
            db.Schematy.Remove(schemat);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Zwolnienie zasobów.
        /// </summary>
        /// <param name="disposing">Wartość true, jeśli zarządzane zasoby powinny być zwolnione; w przeciwnym razie false.</param>
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
