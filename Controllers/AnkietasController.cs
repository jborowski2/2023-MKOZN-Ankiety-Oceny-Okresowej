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
using System.Runtime.InteropServices;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.IO;

namespace OOP.Controllers
{
    public class AnkietasController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public async Task<ActionResult> Index(string searchString)
    {
        var user = User.Identity;
        var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        var userId = user.GetUserId();
        var roles = userManager.GetRoles(userId);

        if (roles.Any())
        {
            if (roles.First() == "Admin")
            {
                var ankiety = db.Ankiety.Include(a => a.Pracownik);
                if (!string.IsNullOrEmpty(searchString))
                {
                    ankiety = ankiety.Where(a => (a.Pracownik.Imie + " " + a.Pracownik.Nazwisko).Contains(searchString));
                }
                return View(ankiety);
            }
            else if (roles.First() == "Pracownik")
            {
                try
                {
                    var pracownik = await db.Pracownicy.Where(p => p.ApplicationUserID == userId).FirstAsync();
                    var ankiety = await db.Ankiety.Where(a => a.PracownikID == pracownik.PracownikID).ToListAsync();
                    return View(ankiety);
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Error");
                }
            }
            else if (roles.First() == "Dzial")
            {
                var dzial = await db.Dzials.Where(d => d.ApplicationUserID == userId).FirstAsync();
                return RedirectToAction("DzialAction", dzial);
            }
            else
            {
                return View();
            }
        }
        else
        {
            return View();
        }
    }

    public async Task<ActionResult> DzialAction(Dzial dzial)
        {

            var stronyAnkiet = await db.StronyAnkiet
                    .Include(s => s.Ankieta)
                    .Where(s => s.DzialID == dzial.DzialID)
                    .ToListAsync();
            var viewModels = new List<AnkietaPracownikViewModel>();
            foreach (var strona in stronyAnkiet)
            {
                var viewModel = new AnkietaPracownikViewModel
                {
                    Ankieta = strona.Ankieta,
                    Pracownik = strona.Ankieta.Pracownik
                };
                viewModels.Add(viewModel);
            }
            return View(viewModels);
        }

        public async Task<ActionResult> DzialDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var userId = User.Identity.GetUserId();
            var userDzialId = await db.Dzials
                             .Where(u => u.ApplicationUserID == userId)
                             .Select(u => u.DzialID)
                             .FirstOrDefaultAsync();
            if (userDzialId == 0)
            {
                return HttpNotFound();
            }
            var stronaAnkiety = await db.StronyAnkiet
                               .FirstOrDefaultAsync(s => s.AnkietaID == id.Value && s.DzialID == userDzialId);
            if (stronaAnkiety == null)
            {
                return HttpNotFound();
            }

