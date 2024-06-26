﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WareHouseManagement.Repository.Models
{
    public partial class OrderManagementContext : DbContext
    {
        public OrderManagementContext()
        {
        }

        public OrderManagementContext(DbContextOptions<OrderManagementContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; } = null!;
        public virtual DbSet<Admin> Admins { get; set; } = null!;
        public virtual DbSet<Batch> Batches { get; set; } = null!;
        public virtual DbSet<BatchOrder> BatchOrders { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<RefreshTokenAccount> RefreshTokenAccounts { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Shipper> Shippers { get; set; } = null!;
        public virtual DbSet<StaticFile> StaticFiles { get; set; } = null!;
        public virtual DbSet<Warehouse> Warehouses { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=LAPTOP-Q339A538\\SQLEXPRESS;User ID=sa;Password=123456;Database=OrderManagement;Trusted_Connection=False;TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("Account");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .HasColumnName("email");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .HasColumnName("password");

                entity.Property(e => e.RoleId).HasColumnName("role_id");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK__Account__role_id__47DBAE45");
            });

            modelBuilder.Entity<Admin>(entity =>
            {
                entity.ToTable("Admin");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.AccountId).HasColumnName("account_id");

                entity.Property(e => e.BatchMode)
                    .HasMaxLength(255)
                    .HasColumnName("batch_mode");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Admins)
                    .HasForeignKey(d => d.AccountId)
                    .HasConstraintName("FK__Admin__account_i__49C3F6B7");
            });

            modelBuilder.Entity<Batch>(entity =>
            {
                entity.ToTable("Batch");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.BatchMode)
                    .HasMaxLength(50)
                    .HasColumnName("batch_mode");

                entity.Property(e => e.DateModifiedBatchMode).HasColumnType("date");

                entity.Property(e => e.Img).HasColumnName("img");

                entity.Property(e => e.ShipperId).HasColumnName("shipper_id");

                entity.Property(e => e.WarehouseId).HasColumnName("warehouse_id");

                entity.HasOne(d => d.Shipper)
                    .WithMany(p => p.Batches)
                    .HasForeignKey(d => d.ShipperId)
                    .HasConstraintName("FK__Batch__shipper_i__4BAC3F29");

                entity.HasOne(d => d.Warehouse)
                    .WithMany(p => p.Batches)
                    .HasForeignKey(d => d.WarehouseId)
                    .HasConstraintName("FK__Batch__warehouse__4D94879B");
            });

            modelBuilder.Entity<BatchOrder>(entity =>
            {
                entity.ToTable("Batch_Order");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.BatchId).HasColumnName("batch_id");

                entity.Property(e => e.OrderId).HasColumnName("order_id");

                entity.HasOne(d => d.Batch)
                    .WithMany(p => p.BatchOrders)
                    .HasForeignKey(d => d.BatchId)
                    .HasConstraintName("FK__Batch_Ord__batch__4F7CD00D");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.BatchOrders)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK__Batch_Ord__order__5165187F");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Order");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Address).HasMaxLength(255);

                entity.Property(e => e.DeliveryDate)
                    .HasColumnType("date")
                    .HasColumnName("delivery_date");

                entity.Property(e => e.ExpectedDateOfDelivery)
                    .HasColumnType("date")
                    .HasColumnName("expected_date_of_delivery");

                entity.Property(e => e.Img).HasColumnName("img");

                entity.Property(e => e.OrderDate)
                    .HasColumnType("datetime")
                    .HasColumnName("order_date");

                entity.Property(e => e.Price)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("price");

                entity.Property(e => e.WarehouseId).HasColumnName("warehouse_id");

                entity.HasOne(d => d.Warehouse)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.WarehouseId)
                    .HasConstraintName("FK__Order__warehouse__534D60F1");
            });

            modelBuilder.Entity<RefreshTokenAccount>(entity =>
            {
                entity.ToTable("refresh_token_account");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.AccessToken)
                    .HasMaxLength(515)
                    .HasColumnName("access_token");

                entity.Property(e => e.AccountId).HasColumnName("account_id");

                entity.Property(e => e.Expires).HasColumnName("expires");

                entity.Property(e => e.RefreshToken)
                    .HasMaxLength(50)
                    .HasColumnName("refresh_token");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.RefreshTokenAccounts)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__refresh_t__accou__5535A963");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.RoleName)
                    .HasMaxLength(255)
                    .HasColumnName("role_name");
            });

            modelBuilder.Entity<Shipper>(entity =>
            {
                entity.ToTable("Shipper");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.AccountId).HasColumnName("account_id");

                entity.Property(e => e.DateOfBirth)
                    .HasColumnType("date")
                    .HasColumnName("date_of_birth");

                entity.Property(e => e.FullName)
                    .HasMaxLength(255)
                    .HasColumnName("full_name");

                entity.Property(e => e.Gender).HasColumnName("gender");

                entity.Property(e => e.Location)
                    .HasMaxLength(255)
                    .HasColumnName("location");

                entity.Property(e => e.Phone)
                    .HasMaxLength(255)
                    .HasColumnName("phone");

                entity.Property(e => e.WarehouseId).HasColumnName("warehouse_id");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Shippers)
                    .HasForeignKey(d => d.AccountId)
                    .HasConstraintName("FK__Shipper__account__571DF1D5");

                entity.HasOne(d => d.Warehouse)
                    .WithMany(p => p.Shippers)
                    .HasForeignKey(d => d.WarehouseId)
                    .HasConstraintName("FK__Shipper__warehou__59063A47");
            });

            modelBuilder.Entity<StaticFile>(entity =>
            {
                entity.ToTable("static_file");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Img).HasColumnName("img");

                entity.Property(e => e.OrderId).HasColumnName("order_id");

                entity.Property(e => e.Video).HasColumnName("video");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.StaticFiles)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__static_fi__order__5AEE82B9");
            });

            modelBuilder.Entity<Warehouse>(entity =>
            {
                entity.ToTable("Warehouse");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.AccountId).HasColumnName("account_id");

                entity.Property(e => e.DateOfBirth)
                    .HasColumnType("date")
                    .HasColumnName("date_of_birth");

                entity.Property(e => e.FullName)
                    .HasMaxLength(255)
                    .HasColumnName("full_name");

                entity.Property(e => e.Gender).HasColumnName("gender");

                entity.Property(e => e.Location)
                    .HasMaxLength(255)
                    .HasColumnName("location");

                entity.Property(e => e.Phone)
                    .HasMaxLength(255)
                    .HasColumnName("phone");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Warehouses)
                    .HasForeignKey(d => d.AccountId)
                    .HasConstraintName("FK__Warehouse__accou__5BE2A6F2");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
