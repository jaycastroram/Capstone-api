namespace Capstone.Api.Models.DTOs
{
    public class InquiryDTO
    {
        public int? Id { get; set; }
        public int PhotographerId { get; set; }
        public int PackageId { get; set; }
        public required string Message { get; set; }
        public string? Response { get; set; }
        public string Status { get; set; }
        public UserDTO? User { get; set; }
        public PhotographerDTO? Photographer { get; set; }
        public PackageDTO? Package { get; set; }
    }
} 