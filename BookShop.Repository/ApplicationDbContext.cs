using BookShop.Domain.DomainModels;
using BookShop.Domain.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Repository

{
    public class ApplicationDbContext : IdentityDbContext<BookShopApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public virtual DbSet<ProductInShoppingCart> ProductInShoppingCarts { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            /*namesto required vo modelite*/
            builder.Entity<Product>()
                .Property(z => z.Id)
                .ValueGeneratedOnAdd();
            builder.Entity<ShoppingCart>()
               .Property(z => z.Id)
               .ValueGeneratedOnAdd();
          //  builder.Entity<ProductInShoppingCart>()
          //    .HasKey(z => new { z.ProductId, z.ShoppingCartId });
            builder.Entity<ProductInShoppingCart>()
                .HasOne(z => z.Product)
                .WithMany(z => z.ProductInShoppingCarts)
                .HasForeignKey(z => z.ShoppingCartId);
            builder.Entity<ProductInShoppingCart>()
                .HasOne(z => z.ShoppingCart)
                .WithMany(z => z.ProductInShoppingCarts)
                .HasForeignKey(z => z.ProductId);
            builder.Entity<ShoppingCart>()
                .HasOne<BookShopApplicationUser>(z => z.Owner)
                .WithOne(z => z.UserCart)
                .HasForeignKey<ShoppingCart>(z => z.OwnerId);

          //  builder.Entity<ProductInOrder>()
          //      .HasKey(z => new { z.ProductId, z.OrderId });
            builder.Entity<ProductInOrder>()
                .HasOne(z => z.SelectedProduct)
                .WithMany(z => z.Orders)
                .HasForeignKey(z => z.ProductId);
            builder.Entity<ProductInOrder>()
               .HasOne(z => z.UserOrder)
               .WithMany(z => z.Products)
               .HasForeignKey(z => z.OrderId);
        }
    }
}
