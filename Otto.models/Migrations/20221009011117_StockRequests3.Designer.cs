﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Otto.models;

#nullable disable

namespace Otto.models.Migrations
{
    [DbContext(typeof(OttoDbContext))]
    [Migration("20221009011117_StockRequests3")]
    partial class StockRequests3
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Otto.models.Company", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("CUIT")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("CreatedByUserId")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Owner")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Province")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("Otto.models.JoinRequest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("CompanyId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("Created")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Modified")
                        .HasColumnType("datetime2");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("JoinRequests");
                });

            modelBuilder.Entity("Otto.models.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("CompanyId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("EAN")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("InProgress")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("InProgressDateTimeModified")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("InProgressDateTimeTaken")
                        .HasColumnType("datetime2");

                    b.Property<string>("ItemDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ItemId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("MOrderId")
                        .HasColumnType("bigint");

                    b.Property<long?>("MShippingId")
                        .HasColumnType("bigint");

                    b.Property<long?>("MUserId")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("Modified")
                        .HasColumnType("datetime2");

                    b.Property<string>("PackId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ProductInStockId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<string>("SKU")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ShippingStatus")
                        .HasColumnType("int");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.Property<string>("StateDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.Property<int?>("UserIdInProgress")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("Otto.models.ProductInStock", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Batch")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Category")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CompanyId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Location")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MItemId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MSellerId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Origin")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<string>("SKU")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Size")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.Property<string>("StateDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TItemId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TSellerId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("UserIdMail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ProductsInStock");
                });

            modelBuilder.Entity("Otto.models.StockRequest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Batch")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Category")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CompanyId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Location")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MItemId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MSellerId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("Modified")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Origin")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<string>("SKU")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Size")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.Property<string>("StateDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TItemId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TSellerId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("UserIdMail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("StockRequests");
                });

            modelBuilder.Entity("Otto.models.Token", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("AccessToken")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("Active")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("Created")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ExpiresAt")
                        .HasColumnType("datetime2");

                    b.Property<long?>("ExpiresIn")
                        .HasColumnType("bigint");

                    b.Property<long?>("MUserId")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("Modified")
                        .HasColumnType("datetime2");

                    b.Property<string>("RefreshToken")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SalesChannel")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("TUserId")
                        .HasColumnType("bigint");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Tokens");
                });

            modelBuilder.Entity("Otto.models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("CompanyId")
                        .HasColumnType("int");

                    b.Property<string>("MUserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Mail")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Pass")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Rol")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TUserId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Id", "Mail")
                        .IsUnique();

                    b.ToTable("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
