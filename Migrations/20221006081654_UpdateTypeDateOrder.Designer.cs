﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PetStoreApi.Helpers;

#nullable disable

namespace PetStoreApi.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20221006081654_UpdateTypeDateOrder")]
    partial class UpdateTypeDateOrder
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("PetStoreApi.Data.Entity.AppRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("AppRole", (string)null);
                });

            modelBuilder.Entity("PetStoreApi.Data.Entity.AppUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("AccountNonLocked")
                        .HasColumnType("bit");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<bool>("Enabled")
                        .HasColumnType("bit");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserInfoId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserInfoId")
                        .IsUnique();

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("AppUser", (string)null);
                });

            modelBuilder.Entity("PetStoreApi.Data.Entity.AppUserProduct", b =>
                {
                    b.Property<Guid>("ProductId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateModified")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasComputedColumnSql("dateadd(hour, 7, getutcdate())");

                    b.Property<bool?>("Favourite")
                        .HasColumnType("bit");

                    b.Property<int?>("Rate")
                        .HasColumnType("int");

                    b.Property<string>("Remark")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ProductId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("AppUserProduct", (string)null);
                });

            modelBuilder.Entity("PetStoreApi.Data.Entity.AppUserRole", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AppUserRole", (string)null);
                });

            modelBuilder.Entity("PetStoreApi.Data.Entity.Breed", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Breed", (string)null);
                });

            modelBuilder.Entity("PetStoreApi.Data.Entity.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Category", (string)null);
                });

            modelBuilder.Entity("PetStoreApi.Data.Entity.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("OrderDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("dateadd(hour, 7, getutcdate())");

                    b.Property<int>("OrderStatusId")
                        .HasColumnType("int");

                    b.Property<int>("PaymentId")
                        .HasColumnType("int");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Reciever")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<double>("TotalPrice")
                        .HasColumnType("float");

                    b.Property<int>("TotalQuantity")
                        .HasColumnType("int");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("OrderStatusId");

                    b.HasIndex("PaymentId");

                    b.HasIndex("UserId");

                    b.ToTable("Order", (string)null);
                });

            modelBuilder.Entity("PetStoreApi.Data.Entity.OrderItem", b =>
                {
                    b.Property<Guid>("ProductId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("OrderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("ProductId", "OrderId");

                    b.HasIndex("OrderId");

                    b.ToTable("OrderItem", (string)null);
                });

            modelBuilder.Entity("PetStoreApi.Data.Entity.OrderStatus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("OrderStatus", (string)null);
                });

            modelBuilder.Entity("PetStoreApi.Data.Entity.Origin", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Origin", (string)null);
                });

            modelBuilder.Entity("PetStoreApi.Data.Entity.Payment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Payment", (string)null);
                });

            modelBuilder.Entity("PetStoreApi.Data.Entity.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("Age")
                        .HasColumnType("int");

                    b.Property<int>("AmountInStock")
                        .HasColumnType("int");

                    b.Property<int?>("BreedId")
                        .HasColumnType("int");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("Gender")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<int?>("Rate")
                        .HasColumnType("int");

                    b.Property<bool?>("Status")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("BreedId");

                    b.HasIndex("CategoryId");

                    b.ToTable("Product", (string)null);
                });

            modelBuilder.Entity("PetStoreApi.Data.Entity.ProductImage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ImagePath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductImage", (string)null);
                });

            modelBuilder.Entity("PetStoreApi.Data.Entity.ProductOrigin", b =>
                {
                    b.Property<Guid>("ProductId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("OriginId")
                        .HasColumnType("int");

                    b.HasKey("ProductId", "OriginId");

                    b.HasIndex("OriginId");

                    b.ToTable("ProductOrigin", (string)null);
                });

            modelBuilder.Entity("PetStoreApi.Data.Entity.RefreshToken", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("ExpiredAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsRevoked")
                        .HasColumnType("bit");

                    b.Property<bool>("IsUsed")
                        .HasColumnType("bit");

                    b.Property<DateTime>("IssuedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("JwtId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("RefreshToken", (string)null);
                });

            modelBuilder.Entity("PetStoreApi.Data.Entity.UserInfo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AvatarImg")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("UserInfo", (string)null);
                });

            modelBuilder.Entity("PetStoreApi.Data.Entity.VerificationToken", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsSend")
                        .HasColumnType("bit");

                    b.Property<bool>("IsVerify")
                        .HasColumnType("bit");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("VerifyDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("VerificationToken", (string)null);
                });

            modelBuilder.Entity("PetStoreApi.Data.Entity.AppUser", b =>
                {
                    b.HasOne("PetStoreApi.Data.Entity.UserInfo", "UserInfo")
                        .WithOne("AppUser")
                        .HasForeignKey("PetStoreApi.Data.Entity.AppUser", "UserInfoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_AppUser_UserInfo");

                    b.Navigation("UserInfo");
                });

            modelBuilder.Entity("PetStoreApi.Data.Entity.AppUserProduct", b =>
                {
                    b.HasOne("PetStoreApi.Data.Entity.Product", "Product")
                        .WithMany("AppUserProducts")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_AppUserProduct_Product");

                    b.HasOne("PetStoreApi.Data.Entity.AppUser", "AppUser")
                        .WithMany("AppUserProducts")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_AppUserProduct_AppUser");

                    b.Navigation("AppUser");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("PetStoreApi.Data.Entity.AppUserRole", b =>
                {
                    b.HasOne("PetStoreApi.Data.Entity.AppRole", "AppRole")
                        .WithMany("AppUserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_AppUserRole_AppRole");

                    b.HasOne("PetStoreApi.Data.Entity.AppUser", "AppUser")
                        .WithMany("AppUserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_AppUserRole_AppUser");

                    b.Navigation("AppRole");

                    b.Navigation("AppUser");
                });

            modelBuilder.Entity("PetStoreApi.Data.Entity.Order", b =>
                {
                    b.HasOne("PetStoreApi.Data.Entity.OrderStatus", "OrderStatus")
                        .WithMany("Orders")
                        .HasForeignKey("OrderStatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Order_OrderStatus");

                    b.HasOne("PetStoreApi.Data.Entity.Payment", "Payment")
                        .WithMany("Orders")
                        .HasForeignKey("PaymentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Order_Payment");

                    b.HasOne("PetStoreApi.Data.Entity.AppUser", "AppUser")
                        .WithMany("Orders")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Order_AppUser");

                    b.Navigation("AppUser");

                    b.Navigation("OrderStatus");

                    b.Navigation("Payment");
                });

            modelBuilder.Entity("PetStoreApi.Data.Entity.OrderItem", b =>
                {
                    b.HasOne("PetStoreApi.Data.Entity.Order", "Order")
                        .WithMany("OrderItems")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_OrderItem_Order");

                    b.HasOne("PetStoreApi.Data.Entity.Product", "Product")
                        .WithMany("OrderItems")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_OrderItem_Product");

                    b.Navigation("Order");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("PetStoreApi.Data.Entity.Product", b =>
                {
                    b.HasOne("PetStoreApi.Data.Entity.Breed", "Breed")
                        .WithMany("Products")
                        .HasForeignKey("BreedId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK_Product_Breed");

                    b.HasOne("PetStoreApi.Data.Entity.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Product_Category");

                    b.Navigation("Breed");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("PetStoreApi.Data.Entity.ProductImage", b =>
                {
                    b.HasOne("PetStoreApi.Data.Entity.Product", "Product")
                        .WithMany("ProductImages")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_ProductImage_Product");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("PetStoreApi.Data.Entity.ProductOrigin", b =>
                {
                    b.HasOne("PetStoreApi.Data.Entity.Origin", "Origin")
                        .WithMany("ProductOrigins")
                        .HasForeignKey("OriginId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_ProductOrigin_Origin");

                    b.HasOne("PetStoreApi.Data.Entity.Product", "Product")
                        .WithMany("ProductOrigins")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_ProductOrigin_Product");

                    b.Navigation("Origin");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("PetStoreApi.Data.Entity.RefreshToken", b =>
                {
                    b.HasOne("PetStoreApi.Data.Entity.AppUser", "AppUser")
                        .WithMany("RefreshTokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_RefreshToken_AppUser");

                    b.Navigation("AppUser");
                });

            modelBuilder.Entity("PetStoreApi.Data.Entity.VerificationToken", b =>
                {
                    b.HasOne("PetStoreApi.Data.Entity.AppUser", "AppUser")
                        .WithMany("VerificationTokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_VerificationToken_AppUser");

                    b.Navigation("AppUser");
                });

            modelBuilder.Entity("PetStoreApi.Data.Entity.AppRole", b =>
                {
                    b.Navigation("AppUserRoles");
                });

            modelBuilder.Entity("PetStoreApi.Data.Entity.AppUser", b =>
                {
                    b.Navigation("AppUserProducts");

                    b.Navigation("AppUserRoles");

                    b.Navigation("Orders");

                    b.Navigation("RefreshTokens");

                    b.Navigation("VerificationTokens");
                });

            modelBuilder.Entity("PetStoreApi.Data.Entity.Breed", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("PetStoreApi.Data.Entity.Category", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("PetStoreApi.Data.Entity.Order", b =>
                {
                    b.Navigation("OrderItems");
                });

            modelBuilder.Entity("PetStoreApi.Data.Entity.OrderStatus", b =>
                {
                    b.Navigation("Orders");
                });

            modelBuilder.Entity("PetStoreApi.Data.Entity.Origin", b =>
                {
                    b.Navigation("ProductOrigins");
                });

            modelBuilder.Entity("PetStoreApi.Data.Entity.Payment", b =>
                {
                    b.Navigation("Orders");
                });

            modelBuilder.Entity("PetStoreApi.Data.Entity.Product", b =>
                {
                    b.Navigation("AppUserProducts");

                    b.Navigation("OrderItems");

                    b.Navigation("ProductImages");

                    b.Navigation("ProductOrigins");
                });

            modelBuilder.Entity("PetStoreApi.Data.Entity.UserInfo", b =>
                {
                    b.Navigation("AppUser")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
