using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Capstone.Api.Models;

namespace Capstone.Api.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        private readonly IConfiguration _configuration;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }

        public DbSet<Photographer> Photographers { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Style> Styles { get; set; }
        public DbSet<PhotographerStyle> PhotographerStyles { get; set; }
        public DbSet<EventStyle> EventStyles { get; set; }
        public DbSet<Gallery> Galleries { get; set; }
        public DbSet<GalleryImage> GalleryImages { get; set; }
        public DbSet<GalleryStyle> GalleryStyles { get; set; }
        public DbSet<Inquiry> Inquiries { get; set; }
        public DbSet<ClientFollowup> ClientFollowups { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<GalleryComment> GalleryComments { get; set; }
        public DbSet<ImageComment> ImageComments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure many-to-many relationships
            builder.Entity<PhotographerStyle>()
                .HasKey(ps => new { ps.PhotographerId, ps.StyleId });

            builder.Entity<EventStyle>()
                .HasKey(es => new { es.EventId, es.StyleId });

            builder.Entity<GalleryStyle>()
                .HasKey(gs => new { gs.GalleryId, gs.StyleId });

            // Configure User relationships
            builder.Entity<User>()
                .HasOne(u => u.Photographer)
                .WithOne(p => p.User)
                .HasForeignKey<Photographer>(p => p.UserId);

            // Configure Photographer relationships
            builder.Entity<Photographer>()
                .HasMany(p => p.Events)
                .WithOne(e => e.Photographer)
                .HasForeignKey(e => e.PhotographerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Photographer>()
                .HasMany(p => p.Packages)
                .WithOne(p => p.Photographer)
                .HasForeignKey(p => p.PhotographerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure Booking relationships
            builder.Entity<Booking>()
                .HasOne(b => b.User)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Booking>()
                .HasOne(b => b.Photographer)
                .WithMany(p => p.Bookings)
                .HasForeignKey(b => b.PhotographerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure Gallery relationships
            builder.Entity<Gallery>()
                .HasMany(g => g.Images)
                .WithOne(i => i.Gallery)
                .HasForeignKey(i => i.GalleryId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<GalleryImage>()
                .HasMany(gi => gi.Comments)
                .WithOne(c => c.GalleryImage)
                .HasForeignKey(c => c.GalleryImageId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure Payment relationships
            builder.Entity<Payment>()
                .HasOne(p => p.Booking)
                .WithMany(b => b.Payments)
                .HasForeignKey(p => p.BookingId)
                .OnDelete(DeleteBehavior.Restrict);

            // Seed Roles
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole 
                { 
                    Id = "admin-role-id", 
                    Name = "Admin", 
                    NormalizedName = "ADMIN" 
                },
                new IdentityRole 
                { 
                    Id = "photographer-role-id", 
                    Name = "Photographer", 
                    NormalizedName = "PHOTOGRAPHER" 
                },
                new IdentityRole 
                { 
                    Id = "user-role-id", 
                    Name = "User", 
                    NormalizedName = "USER" 
                }
            );

            // Create admin user
            var hasher = new PasswordHasher<User>();
            var adminUser = new User
            {
                Id = "admin-id",
                UserName = "admin@photo.com",
                NormalizedUserName = "ADMIN@PHOTOGRAPHY.COM",
                Email = "admin@photo.com",
                NormalizedEmail = "ADMIN@PHOTOGRAPHY.COM",
                EmailConfirmed = true,
                Role = "Admin",
                IsVerified = true,
                FirstName = "Admin",
                LastName = "User",
                CreateDateTime = DateTime.SpecifyKind(new DateTime(2024, 1, 1), DateTimeKind.Utc),
                ImageLocation = "https://api.dicebear.com/7.x/avataaars/svg?seed=Admin"
            };
            adminUser.PasswordHash = hasher.HashPassword(adminUser, _configuration["AdminPassword"]);
            builder.Entity<User>().HasData(adminUser);

            // Seed some regular users
            var users = new List<User>
            {
                new User
                {
                    Id = "user-id-1",
                    UserName = "john.doe@example.com",
                    NormalizedUserName = "JOHN.DOE@EXAMPLE.COM",
                    Email = "john.doe@example.com",
                    NormalizedEmail = "JOHN.DOE@EXAMPLE.COM",
                    EmailConfirmed = true,
                    FirstName = "John",
                    LastName = "Doe",
                    Role = "User",
                    IsVerified = true,
                    CreateDateTime = DateTime.SpecifyKind(new DateTime(2024, 2, 1), DateTimeKind.Utc),
                    ImageLocation = "https://api.dicebear.com/7.x/avataaars/svg?seed=John"
                },
                new User
                {
                    Id = "photographer-id-1",
                    UserName = "jane.smith@example.com",
                    NormalizedUserName = "JANE.SMITH@EXAMPLE.COM",
                    Email = "jane.smith@example.com",
                    NormalizedEmail = "JANE.SMITH@EXAMPLE.COM",
                    EmailConfirmed = true,
                    FirstName = "Jane",
                    LastName = "Smith",
                    Role = "Photographer",
                    IsVerified = true,
                    CreateDateTime = DateTime.SpecifyKind(new DateTime(2024, 1, 1), DateTimeKind.Utc),
                    ImageLocation = "https://api.dicebear.com/7.x/avataaars/svg?seed=Jane"
                },
                new User
                {
                    Id = "photographer-id-2",
                    UserName = "sarah.wilson@example.com",
                    NormalizedUserName = "SARAH.WILSON@EXAMPLE.COM",
                    Email = "sarah.wilson@example.com",
                    NormalizedEmail = "SARAH.WILSON@EXAMPLE.COM",
                    EmailConfirmed = true,
                    FirstName = "Sarah",
                    LastName = "Wilson",
                    Role = "Photographer",
                    IsVerified = true,
                    CreateDateTime = DateTime.SpecifyKind(new DateTime(2024, 1, 15), DateTimeKind.Utc),
                    ImageLocation = "https://api.dicebear.com/7.x/avataaars/svg?seed=Sarah"
                },
                new User
                {
                    Id = "photographer-id-3",
                    UserName = "mike.johnson@example.com",
                    NormalizedUserName = "MIKE.JOHNSON@EXAMPLE.COM",
                    Email = "mike.johnson@example.com",
                    NormalizedEmail = "MIKE.JOHNSON@EXAMPLE.COM",
                    EmailConfirmed = true,
                    FirstName = "Mike",
                    LastName = "Johnson",
                    Role = "Photographer",
                    IsVerified = true,
                    CreateDateTime = DateTime.SpecifyKind(new DateTime(2024, 2, 1), DateTimeKind.Utc),
                    ImageLocation = "https://api.dicebear.com/7.x/avataaars/svg?seed=Mike"
                },
                new User
                {
                    Id = "user-id-2",
                    UserName = "emma.davis@example.com",
                    NormalizedUserName = "EMMA.DAVIS@EXAMPLE.COM",
                    Email = "emma.davis@example.com",
                    NormalizedEmail = "EMMA.DAVIS@EXAMPLE.COM",
                    EmailConfirmed = true,
                    FirstName = "Emma",
                    LastName = "Davis",
                    Role = "User",
                    IsVerified = true,
                    CreateDateTime = DateTime.SpecifyKind(new DateTime(2024, 2, 15), DateTimeKind.Utc),
                    ImageLocation = "https://api.dicebear.com/7.x/avataaars/svg?seed=Emma"
                }
            };

            foreach (var user in users)
            {
                user.PasswordHash = hasher.HashPassword(user, _configuration["DefaultPassword"]);
                builder.Entity<User>().HasData(user);
            }

            // Seed photographers
            builder.Entity<Photographer>().HasData(
                new Photographer
                {
                    Id = 1,
                    UserId = "photographer-id-1",
                    Name = "John Doe",
                    Bio = "Professional photographer with 10 years experience",
                    PortfolioLink = "https://portfolio.com/johndoe"
                },
                new Photographer
                {
                    Id = 2,
                    UserId = "photographer-id-2",
                    Name = "Sarah Wilson",
                    Bio = "Specializing in wedding and portrait photography",
                    PortfolioLink = "https://portfolio.com/sarahwilson"
                },
                new Photographer
                {
                    Id = 3,
                    UserId = "photographer-id-3",
                    Name = "Mike Johnson",
                    Bio = "Nature and landscape photographer",
                    PortfolioLink = "https://portfolio.com/mikejohnson"
                }
            );

            // Seed packages
            builder.Entity<Package>().HasData(
                new Package
                {
                    Id = 1,
                    PhotographerId = 1,
                    Name = "Wedding Premium",
                    Description = "Full day wedding coverage with 2 photographers",
                    Price = 2500M
                },
                new Package
                {
                    Id = 2,
                    PhotographerId = 1,
                    Name = "Portrait Session",
                    Description = "2-hour portrait session with 20 edited photos",
                    Price = 300M
                },
                new Package
                {
                    Id = 3,
                    PhotographerId = 2,
                    Name = "Event Coverage",
                    Description = "4-hour event coverage",
                    Price = 800M
                },
                new Package
                {
                    Id = 4,
                    PhotographerId = 3,
                    Name = "Family Portrait",
                    Description = "Outdoor family portrait session",
                    Price = 400M
                },
                new Package
                {
                    Id = 5,
                    PhotographerId = 2,
                    Name = "Mini Session",
                    Description = "30-minute portrait session",
                    Price = 150M
                }
            );

            // Seed styles
            builder.Entity<Style>().HasData(
                new Style { Id = 1, Name = "Portrait" },
                new Style { Id = 2, Name = "Landscape" },
                new Style { Id = 3, Name = "Wedding" },
                new Style { Id = 4, Name = "Street" }
            );

            // Keep the role assignment
            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = "admin-role-id",
                    UserId = "admin-id"
                }
            );

            // Seed inquiries
            builder.Entity<Inquiry>().HasData(
                new Inquiry
                {
                    Id = 1,
                    UserId = "user-id-1",
                    PhotographerId = 1,
                    PackageId = 1,
                    Message = "Interested in wedding photography",
                    Status = "open",
                    CreatedAt = DateTime.SpecifyKind(new DateTime(2024, 3, 1), DateTimeKind.Utc)
                },
                new Inquiry
                {
                    Id = 2,
                    UserId = "user-id-1",
                    PhotographerId = 1,
                    PackageId = 1,
                    Message = "Looking for engagement photo session",
                    Status = "open",
                    CreatedAt = DateTime.SpecifyKind(new DateTime(2024, 3, 1), DateTimeKind.Utc)
                },
                new Inquiry
                {
                    Id = 3,
                    UserId = "user-id-2",
                    PhotographerId = 2,
                    PackageId = 3,
                    Message = "Need photographer for corporate event",
                    Status = "pending",
                    CreatedAt = DateTime.SpecifyKind(new DateTime(2024, 3, 1), DateTimeKind.Utc)
                },
                new Inquiry
                {
                    Id = 4,
                    UserId = "user-id-1",
                    PhotographerId = 3,
                    PackageId = 4,
                    Message = "Interested in family portrait session",
                    Status = "confirmed",
                    Response = "Great! Let's schedule for next week",
                    CreatedAt = DateTime.SpecifyKind(new DateTime(2024, 3, 1), DateTimeKind.Utc)
                }
            );

            // Seed bookings
            builder.Entity<Booking>().HasData(
                new Booking
                {
                    Id = 1,
                    UserId = "user-id-1",
                    PackageId = 1,
                    PhotographerId = 1,
                    BookingDate = DateTime.SpecifyKind(new DateTime(2024, 3, 15), DateTimeKind.Utc),
                    Time = new TimeSpan(14, 0, 0),
                    Status = "confirmed",
                    TotalAmount = 500.00M,
                    Notes = "Outdoor session"
                },
                new Booking
                {
                    Id = 2,
                    UserId = "user-id-2",
                    PackageId = 3,
                    PhotographerId = 2,
                    BookingDate = DateTime.SpecifyKind(new DateTime(2024, 4, 1), DateTimeKind.Utc),
                    Time = new TimeSpan(10, 0, 0),
                    Status = "pending",
                    TotalAmount = 750.00M,
                    Notes = "Indoor studio session"
                },
                new Booking
                {
                    Id = 3,
                    UserId = "user-id-1",
                    PackageId = 2,
                    BookingDate = DateTime.SpecifyKind(new DateTime(2024, 2, 28), DateTimeKind.Utc),
                    Status = "completed",
                    TotalAmount = 300M,
                    Notes = "Portrait session completed"
                }
            );
        }
    }
}
