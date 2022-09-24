using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PetStoreApi.Data.Entity;

namespace PetStoreApi.Helpers
{
    public class DataContext : DbContext
    {
        public DataContext()
        {
        }
        public DataContext(DbContextOptions options): base(options)
        {
        }

        #region DbSet
        public DbSet<Breed> Breeds { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Origin> Origins { get; set; }
        public DbSet<ProductOrigin> ProductOrigins { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<AppRole> AppRoles { get; set; }
        public DbSet<AppUserRole> AppUserRoles { get; set; }
        public DbSet<UserInfo> UserInfos { get; set; }
        public DbSet<VerificationToken> VerificationTokens { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Breed>(entity =>
            {
                entity.ToTable("Breed");
                entity.HasKey(e => e.Id);
            });
            modelBuilder.Entity<Origin>(entity =>
            {
                entity.ToTable("Origin");
                entity.HasKey(e => e.Id);
            });
            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");
                entity.HasKey(e => e.Id);
            });
            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");
                entity.HasKey(e => e.Id);

                entity.HasOne(e => e.Breed)
                    .WithMany(e => e.Products)
                    .HasForeignKey(e => e.BreedId)
                    .HasConstraintName("FK_Product_Breed");

                entity.HasOne(e => e.Category)
                    .WithMany(e => e.Products)
                    .HasForeignKey(e => e.CategoryId)
                    .HasConstraintName("FK_Product_Category");
            });
            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Order");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.OrderDate).HasDefaultValueSql("getutcdate()");
                entity.Property(e => e.Reciever).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Address).IsRequired();

                entity.HasOne(o => o.AppUser)
                    .WithMany(u => u.Orders)
                    .HasForeignKey(o => o.UserId)
                    .HasConstraintName("FK_Order_AppUser");
            });
            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.ToTable("OrderItem");
                entity.HasKey(e => new { e.ProductId, e.OrderId });

                entity.HasOne(e => e.Order)
                    .WithMany(e => e.OrderItems)
                    .HasForeignKey(e => e.OrderId)
                    .HasConstraintName("FK_OrderItem_Order");

                entity.HasOne(e => e.Product)
                    .WithMany(e => e.OrderItems)
                    .HasForeignKey(e => e.ProductId)
                    .HasConstraintName("FK_OrderItem_Product");
            });
            modelBuilder.Entity<ProductImage>(entity =>
            {
                entity.ToTable("ProductImage");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ImagePath).IsRequired();
                entity.HasOne(e => e.Product)
                    .WithMany(e => e.ProductImages)
                    .HasForeignKey(e => e.ProductId)
                    .HasConstraintName("FK_ProductImage_Product");
            });
            modelBuilder.Entity<ProductOrigin>(entity =>
            {
                entity.ToTable("ProductOrigin");
                entity.HasKey(e => new { e.ProductId, e.OriginId });

                entity.HasOne(e => e.Origin)
                    .WithMany(e => e.ProductOrigins)
                    .HasForeignKey(e => e.OriginId)
                    .HasConstraintName("FK_ProductOrigin_Origin");

                entity.HasOne(e => e.Product)
                    .WithMany(e => e.ProductOrigins)
                    .HasForeignKey(e => e.ProductId)
                    .HasConstraintName("FK_ProductOrigin_Product");
            });
            modelBuilder.Entity<AppUser>(entity =>
            {
                entity.ToTable("AppUser");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Username).IsUnique();
                entity.Property(e => e.Email).IsRequired().HasMaxLength(150);

                entity.HasOne(e => e.UserInfo)
                    .WithOne(u => u.AppUser)
                    .HasForeignKey<AppUser>(e => e.UserInfoId)
                    .HasConstraintName("FK_AppUser_UserInfo");
            });
            modelBuilder.Entity<AppRole>(entity =>
            {
                entity.ToTable("AppRole");
                entity.HasKey(e => e.Id);
            });
            modelBuilder.Entity<AppUserRole>(entity =>
            {
                entity.ToTable("AppUserRole");
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasOne(e => e.AppUser)
                    .WithMany(e => e.AppUserRoles)
                    .HasForeignKey(e => e.UserId)
                    .HasConstraintName("FK_AppUserRole_AppUser");

                entity.HasOne(e => e.AppRole)
                    .WithMany(e => e.AppUserRoles)
                    .HasForeignKey(e => e.RoleId)
                    .HasConstraintName("FK_AppUserRole_AppRole");
            });
            modelBuilder.Entity<UserInfo>(entity =>
            {
                entity.ToTable("UserInfo");
                entity.HasKey(e => e.Id);
            });
            modelBuilder.Entity<VerificationToken>(entity =>
            {
                entity.ToTable("VerificationToken");
                entity.HasKey(e => e.Id);

                entity.HasOne(v => v.AppUser)
                    .WithMany(u => u.VerificationTokens)
                    .HasForeignKey(v => v.UserId)
                    .HasConstraintName("FK_VerificationToken_AppUser");
            });
            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.ToTable("RefreshToken");
                entity.HasKey(e => e.Id);
                entity.HasOne(r => r.AppUser)
                    .WithMany(u => u.RefreshTokens)
                    .HasForeignKey(r => r.UserId)
                    .HasConstraintName("FK_RefreshToken_AppUser");
            });
        }

    }
}
