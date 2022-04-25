using Hannet.Model.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace Hannet.Data
{
    public class HannetDbContext :IdentityDbContext<AppUser>
    { 
        public HannetDbContext()
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.;Database=HannetDB;Trusted_Connection=True;");
            
        }
        public HannetDbContext(DbContextOptions<HannetDbContext> options) : base(options)
        {
            
        }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AppGroup> AppGroups { get; set; }
        public DbSet<AppRole> AppRoles { get; set; }
        public DbSet<AppRole_Group> AppRole_Groups { get; set; }
        public DbSet<AppUser_Group> AppUser_Groups { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Place> Places { get; set; }
        public DbSet<PersonImage> PersonImages { get; set; }
        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppUser_Group>().ToTable("AppUser_Groups").HasKey(x => new { x.UserId, x.GroupId });
            modelBuilder.Entity<AppRole_Group>().ToTable("AppRole_Groups").HasKey(x => new { x.RoleId, x.GroupId }); 
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("AppUser_Roles").HasKey(x => new { x.UserId, x.RoleId });
            modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("AppUser_Logins").HasKey(i => i.UserId);
            modelBuilder.Entity<IdentityRole>().ToTable("AppRoles");
            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("AppUser_Claims").HasKey(i => i.UserId);
            modelBuilder.Entity<IdentityUserToken<string>>().ToTable("AppUser_Tokens").HasKey(i => i.UserId);
        }
     }
}
