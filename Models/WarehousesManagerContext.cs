using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Inventory_manager.Models;

public partial class WarehousesManagerContext : DbContext
{
    public WarehousesManagerContext()
    {
    }

    public WarehousesManagerContext(DbContextOptions<WarehousesManagerContext> options)
        : base(options)
    {
    }

    public virtual DbSet<InventoryIssue> InventoryIssues { get; set; }

    public virtual DbSet<InventoryIssueDetail> InventoryIssueDetails { get; set; }

    public virtual DbSet<InventoryReceipt> InventoryReceipts { get; set; }

    public virtual DbSet<InventoryReceiptDetail> InventoryReceiptDetails { get; set; }

    public virtual DbSet<Material> Materials { get; set; }

    public virtual DbSet<MaterialCategory> MaterialCategories { get; set; }

    public virtual DbSet<Stock> Stocks { get; set; }

    public virtual DbSet<StockLog> StockLogs { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Warehouse> Warehouses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(
                "Server=.;Database=warehouses_manager;User ID=sa;Password=123456;Encrypt=True;TrustServerCertificate=True",
                options => options.EnableRetryOnFailure()
            );
        }
    }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<InventoryIssue>(entity =>
        {
            entity.HasKey(e => e.IssueId).HasName("PK__Inventor__6C861604381A1F1A");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.IssueCode).HasMaxLength(50);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.InventoryIssues)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__Inventory__Creat__5441852A");

            entity.HasOne(d => d.Warehouse).WithMany(p => p.InventoryIssues)
                .HasForeignKey(d => d.WarehouseId)
                .HasConstraintName("FK__Inventory__Wareh__534D60F1");
        });

        modelBuilder.Entity<InventoryIssueDetail>(entity =>
        {
            entity.HasKey(e => e.DetailId).HasName("PK__Inventor__135C316D066C3BAE");

            entity.Property(e => e.Quantity).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalPrice)
                .HasComputedColumnSql("([Quantity]*[UnitPrice])", false)
                .HasColumnType("decimal(37, 4)");
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Issue).WithMany(p => p.InventoryIssueDetails)
                .HasForeignKey(d => d.IssueId)
                .HasConstraintName("FK__Inventory__Issue__5812160E");

            entity.HasOne(d => d.Material).WithMany(p => p.InventoryIssueDetails)
                .HasForeignKey(d => d.MaterialId)
                .HasConstraintName("FK__Inventory__Mater__59063A47");
        });

        modelBuilder.Entity<InventoryReceipt>(entity =>
        {
            entity.HasKey(e => e.ReceiptId).HasName("PK__Inventor__CC08C42020F8F3AD");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.ReceiptCode).HasMaxLength(50);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.InventoryReceipts)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__Inventory__Creat__4BAC3F29");

            entity.HasOne(d => d.Warehouse).WithMany(p => p.InventoryReceipts)
                .HasForeignKey(d => d.WarehouseId)
                .HasConstraintName("FK__Inventory__Wareh__4AB81AF0");
        });

        modelBuilder.Entity<InventoryReceiptDetail>(entity =>
        {
            entity.HasKey(e => e.DetailId).HasName("PK__Inventor__135C316DABE4B128");

            entity.Property(e => e.TotalPrice)
                .HasComputedColumnSql("([Quantity]*[UnitPrice])", false)
                .HasColumnType("decimal(29, 2)");
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Material).WithMany(p => p.InventoryReceiptDetails)
                .HasForeignKey(d => d.MaterialId)
                .HasConstraintName("FK__Inventory__Mater__5070F446");

            entity.HasOne(d => d.Receipt).WithMany(p => p.InventoryReceiptDetails)
                .HasForeignKey(d => d.ReceiptId)
                .HasConstraintName("FK__Inventory__Recei__4F7CD00D");
        });

        modelBuilder.Entity<Material>(entity =>
        {
            entity.HasKey(e => e.MaterialId).HasName("PK__Material__C50610F7C88B4A57");

            entity.HasIndex(e => e.MaterialCode, "UQ__Material__170C54BAD56F60B0").IsUnique();

            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.MaterialCode).HasMaxLength(50);
            entity.Property(e => e.MaterialName).HasMaxLength(150);
            entity.Property(e => e.Unit).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.Category).WithMany(p => p.Materials)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__Materials__Categ__412EB0B6");
        });

        modelBuilder.Entity<MaterialCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Material__19093A0B7A32C16F");

            entity.Property(e => e.CategoryName).HasMaxLength(100);
        });

        modelBuilder.Entity<Stock>(entity =>
        {
            entity.HasKey(e => e.StockId).HasName("PK__Stocks__2C83A9C294F7B36A");

            entity.Property(e => e.LastUpdated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Quantity)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Material).WithMany(p => p.Stocks)
                .HasForeignKey(d => d.MaterialId)
                .HasConstraintName("FK__Stocks__Material__45F365D3");

            entity.HasOne(d => d.Warehouse).WithMany(p => p.Stocks)
                .HasForeignKey(d => d.WarehouseId)
                .HasConstraintName("FK__Stocks__Warehous__44FF419A");
        });

        modelBuilder.Entity<StockLog>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PK__StockLog__5E548648B15F691A");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FinalQuantity).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.QuantityChange).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.StockLogs)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__StockLogs__Creat__5EBF139D");

            entity.HasOne(d => d.Material).WithMany(p => p.StockLogs)
                .HasForeignKey(d => d.MaterialId)
                .HasConstraintName("FK__StockLogs__Mater__5BE2A6F2");

            entity.HasOne(d => d.Warehouse).WithMany(p => p.StockLogs)
                .HasForeignKey(d => d.WarehouseId)
                .HasConstraintName("FK__StockLogs__Wareh__5CD6CB2B");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4C2CFC59E0");

            entity.HasIndex(e => e.Username, "UQ__Users__536C85E4305CDFF2").IsUnique();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.PasswodHash).HasMaxLength(255);
            entity.Property(e => e.Role).HasMaxLength(50);
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        modelBuilder.Entity<Warehouse>(entity =>
        {
            entity.HasKey(e => e.WarehouseId).HasName("PK__Warehous__2608AFF9C914584F");

            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Location).HasMaxLength(200);
            entity.Property(e => e.WarehouseName).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
