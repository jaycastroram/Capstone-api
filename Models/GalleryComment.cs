using System.ComponentModel.DataAnnotations;

namespace Capstone.Api.Models
{
    public class GalleryComment
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }
        public int GalleryId { get; set; }

        [Required]
        public string Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual User User { get; set; }
        public virtual Gallery Gallery { get; set; }
    }
} 