using insurance.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace insurance.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<InsuredViewModel> Insured { get; set; } = null!;
        public DbSet<InsuranceViewModel> Insurance { get; set; } = null!;
        public DbSet<InsuranceEventsViewModel> InsuranceEvents { get; set; } = null!;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options
            )
            : base(options) 
        { 
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            const string ADMIN_ID = "a18be9c0-aa65-4af8-bd17-00bd9344e575";
            base.OnModelCreating(builder);
            builder.Entity<InsuredViewModel>().HasData(
                new InsuredViewModel { Id = 1,
                    Name = "Jakub",
                    Surname = "Penkala",
                    Email = "admin@gmail.com",
                    City = "Ostrava",
                    Street = "Ulice",
                    ZipCode = 70900,
                    PhoneNumber = "725311931"}
                );
            var hasher = new PasswordHasher<IdentityUser>();
            builder.Entity<IdentityUser>().HasData(
                new IdentityUser { Id = ADMIN_ID,
                    UserName = "admin@gmail.com",
                    NormalizedUserName = "ADMIN@GMAIL.COM",
                    Email = "admin@gmail.com",
                    NormalizedEmail = "ADMIN@GMAIL.COM",
                    EmailConfirmed = false,
                    PasswordHash = hasher.HashPassword(null, "Heslo123"),
                    SecurityStamp = string.Empty }
                );       
        }
    }
}