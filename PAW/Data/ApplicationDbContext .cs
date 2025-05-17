using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PAW.Models;

namespace PAW.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Game> Games { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<FavoriteList> FavoriteLists { get; set; }
        public DbSet<FavoriteItem> FavoriteItems { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Photo> Photos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Game>()
                .Property(g => g.Price)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Game>()
                .HasOne(g => g.Genre)
                .WithMany(g => g.Games)
                .HasForeignKey(g => g.GenreId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<CartItem>()
                .Property(ci => ci.Price)
                .HasPrecision(10, 2);

            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.Price)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Orders>()
                .Property(o => o.TotalAmount)
                .HasPrecision(10, 2);

            modelBuilder.Entity<FavoriteItem>()
                .Property(fi => fi.GameTitle)
                .HasMaxLength(255);

            modelBuilder.Entity<Photo>()
                .HasKey(p => p.PhotoId);

            modelBuilder.Entity<Photo>()
                .Property(p => p.ImagePath)
                .HasMaxLength(500);

            modelBuilder.Entity<Game>()
                .HasMany(g => g.Reviews)
                .WithOne(r => r.Game)
                .HasForeignKey(r => r.GameID);

            modelBuilder.Entity<Game>()
                .HasMany(g => g.CartItems)
                .WithOne(ci => ci.Game)
                .HasForeignKey(ci => ci.GameID);

            modelBuilder.Entity<Game>()
                .HasMany(g => g.OrderItems)
                .WithOne(oi => oi.Game)
                .HasForeignKey(oi => oi.GameID);

            modelBuilder.Entity<Game>()
                .HasMany(g => g.FavoriteItems)
                .WithOne(fi => fi.Game)
                .HasForeignKey(fi => fi.GameID);

            modelBuilder.Entity<FavoriteList>()
                .HasMany(fl => fl.FavoriteItems)
                .WithOne(fi => fi.FavoriteList)
                .HasForeignKey(fi => fi.FavoriteListID);

            modelBuilder.Entity<ShoppingCart>()
                .HasMany(sc => sc.CartItems)
                .WithOne(ci => ci.ShoppingCart)
                .HasForeignKey(ci => ci.ShoppingCartID);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                    "Server=(localdb)\\mssqllocaldb;Database=GamersHeavenDB;Trusted_Connection=True;MultipleActiveResultSets=true");
            }
        }
    }
}
