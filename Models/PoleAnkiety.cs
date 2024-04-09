namespace OOP.Models
{
    public class PoleAnkiety
    {
        public int PoleAnkietyID { get; set; }
        public int StronaAnkietyID { get; set; }
        public int LiczbaPunktow { get; set; }
        public string Tresc { get; set; }
        public bool Organizacyjne { get; set; }
        public string MaksymalnaIloscPunktow { get; set; }
        public int? AttachmentID { get; set; }

        public virtual StronaAnkiety StronaAnkiety { get; set; }
        public virtual Attachment Attachment { get; set; }
        public virtual Comment Comment { get; set; }
        public virtual Comment DzialComment { get; set; }
        public virtual Comment PrzelozonyComment { get; set; }
    }
}