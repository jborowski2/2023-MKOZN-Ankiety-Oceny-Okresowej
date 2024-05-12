using OOP.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace OOP.Controllers
{
    public class FileController : Controller
    {

        private readonly ApplicationDbContext db = new ApplicationDbContext();
        // GET: File
        public ActionResult Index()
        {
            return View();
        }
        public async Task<ActionResult> Download(int id)
        {

            var filePath =  await GetFilePathById(id);
            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            var fileName = System.IO.Path.GetFileName(filePath);

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        private async Task<string> GetFilePathById(int id)
        {

            var file = await db.Attachments.FindAsync(id);
            string serverFolderPath = Server.MapPath("~/App_Data/Uploads");
            string filePath = Path.Combine(serverFolderPath, file.FilePath);
            return filePath;
        }


        [HttpPost]
        public async Task<ActionResult> Upload(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                if (Path.GetExtension(file.FileName).ToLower() != ".pdf")
                {
                    
                    return View("Index");
                }

                    string originalFileName = Path.GetFileName(file.FileName);
                    string uniqueFileName = originalFileName;

                    string uploadFolderPath = Server.MapPath("~/App_Data/uploads");
                    string filePath = Path.Combine(uploadFolderPath, uniqueFileName);

                    int counter = 1;
                    while (System.IO.File.Exists(filePath))
                    {
                        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(originalFileName);
                        string fileExtension = Path.GetExtension(originalFileName);
                        uniqueFileName = $"{fileNameWithoutExtension} ({counter}){fileExtension}";
                        filePath = Path.Combine(uploadFolderPath, uniqueFileName);
                        counter++;
                    }

                    file.SaveAs(filePath);

               
                    var attachment = new Attachment
                    {
                        Name = uniqueFileName,
                        FilePath = uniqueFileName 
                    };

                    db.Attachments.Add(attachment);
                    await db.SaveChangesAsync();

                    return RedirectToAction("Index");
                }
              
            return View("Index");
        }


        [HttpDelete]
        public ActionResult DeleteAttachment(int id)
        {
            // Znajdź załącznik w bazie danych
            var attachment = db.Attachments.FirstOrDefault(a => a.AttachmentID == id);
            if (attachment != null)
            {
                // Usuń załącznik
                db.Attachments.Remove(attachment);
                db.SaveChanges();
                // Zwróć odpowiedź HTTP 200 OK
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            else
            {
                // Jeśli załącznik nie został znaleziony, zwróć odpowiedź HTTP 404 Not Found
                return HttpNotFound();
            }
        }


    }

}