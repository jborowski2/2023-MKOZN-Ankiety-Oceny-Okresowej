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
    [Authorize(Roles = "Admin")]
    public class PracowniksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Pracowniks
        public async Task<ActionResult> Index(String searchString)
        {
            
            if (!string.IsNullOrEmpty(searchString))
            {

                var pracownicy = db.Pracownicy.Where(a => (a.Imie + " " + a.Nazwisko).Contains(searchString));
                return View(await  pracownicy.ToListAsync());
            }
            
              

            return View(await db.Pracownicy.ToListAsync());
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
        public async Task<ActionResult> Create([Bind(Include = "PracownikID,Imie,Nazwisko,Stopien,Tytul,NumerTelefonu,PrzelozonyID,ApplicationUserID")] Pracownik pracownik)
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
            return View(pracownik);
        }

        // POST: Pracowniks/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "PracownikID,Imie,Nazwisko,Stopien,Tytul,NumerTelefonu,PrzelozonyID,ApplicationUserID")] Pracownik pracownik)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pracownik).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
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
