using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace RestaurantBooking.Models
{
    public partial class RestaurantBookingContext : DbContext
    {
        public RestaurantBookingContext()
        {
        }

        public RestaurantBookingContext(DbContextOptions<RestaurantBookingContext> options)
            : base(options)
        {
        }

        public virtual DbSet<MenuItem> MenuItems { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<Payment> Payments { get; set; } = null!;
        public virtual DbSet<Table> Tables { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfiguration configuration = builder.Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("MyCnn"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MenuItem>(entity =>
            {
                entity.Property(e => e.MenuItemId)
                    .ValueGeneratedNever()
                    .HasColumnName("MenuItemID");

                entity.Property(e => e.ItemName).HasMaxLength(100);

                entity.Property(e => e.ItemPrice).HasColumnType("decimal(10, 2)");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.OrderId)
                    .ValueGeneratedNever()
                    .HasColumnName("OrderID");

                entity.Property(e => e.OrderTime).HasColumnType("datetime");

                entity.Property(e => e.PaymentStatus).HasMaxLength(50);

                entity.Property(e => e.Status).HasMaxLength(50);

                entity.Property(e => e.TableId).HasColumnName("TableID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Table)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.TableId)
                    .HasConstraintName("FK__Orders__TableID__2B3F6F97");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Orders__UserID__2C3393D0");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.Property(e => e.PaymentId)
                    .ValueGeneratedNever()
                    .HasColumnName("PaymentID");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.PaymentAmount).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.PaymentDate).HasColumnType("datetime");

                entity.Property(e => e.PaymentStatus).HasMaxLength(50);

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK__Payments__OrderI__2F10007B");
            });

            modelBuilder.Entity<Table>(entity =>
            {
                entity.Property(e => e.TableId)
                    .ValueGeneratedNever()
                    .HasColumnName("TableID");

                entity.Property(e => e.TableName).HasMaxLength(50);

                entity.Property(e => e.TableStatus).HasMaxLength(50);

                entity.Property(e => e.TableType).HasMaxLength(50);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.UserId)
                    .ValueGeneratedNever()
                    .HasColumnName("UserID");

                entity.Property(e => e.FullName).HasMaxLength(100);

                entity.Property(e => e.Password).HasMaxLength(50);

                entity.Property(e => e.UserRole).HasMaxLength(50);

                entity.Property(e => e.Username).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
