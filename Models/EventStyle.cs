using System.ComponentModel.DataAnnotations;

namespace Capstone.Api.Models
{
    public class EventStyle
    {
        public int EventId { get; set; }
        public int StyleId { get; set; }

        // Navigation properties
        public virtual Event Event { get; set; }
        public virtual Style Style { get; set; }
    }
} 