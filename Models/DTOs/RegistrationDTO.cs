namespace Capstone.Api.Models.DTOs
{
    public class RegistrationDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? ImageLocation { get; set; }
    }
} 