            return View(stronaAnkiety);
        }


        // GET: Ankietas/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            ViewBag.Dzialy = new SelectList(db.Dzials, "Nazwa");
            ViewBag.Dzialy = new SelectList(db.Dzials, "Nazwa");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ankieta ankieta = await db.Ankiety.FindAsync(id);
            if (ankieta == null)
            {
                return HttpNotFound();
            }
            ViewBag.Message = CalculateTotalPoints(ankieta);
            return View(ankieta);
        }

        // GET: Ankietas/Create
        public ActionResult Create()
        {
            string loggiedid = User.Identity.GetUserId();
            bool isPracownik = User.IsInRole("Pracownik");
            var pracownicy = db.Pracownicy.Select(p => new
            {
                PracownikID = p.PracownikID,
                ApplicationUserID = p.ApplicationUserID,
                ImieNazwisko = p.Imie + " " + p.Nazwisko
            }).ToList();

            if (!isPracownik)
                ViewBag.PracownikID = new SelectList(pracownicy, "PracownikID", "ImieNazwisko");
            else
            {
                var loggedEmployee = pracownicy.SingleOrDefault(p => p.ApplicationUserID == loggiedid);
                pracownicy.Clear();
                pracownicy.Add(loggedEmployee);
                ViewBag.PracownikID = new SelectList(pracownicy, "PracownikID", "ImieNazwisko");

            }
            return View();


        }
        // POST: Ankietas/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "AnkietaID,Data,PracownikID")] Ankieta ankieta)
        {
            if (ModelState.IsValid)
            {

                db.Ankiety.Add(ankieta);
                //addin pages to each dzial
                var dzialy = await db.Dzials.Select(d => d).ToListAsync();
                var schemats = await db.Schematy.Select(s => s).ToListAsync();
                ankieta.AnkietaState = AnkietaState.DO_WYPELNIENIA;
                try
                {
                    foreach (Dzial d in dzialy)
                    {
                        StronaAnkiety s = new StronaAnkiety();
                        s.Dzial = d;
                        s.Ankieta = ankieta;
                        db.StronyAnkiet.Add(s);
                        var polaschemat = schemats.Where(dz => dz.DzialID == d.DzialID).ToList();
                        foreach (var schem in polaschemat)
                        {
                            PoleAnkiety pa = new PoleAnkiety();
                            pa.Tresc = schem.Name;
                            pa.MaksymalnaIloscPunktow = schem.MaxPoints;
                            pa.StronaAnkiety = s;
                            pa.Organizacyjne = schem.IsOrganizational;
                            db.PolaAnkiet.Add(pa);
                        }

                    }

                    await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {

                }
                //6 200ms do optymaizacji
            }

            var pracownicy = db.Pracownicy.Select(p => new
            {
                PracownikID = p.PracownikID,
                ApplicationUserID = p.ApplicationUserID,
                ImieNazwisko = p.Imie + " " + p.Nazwisko
            }).ToList();
            ViewBag.PracownikID = new SelectList(pracownicy, "PracownikID", "ImieNazwisko");
            return View(ankieta);
        }

        // GET: Ankietas/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ankieta ankieta = await db.Ankiety.FindAsync(id);
            if (ankieta == null)
            {
                return HttpNotFound();
            }
            ViewBag.PracownikID = new SelectList(db.Pracownicy, "PracownikID", "Imie", ankieta.PracownikID);
            return View(ankieta);
        }

        // POST: Ankietas/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "AnkietaID,Data,PracownikID")] Ankieta ankieta)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ankieta).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.PracownikID = new SelectList(db.Pracownicy, "PracownikID", "Imie", ankieta.PracownikID);
            return View(ankieta);
        }

        // GET: Ankietas/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ankieta ankieta = await db.Ankiety.FindAsync(id);
            if (ankieta == null)
            {
                return HttpNotFound();
            }
            return View(ankieta);
        }

        // POST: Ankietas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Ankieta ankieta = await db.Ankiety.FindAsync(id);
            db.Ankiety.Remove(ankieta);
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
        [HttpPost]
        public async Task<ActionResult> SaveFields(Ankieta ankieta)
        {
            if (!ModelState.IsValid)
            {
                // return RedirectToAction("Details/");
            }

            string serverFolderPath = Server.MapPath("~/App_Data/Uploads");

            using (var context = db)
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {

                    if (ankieta.StronyAnkiet == null)
                    {
                        throw new NullReferenceException("strony ankiet null");
                    }
                    foreach (var strona in ankieta.StronyAnkiet)
                    {
                        if (strona.PolaAnkiety != null)
                        {
                            foreach (var pole in strona.PolaAnkiety)
                            {
                                if (pole.Attachment != null && pole.Attachment.File != null)
                                {
                                    Attachment attachment = new Attachment();

                                    string filePath = Path.Combine(serverFolderPath, pole.Attachment.File.FileName);
                                    pole.Attachment.File.SaveAs(filePath);

                                    attachment.Name = pole.Attachment.File.FileName;
                                    attachment.FilePath = filePath;
                                    attachment.FileType = pole.Attachment.File.ContentType;
                                    attachment.PoleAnkietyID = pole.PoleAnkietyID;
                                    attachment.PoleAnkiety = pole;
                                    context.Attachments.Add(attachment);
                                    pole.AttachmentID = attachment.AttachmentID;
                                    pole.Attachment = attachment;

                                }
                                else
                                {
                                    pole.Attachment = null;
                                }
                                if (!string.IsNullOrEmpty(pole.Comment.CommentText))
                                {

                                    Comment comment = new Comment();
                                    comment.CommentText = pole.Comment.CommentText;
                                    comment.PoleAnkiety = pole;
                                    comment.PoleAnkietyID = pole.PoleAnkietyID;
                                    context.Comments.Add(comment);
                                    pole.CommentID = comment.CommentID;
                                    pole.Comment = comment;
                  

                                }


                                var entity = context.Entry(pole);
                                entity.State = EntityState.Unchanged;
                                entity.Property(p => p.LiczbaPunktow).IsModified = true;
                            }
                        }
                    }
                    await db.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                   
                    transaction.Rollback();
                }
            }

            return RedirectToAction("Details", new { id = ankieta.AnkietaID });
        }

        [HttpPost]
        public async Task<ActionResult> DeleteAttachment(int attachmentId)
        {
            try
            {
                var attachment = await db.Attachments.FindAsync(attachmentId);
                if (attachment == null)
                {
                    return HttpNotFound();
                }
                db.Attachments.Remove(attachment);
                await db.SaveChangesAsync();

                var serverFolderPath = Server.MapPath("~/App_Data/Uploads");
                var filePath = Path.Combine(serverFolderPath, attachment.FilePath);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                return Json(new { success = true, message = "Załącznik został usunięty." });

            }
            catch (Exception ex)
            {
                // Obsługa wyjątków
                return Json(new { success = false, message = ex.Message });
            }
        }

        private int[] CalculateTotalPoints(Ankieta ankieta)
        {
            int sumaO = 0;
            int sumaN = 0;
            foreach (StronaAnkiety s in ankieta.StronyAnkiet)
            {
                try
                {
                    foreach (PoleAnkiety p in s.PolaAnkiety)
                    {
                        if (p.Organizacyjne)
                        {
                            sumaO += p.LiczbaPunktow;
                        }
                        if (!p.Organizacyjne)
                        {
                            sumaN += p.LiczbaPunktow;
                        }
                    }
                }
                catch (Exception ex)
                {
                    continue;
                }

            }
            int[] tab = new int[2];
            tab[0] = sumaN; tab[1] = sumaO;
            return tab;
        }


    }
}
