using EntityFramework.Models;
using Microsoft.EntityFrameworkCore;

namespace EntityFramework.dbServeicer
{
    public class SqlDataAcceess : DbContext
    {

        public SqlDataAcceess(DbContextOptions<SqlDataAcceess> options) : base(options) 
        { 

        }

        public DbSet<Books> Books { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Books>()
                .HasKey(b => b.Id);

            // Other configurations...
        }

    }
}
