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
                UserName = "admin@photography.com",
                NormalizedUserName = "ADMIN@PHOTOGRAPHY.COM",
                Email = "admin@photography.com",
                NormalizedEmail = "ADMIN@PHOTOGRAPHY.COM",
                EmailConfirmed = true,
                Role = "Admin",
                IsVerified = true,
                FirstName = "Admin",
                LastName = "User",
                CreateDateTime = DateTime.UtcNow,
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
                    CreateDateTime = DateTime.UtcNow.AddDays(-30),
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
                    CreateDateTime = DateTime.UtcNow.AddDays(-60),
                    ImageLocation = "https://api.dicebear.com/7.x/avataaars/svg?seed=Jane"
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
                    Name = "Jane Smith",
                    Bio = "Professional photographer with 10 years of experience",
                    PortfolioLink = "https://portfolio.example.com/jane",
                    ContactInfo = "Phone: 555-0123"
                }
            );

            // Seed packages
            builder.Entity<Package>().HasData(
                new Package
                {
                    Id = 1,
                    PhotographerId = 1,
                    Name = "Wedding Package",
                    Description = "Complete wedding photography coverage",
                    Price = 1500.00M,
                    DurationMinutes = 480
                },
                new Package
                {
                    Id = 2,
                    PhotographerId = 1,
                    Name = "Portrait Session",
                    Description = "Professional portrait photography",
                    Price = 250.00M,
                    DurationMinutes = 60
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
        }
    }
}
