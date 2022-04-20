using Hannet.Model.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hannet.Data
{
    public class HannetDbContext : DbContext
    {
        public HannetDbContext(DbContextOptions options) : base(options)
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
