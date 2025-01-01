using Book_Store_Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Book_Store_Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser,IdentityRole<int> , int>
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Configure ISBN as unique
            builder.Entity<Book>()
                .HasIndex(b => b.ISBN)
                .IsUnique();
            builder.Entity<ApplicationUser>()
                .HasIndex(b => b.FullName)
                .IsUnique();
            // Configure relationships
        

            builder.Entity<InvoiceItem>()
                .HasOne(ii => ii.Book)
                .WithMany()
                .HasForeignKey(ii => ii.BookId);
            builder.Entity<Invoice>()
      .HasOne(i => i.Order)
      .WithMany()
      .HasForeignKey(i => i.OrderId);

            builder.Entity<Category>()
         .HasMany(c => c.Books) // A Category has many Books
         .WithOne(b => b.Category) // A Book belongs to one Category
         .HasForeignKey(b => b.CategoryId) // Foreign key in the Book table
         .OnDelete(DeleteBehavior.Cascade); // Cascading delete for related books
        }
        public DbSet<Book> Books { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceItem> InvoiceItems { get; set; }

        public DbSet<OrderDetails> OrderDetails { get; set; }

    }
}
