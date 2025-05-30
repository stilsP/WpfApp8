using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp8.Entities;

namespace WpfApp8
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base("name=diplomchikEntities") { }

        public DbSet<Clients> Clients { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Users> Users { get; set; }

       

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<AppDbContext>(null); // Отключаем миграции
            base.OnModelCreating(modelBuilder);
        }
    }
}
