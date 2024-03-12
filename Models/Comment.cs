using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OOP.Models
{
    public class Comment
    {
        public int CommentID { get; set; }
        public string CommentText { get; set; }
        public string ApplicationUserID { get; set; }
        public string PoleAnkietyID { get; set; }

        public virtual PoleAnkiety PoleAnkiety { get; set;}
        public virtual ApplicationUser ApplicationUser { get; set; }  
    }
}