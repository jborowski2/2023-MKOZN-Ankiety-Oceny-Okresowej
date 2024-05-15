using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OOP.Models
{
    public class Comment
    {
        [Key]
        public int CommentID { get; set; }
        public string CommentText { get; set; }
        public int PoleAnkietyID { get; set; }
        public virtual PoleAnkiety PoleAnkiety { get; set;}
  
    }
}