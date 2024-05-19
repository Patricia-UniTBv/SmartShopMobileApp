using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

public partial class SmartShopDBContext : DbContext
{
    public SmartShopDBContext()
    {
    }

    public SmartShopDBContext(DbContextOptions<SmartShopDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CartItem> CartItems { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<Offer> Offers { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; }

    public virtual DbSet<Supermarket> Supermarkets { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Voucher> Vouchers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=smartshopserver.database.windows.net;Database=SmartShopDB;User ID=sqladmin;Password=SmartShop2024&;Trusted_Connection=False;Encrypt=True;Trust Server Certificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.ToTable("CartItem");

            entity.HasOne(d => d.Product).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.ProductID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CartItem_Product");

            entity.HasOne(d => d.ShoppingCart).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.ShoppingCartID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CartItem_ShoppingCart");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Category");

            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasOne(d => d.ParentCategory).WithMany(p => p.InverseParentCategory)
                .HasForeignKey(d => d.ParentCategoryID)
                .HasConstraintName("FK_Category_Category");
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.ToTable("Location");

            entity.Property(e => e.Address).HasMaxLength(50);

            entity.HasOne(d => d.Supermarket).WithMany(p => p.Locations)
                .HasForeignKey(d => d.SupermarketID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Location_Location");
        });

        modelBuilder.Entity<Offer>(entity =>
        {
            entity.ToTable("Offer");

            entity.Property(e => e.OfferEndDate).HasColumnType("date");
            entity.Property(e => e.OfferStartDate).HasColumnType("date");

            entity.HasOne(d => d.Product).WithMany(p => p.Offers)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Offer_Product");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Product");

            entity.Property(e => e.Barcode).HasMaxLength(50);
            entity.Property(e => e.ImageSource).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Unit).HasMaxLength(20);

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Product_Category");

            entity.HasOne(d => d.Supermarket).WithMany(p => p.Products)
                .HasForeignKey(d => d.SupermarketID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Product_Supermarket");
        });

        modelBuilder.Entity<ShoppingCart>(entity =>
        {
            entity.ToTable("ShoppingCart");

            entity.Property(e => e.CreationDate).HasColumnType("date");
            entity.Property(e => e.IsTransacted).HasDefaultValueSql("((0))");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Supermarket).WithMany(p => p.ShoppingCarts)
                .HasForeignKey(d => d.SupermarketID)
                .HasConstraintName("FK_ShoppingCart_Supermarket");

            entity.HasOne(d => d.User).WithMany(p => p.ShoppingCarts)
                .HasForeignKey(d => d.UserID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ShoppingCart_User");
        });

        modelBuilder.Entity<Supermarket>(entity =>
        {
            entity.ToTable("Supermarket");

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.ToTable("Transaction");

            entity.Property(e => e.Barcode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TransactionDate).HasColumnType("date");
            entity.Property(e => e.VoucherDiscount).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.ShoppingCart).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.ShoppingCartID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Transaction_ShoppingCart");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.Property(e => e.Birthdate).HasColumnType("date");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FirstName).HasMaxLength(20);
            entity.Property(e => e.LastName).HasMaxLength(20);
            entity.Property(e => e.Password)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.PreferredCurrency).HasMaxLength(50);
            entity.Property(e => e.PreferredLanguage)
                .HasMaxLength(2)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Voucher>(entity =>
        {
            entity.ToTable("Voucher");

            entity.Property(e => e.CardNumber).HasMaxLength(50);
            entity.Property(e => e.EarnedPoints).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Supermarket).WithMany(p => p.Vouchers)
                .HasForeignKey(d => d.SupermarketID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Voucher_Supermarket");

            entity.HasOne(d => d.User).WithMany(p => p.Vouchers)
                .HasForeignKey(d => d.UserID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Voucher_User");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
