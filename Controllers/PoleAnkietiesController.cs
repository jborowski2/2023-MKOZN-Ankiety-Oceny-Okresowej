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

namespace OOP.Controllers
{
    public class PoleAnkietiesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: PoleAnkieties
        public async Task<ActionResult> Index()
        {
            var polaAnkiet = db.PolaAnkiet.Include(p => p.StronaAnkiety);
            return View(await polaAnkiet.ToListAsync());
        }

        // GET: PoleAnkieties/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PoleAnkiety poleAnkiety = await db.PolaAnkiet.FindAsync(id);
            if (poleAnkiety == null)
            {
                return HttpNotFound();
            }
            return View(poleAnkiety);
        }

        // GET: PoleAnkieties/Create
        public ActionResult Create()
        {
            ViewBag.StronaAnkietyID = new SelectList(db.StronyAnkiet, "StronaAnkietyID", "StronaAnkietyID");
            return View();
        }

        // POST: PoleAnkieties/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "PoleAnkietyID,StronaAnkietyID,LiczbaPunktow,Tresc")] PoleAnkiety poleAnkiety)
        {
            if (ModelState.IsValid)
            {
                db.PolaAnkiet.Add(poleAnkiety);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.StronaAnkietyID = new SelectList(db.StronyAnkiet, "StronaAnkietyID", "StronaAnkietyID", poleAnkiety.StronaAnkietyID);
            return View(poleAnkiety);
        }

        // GET: PoleAnkieties/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PoleAnkiety poleAnkiety = await db.PolaAnkiet.FindAsync(id);
            if (poleAnkiety == null)
            {
                return HttpNotFound();
            }
            ViewBag.StronaAnkietyID = new SelectList(db.StronyAnkiet, "StronaAnkietyID", "StronaAnkietyID", poleAnkiety.StronaAnkietyID);
            return View(poleAnkiety);
        }

        // POST: PoleAnkieties/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "PoleAnkietyID,StronaAnkietyID,LiczbaPunktow,Tresc")] PoleAnkiety poleAnkiety)
        {
            if (ModelState.IsValid)
            {
                db.Entry(poleAnkiety).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.StronaAnkietyID = new SelectList(db.StronyAnkiet, "StronaAnkietyID", "StronaAnkietyID", poleAnkiety.StronaAnkietyID);
            return View(poleAnkiety);
        }

        // GET: PoleAnkieties/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PoleAnkiety poleAnkiety = await db.PolaAnkiet.FindAsync(id);
            if (poleAnkiety == null)
            {
                return HttpNotFound();
            }
            return View(poleAnkiety);
        }

        // POST: PoleAnkieties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            PoleAnkiety poleAnkiety = await db.PolaAnkiet.FindAsync(id);
            db.PolaAnkiet.Remove(poleAnkiety);
            await db.SaveChangesAsync();
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
