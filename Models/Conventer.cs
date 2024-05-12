using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace OOP.Models
{
    public static class Conventer
    {
        public static byte[] ConvertToBytes(HttpPostedFileBase file)
        {
            using (var stream = new MemoryStream())
            {
                file.InputStream.CopyTo(stream);
                return stream.ToArray();
            }
        }
    }
}