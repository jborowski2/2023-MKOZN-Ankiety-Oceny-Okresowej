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
    /// <summary>
    /// Kontroler dla zarządzania Komisjami.
    /// </summary>
    public class KomisjasController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        /// <summary>
        /// Akcja wyświetlająca listę wszystkich Komisji.
        /// </summary>
        /// <returns>Widok listy Komisji.</returns>
        // GET: Komisjas
        public async Task<ActionResult> Index()
        {
            return View(await db.Komisje.ToListAsync());
        }
        /// <summary>
        /// Akcja wyświetlająca szczegóły konkretnej Komisji.
        /// </summary>
        /// <param name="id">Identyfikator Komisji.</param>
        /// <returns>Widok szczegółów Komisji.</returns>
        // GET: Komisjas/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Komisja komisja = await db.Komisje.FindAsync(id);
            if (komisja == null)
            {
                return HttpNotFound();
            }
            return View(komisja);
        }
        /// <summary>
        /// Akcja wyświetlająca formularz tworzenia nowej Komisji.
        /// </summary>
        /// <returns>Widok formularza tworzenia Komisji.</returns>
        // GET: Komisjas/Create
        public ActionResult Create()
        {
            ViewBag.Pracownicy = new SelectList(db.Pracownicy, "PracownikID", "Nazwisko");
            return View(new KomisjaViewModel());
        }

        /// <summary>
        /// Akcja obsługująca tworzenie nowej Komisji.
        /// </summary>
        /// <param name="model">Model widoku Komisji.</param>
        /// <returns>Widok indeksu po utworzeniu Komisji.</returns>
        // POST: Komisjas/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(KomisjaViewModel model)
        {
            if (ModelState.IsValid)
            {
                var komisja = new Komisja
                {
                    KomisjaName = model.KomisjaName,
                    KomisjaType = model.KomisjaType,
                    Pracowniks = new List<Pracownik>()
                };
                foreach (int pracownikId in model.SelectedPracownicyIds)
                {
                    var pracownik = await db.Pracownicy.FindAsync(pracownikId);
                    if (pracownik != null)
                    {
                        komisja.Pracowniks.Add(pracownik);
                    }
                }
                db.Komisje.Add(komisja);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Pracownicy = new SelectList(db.Pracownicy, "PracownikID", "Nazwisko");
            return View(model);
        }
        /// <summary>
        /// Akcja wyświetlająca formularz edycji istniejącej Komisji.
        /// </summary>
        /// <param name="id">Identyfikator Komisji.</param>
        /// <returns>Widok formularza edycji Komisji.</returns>
        // GET: Komisjas/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Komisja komisja = await db.Komisje.FindAsync(id);
            if (komisja == null)
            {
                return HttpNotFound();
            }
            return View(komisja);
        }

        /// <summary>
        /// Akcja obsługująca edycję istniejącej Komisji.
        /// </summary>
        /// <param name="komisja">Model Komisji do edycji.</param>
        /// <returns>Widok indeksu po edycji Komisji.</returns>
        // POST: Komisjas/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "KomisjaID,KomisjaName,KomisjaType")] Komisja komisja)
        {
            if (ModelState.IsValid)
            {
                db.Entry(komisja).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(komisja);
        }
        /// <summary>
        /// Akcja wyświetlająca formularz usuwania istniejącej Komisji.
        /// </summary>
        /// <param name="id">Identyfikator Komisji.</param>
        /// <returns>Widok formularza usuwania Komisji.</returns>
        // GET: Komisjas/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Komisja komisja = await db.Komisje.FindAsync(id);
            if (komisja == null)
            {
                return HttpNotFound();
            }
            return View(komisja);
        }
        /// <summary>
        /// Akcja obsługująca usunięcie istniejącej Komisji.
        /// </summary>
        /// <param name="id">Identyfikator Komisji.</param>
        /// <returns>Widok indeksu po usunięciu Komisji.</returns>
        // POST: Komisjas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Komisja komisja = await db.Komisje.FindAsync(id);
            db.Komisje.Remove(komisja);
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
