using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OOP.Models
{
    public class Attachment
    {
        [Key]
        public int AttachmentID { get; set; }
        [NotMapped]
        public HttpPostedFileBase File { get; set; }
        public string Name { get; set; }
        public string FileType { get; set; }
        public string FilePath { get; set; }
        public int PoleAnkietyID { get; set; }

        public virtual PoleAnkiety PoleAnkiety { get; set; }
    }
}