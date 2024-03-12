using OOP.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

    }

}