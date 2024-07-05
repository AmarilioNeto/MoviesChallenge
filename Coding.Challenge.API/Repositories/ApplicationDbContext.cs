using Coding.Challenge.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Coding.Challenge.API.Repositories
{


    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
        }
        public DbSet<ContentInput> Contents { get; set; }
        public DbSet<Genrer> Genrers { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ContentInput>()
                .HasMany(c => c.Genrers)
                .WithOne(g => g.ContentInput)
                .HasForeignKey(g => g.ContentInputId);

            base.OnModelCreating(modelBuilder);
        }

    }

}


