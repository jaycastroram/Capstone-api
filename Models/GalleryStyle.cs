namespace Capstone.Api.Models
{
    public class GalleryStyle
    {
        public int GalleryId { get; set; }
        public int StyleId { get; set; }

        // Navigation properties
        public virtual Gallery Gallery { get; set; }
        public virtual Style Style { get; set; }
    }
} 