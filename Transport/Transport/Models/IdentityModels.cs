using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Transport.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public int? PersonnelId { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
      
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("TransportEntity", throwIfV1Schema: false)
        {
        }
        static ApplicationDbContext()
        {
            Database.SetInitializer<ApplicationDbContext>(null);
        }
        protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ApplicationUser>().ToTable("IDN.Users").Property(p => p.Id).HasColumnName("Id");
            modelBuilder.Entity<IdentityUserRole>().ToTable("IDN.UserRoles");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("IDN.UserLogins");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("IDN.UserClaims");
            modelBuilder.Entity<IdentityRole>().ToTable("IDN.Roles");

        }
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}