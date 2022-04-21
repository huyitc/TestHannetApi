using Hannet.Model.Models;
using Microsoft.EntityFrameworkCore;

namespace Hannet.Data
{
    public class HannetDbContext : DbContext
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
        public DbSet<Device> Devices { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Place> Places { get; set; }
        public DbSet<PersonImage> PersonImages { get; set; }
        public DbSet<AppRole> AppRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        { 
        
            }
     }
}
