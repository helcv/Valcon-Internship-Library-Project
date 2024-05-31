using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using ValconLibrary.Entities;

namespace ValconLibrary.Data
{
    [ExcludeFromCodeCoverage]
    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext(DbContextOptions options) : base(options)
        {
            
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<RentBook> Rents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
                .HasMany(s => s.Authors)
                .WithMany(c => c.Books)
                .UsingEntity<Dictionary<string, object>>(
                    "AuthorsBooks",
                    j => j.HasOne<Author>().WithMany().HasForeignKey("AuthorId"),
                    j => j.HasOne<Book>().WithMany().HasForeignKey("BookId"));


            modelBuilder.Entity<RentBook>()
                .HasOne(rb => rb.Book)
                .WithMany(b => b.Rents)
                .HasForeignKey(rb => rb.BookId)
                .IsRequired();

            modelBuilder.Entity<RentBook>()
                .HasOne(rb => rb.User)
                .WithMany(u => u.Rents)
                .HasForeignKey(rb => rb.UserId)
                .IsRequired();
        }
    }
}
