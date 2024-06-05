using OOP.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace OOP.Controllers
{
    /// <summary>
    /// Kontroler odpowiedzialny za zarządzanie plikami.
    /// </summary>
    public class FileController : Controller
    {

        private readonly ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Wyświetla stronę główną kontrolera File.
        /// </summary>
        ///  /// <returns>Widok strony głównej kontrolera File.</returns>
        // GET: File
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Pobiera plik na podstawie identyfikatora.
        /// </summary>
        /// <param name="id">Identyfikator pliku.</param>
        /// <returns>Plik do pobrania.</returns>
        public async Task<ActionResult> Download(int id)
        {

            var filePath =  await GetFilePathById(id);
            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            var fileName = System.IO.Path.GetFileName(filePath);

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        /// <summary>
        /// Pobiera ścieżkę pliku na podstawie identyfikatora.
        /// </summary>
        /// <param name="id">Identyfikator pliku.</param>
        /// <returns>Ścieżka pliku.</returns>
        private async Task<string> GetFilePathById(int? id)
        {

            var file = await db.Attachments.FindAsync(id);
            string serverFolderPath = Server.MapPath("~/App_Data/Uploads");
            string filePath = Path.Combine(serverFolderPath, file.FilePath);
            return filePath;
        }

        public async Task<string> AnkietaToPdfConv(Ankieta ankieta, string targetFolderPath)
        {

            Document doc = new Document();
            try
            {
                
                string filePath = Path.Combine(targetFolderPath, ankieta.AnkietaID + ".pdf");
                PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(filePath, FileMode.Create));
                doc.Open();

                doc.Add(new Paragraph($"Ankieta ID: {ankieta.AnkietaID}"));
                doc.Add(new Paragraph($"Stan: {ankieta.AnkietaState}"));
                doc.Add(new Paragraph($"Data: {ankieta.Data:dd-MM-yyyy}"));
                doc.Add(new Paragraph($"Pracownik: {ankieta.Pracownik.Imie} {ankieta.Pracownik.Nazwisko}"));
                doc.Add(new Paragraph($"Punkty Organizacyjne: {ankieta.CalculateTotalPointsO()}"));
                doc.Add(new Paragraph($"Punkty Nieorganizacyjne: {ankieta.CalculateTotalPointsN()}"));
                doc.Add(new Paragraph(""));

                foreach (var strona in ankieta.StronyAnkiet)
                {
                    foreach (var pole in strona.PolaAnkiety)
                    {
                        if(pole.Organizacyjne)
                        doc.Add(new Paragraph($"Nazwa: {pole.Tresc}, Liczba Punktow: {pole.LiczbaPunktow}"));
                    }
                    foreach (var pole in strona.PolaAnkiety)
                    {
                        if (!pole.Organizacyjne)
                            doc.Add(new Paragraph($"Nazwa: {pole.Tresc}, Liczba Punktow: {pole.LiczbaPunktow}"));
                    }
                    doc.Add(new Paragraph(""));
                }
                doc.CloseDocument();
            }
            catch (Exception ex) {
                return ex.ToString();
            }

            return "ok";
        }
    }

}