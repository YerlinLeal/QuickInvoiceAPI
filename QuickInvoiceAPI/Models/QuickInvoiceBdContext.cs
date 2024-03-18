using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace QuickInvoiceAPI.Models;

public partial class QuickInvoiceBdContext : DbContext
{
    public QuickInvoiceBdContext()
    {
    }

    public QuickInvoiceBdContext(DbContextOptions<QuickInvoiceBdContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Sale> Sales { get; set; }

    public virtual DbSet<SaleProduct> SaleProducts { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Code);

            entity.ToTable("PRODUCT");

            entity.Property(e => e.Code)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("CODE");
            entity.Property(e => e.ApplyIva)
                .IsRequired()
                .HasDefaultValueSql("((1))")
                .HasColumnName("APPLY_IVA");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("DESCRIPTION");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("PRICE");
        });

        modelBuilder.Entity<Sale>(entity =>
        {
            entity.ToTable("SALE");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Date)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("DATE");
            entity.Property(e => e.TotalAmount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("TOTAL_AMOUNT");
            entity.Property(e => e.UserId).HasColumnName("USER_ID");

            entity.HasOne(d => d.User).WithMany(p => p.Sales)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_SALE_USER");
        });

        modelBuilder.Entity<SaleProduct>(entity =>
        {
            entity.HasKey(e => new { e.SaleId, e.ProductCode });

            entity.ToTable("SALE_PRODUCT");

            entity.Property(e => e.SaleId).HasColumnName("SALE_ID");
            entity.Property(e => e.ProductCode)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("PRODUCT_CODE");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("DESCRIPTION");
            entity.Property(e => e.ApplyIva)
                .IsRequired()
                .HasDefaultValueSql("((1))")
                .HasColumnName("APPLY_IVA");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("PRICE");
            entity.Property(e => e.Quantity).HasColumnName("QUANTITY");

            entity.HasOne(d => d.Sale).WithMany(p => p.SaleProducts)
                .HasForeignKey(d => d.SaleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SALE_PRODUCT_SALE");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("USER");

            entity.HasIndex(e => e.UserName, "UQ_USER_USER_NAME").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Active)
                .IsRequired()
                .HasDefaultValueSql("((1))")
                .HasColumnName("ACTIVE");
            entity.Property(e => e.Password)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("PASSWORD");
            entity.Property(e => e.UserName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("USER_NAME");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
