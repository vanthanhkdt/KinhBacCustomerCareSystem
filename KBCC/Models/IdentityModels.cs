using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace KBCC.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
        public bool? IsApproved { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập đầy đủ họ tên")]
        [DisplayName("Họ và tên")]
        public string Name { get; set; }
        [Display(Name = "Bộ phận")]
        public string Department { get; set; }
        public string PathProfileImage { get; set; }
        public DateTime? RegistrationTime { get; set; }
        public DateTime? LastLoginTime { get; set; }
        public string LastIPAddress { get; set; }
        public bool? IsBlocked { get; set; }

        // Cơ sở: Hà Nội, Bắc Giang...
        public string Premises { get; set; }
        public bool IsPasswordExpired { get; set; } = false;
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DataAccessObjectModel", throwIfV1Schema: false)
        {
            Database.CreateIfNotExists();
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        public DbSet<RoleAction> RoleActions { get; set; }
        public DbSet<Role> KBCCRoles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<ActionHistories> ActionHistories { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Depot> Depots { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Tag> Tags { get; set; }
    }
    #region User Identities
    public class RoleAction
    {
        [Key]
        public int Id { get; set; }
        public int RoleId { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public string Menu { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedTime { get; set; }
    }

    public class Role
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedTime { get; set; }

    }

    public class UserRole
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int RoleId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedTime { get; set; }
    }
    public class ActionHistories
    {
        [Key]
        public int Id { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string ReturnType { get; set; }
        public string Anonymouse { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Permission { get; set; }
        public string RouteData { get; set; }
        public DateTime DateTime { get; set; }
        public string Remark { get; set; }
    }
    #endregion
}