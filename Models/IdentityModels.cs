using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Drawing;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace OOP.Models
{
    // Możesz dodać dane profilu dla użytkownika, dodając więcej właściwości do klasy ApplicationUser. Odwiedź stronę https://go.microsoft.com/fwlink/?LinkID=317594, aby dowiedzieć się więcej.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Element authenticationType musi pasować do elementu zdefiniowanego w elemencie CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Dodaj tutaj niestandardowe oświadczenia użytkownika
            return userIdentity;
        }
        public virtual Adres Adres { get; set; }

    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        public DbSet<Schemat> Schematy { get; set; }
        public DbSet<Adres> Adresy { get; set; }
        public DbSet<Ankieta> Ankiety { get; set; }
        public DbSet<Dzial> Dzials { get; set; }
        public DbSet<Komisja> Komisje { get; set; }
        public DbSet<Ocena> Ocena { get; set; }
        public DbSet<Osiagniecie> Osiagniecia { get; set; }
        public DbSet<PoleAnkiety> PolaAnkiet { get; set; }
        public DbSet<PracaDyplomowa> PraceDyplomowe { get; set; }
        public DbSet<Pracownik> Pracownicy { get; set; }
        public DbSet<StronaAnkiety> StronyAnkiet { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>()
                .HasRequired(u => u.Adres)
                .WithOptional(a => a.ApplicationUser)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Ankieta>()
                .HasMany(k => k.StronyAnkiet)
                .WithRequired(z => z.Ankieta)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<StronaAnkiety>()
                .HasMany(k => k.PolaAnkiety)
                .WithRequired(z => z.StronaAnkiety)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Pracownik>()
                .HasOptional(u => u.ApplicationUser)
                .WithOptionalDependent()
                .WillCascadeOnDelete(true);
            modelBuilder.Entity<PoleAnkiety>()
                .HasOptional(p => p.Attachment)
                .WithOptionalPrincipal();

            modelBuilder.Entity<Comment>()
                .HasRequired(p=>p.PoleAnkiety)
                .WithOptional(p=>p.Comment);
        }

    }

    //Klasa dodana (napisana)
    public class IdentityManager
    {
        public RoleManager<IdentityRole> LocalRoleManager
        {
            get
            {
                return new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            }
        }


        public UserManager<ApplicationUser> LocalUserManager
        {
            get
            {
                return new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            }
        }


        public ApplicationUser GetUserByID(string userID)
        {
            ApplicationUser user = null;
            UserManager<ApplicationUser> um = this.LocalUserManager;

            user = um.FindById(userID);

            return user;
        }


        public ApplicationUser GetUserByName(string email)
        {
            ApplicationUser user = null;
            UserManager<ApplicationUser> um = this.LocalUserManager;

            user = um.FindByEmail(email);

            return user;
        }


        public bool RoleExists(string name)
        {
            var rm = LocalRoleManager;

            return rm.RoleExists(name);
        }


        public bool CreateRole(string name)
        {
            var rm = LocalRoleManager;
            var idResult = rm.Create(new IdentityRole(name));

            return idResult.Succeeded;
        }


        public bool CreateUser(ApplicationUser user, string password)
        {
            var um = LocalUserManager;
            var idResult = um.Create(user, password);

            return idResult.Succeeded;
        }


        public bool AddUserToRole(string userId, string roleName)
        {
            var um = LocalUserManager;
            var idResult = um.AddToRole(userId, roleName);

            return idResult.Succeeded;
        }


        public bool AddUserToRoleByUsername(string username, string roleName)
        {
            var um = LocalUserManager;

            string userID = um.FindByName(username).Id;
            var idResult = um.AddToRole(userID, roleName);

            return idResult.Succeeded;
        }


        public void ClearUserRoles(string userId)
        {
            var um = LocalUserManager;
            var user = um.FindById(userId);
            var currentRoles = new List<IdentityUserRole>();

            currentRoles.AddRange(user.Roles);

            foreach (var role in currentRoles)
            {
                um.RemoveFromRole(userId, role.RoleId);
            }
        }
    }

}