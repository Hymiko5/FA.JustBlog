﻿// <auto-generated />
using System;
using FA.JustBlog.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FA.JustBlog.Core.Migrations
{
    [DbContext(typeof(JustBlogContext))]
    [Migration("20230905073301_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("FA.JustBlog.Core.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("UrlSlug")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Description = "Latest tech news and updates.",
                            Name = "Technology",
                            UrlSlug = "technology"
                        },
                        new
                        {
                            Id = 2,
                            Description = "Explore exciting travel destinations and experiences.",
                            Name = "Travel",
                            UrlSlug = "travel"
                        },
                        new
                        {
                            Id = 3,
                            Description = "Delicious recipes and culinary delights.",
                            Name = "Food",
                            UrlSlug = "food"
                        });
                });

            modelBuilder.Entity("FA.JustBlog.Core.Models.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CommentHeader")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("CommentText")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CommentTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("PostId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PostId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("FA.JustBlog.Core.Models.Post", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<bool>("Modified")
                        .HasColumnType("bit");

                    b.Property<string>("PostContent")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("PostedOn")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Published")
                        .HasColumnType("bit");

                    b.Property<int>("RateCount")
                        .HasColumnType("int");

                    b.Property<string>("ShortDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("TotalRate")
                        .HasColumnType("int");

                    b.Property<string>("UrlSlug")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("ViewCount")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Posts");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CategoryId = 1,
                            Modified = false,
                            PostContent = "Artificial Intelligence (AI) is transforming...",
                            PostedOn = new DateTime(2023, 8, 29, 14, 33, 1, 151, DateTimeKind.Local).AddTicks(9603),
                            Published = true,
                            RateCount = 0,
                            ShortDescription = "Discover the world of AI and its applications.",
                            Title = "Introduction to Artificial Intelligence",
                            TotalRate = 0,
                            UrlSlug = "introduction-to-ai",
                            ViewCount = 0
                        },
                        new
                        {
                            Id = 2,
                            CategoryId = 2,
                            Modified = false,
                            PostContent = "Bali is a tropical paradise known for its lush...",
                            PostedOn = new DateTime(2023, 8, 26, 14, 33, 1, 151, DateTimeKind.Local).AddTicks(9630),
                            Published = true,
                            RateCount = 0,
                            ShortDescription = "A journey through the stunning landscapes of Bali.",
                            Title = "Exploring Bali: Paradise on Earth",
                            TotalRate = 0,
                            UrlSlug = "exploring-bali",
                            ViewCount = 0
                        },
                        new
                        {
                            Id = 3,
                            CategoryId = 3,
                            Modified = false,
                            PostContent = "Who can resist a moist and decadent chocolate cake...",
                            PostedOn = new DateTime(2023, 8, 31, 14, 33, 1, 151, DateTimeKind.Local).AddTicks(9637),
                            Published = false,
                            RateCount = 0,
                            ShortDescription = "Indulge in the rich flavors of this chocolate cake.",
                            Title = "Delicious Chocolate Cake Recipe",
                            TotalRate = 0,
                            UrlSlug = "chocolate-cake-recipe",
                            ViewCount = 0
                        });
                });

            modelBuilder.Entity("FA.JustBlog.Core.Models.PostTagMap", b =>
                {
                    b.Property<int>("PostId")
                        .HasColumnType("int");

                    b.Property<int>("TagId")
                        .HasColumnType("int");

                    b.HasKey("PostId", "TagId");

                    b.HasIndex("TagId");

                    b.ToTable("PostTagMaps");

                    b.HasData(
                        new
                        {
                            PostId = 1,
                            TagId = 2
                        },
                        new
                        {
                            PostId = 2,
                            TagId = 3
                        },
                        new
                        {
                            PostId = 3,
                            TagId = 1
                        });
                });

            modelBuilder.Entity("FA.JustBlog.Core.Models.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Count")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("UrlSlug")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id");

                    b.ToTable("Tags");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Count = 0,
                            Description = "Artificial Intelligence",
                            Name = "AI",
                            UrlSlug = "ai"
                        },
                        new
                        {
                            Id = 2,
                            Count = 0,
                            Description = "Tech and Gadgets",
                            Name = "Technology",
                            UrlSlug = "technology-tag"
                        },
                        new
                        {
                            Id = 3,
                            Count = 0,
                            Description = "Travel Destinations",
                            Name = "Travel",
                            UrlSlug = "travel-tag"
                        });
                });

            modelBuilder.Entity("FA.JustBlog.Core.Models.Comment", b =>
                {
                    b.HasOne("FA.JustBlog.Core.Models.Post", "Post")
                        .WithMany("Comments")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Post");
                });

            modelBuilder.Entity("FA.JustBlog.Core.Models.Post", b =>
                {
                    b.HasOne("FA.JustBlog.Core.Models.Category", "Category")
                        .WithMany("Posts")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("FA.JustBlog.Core.Models.PostTagMap", b =>
                {
                    b.HasOne("FA.JustBlog.Core.Models.Post", "Post")
                        .WithMany("PostTagMaps")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FA.JustBlog.Core.Models.Tag", "Tag")
                        .WithMany("PostTagMaps")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Post");

                    b.Navigation("Tag");
                });

            modelBuilder.Entity("FA.JustBlog.Core.Models.Category", b =>
                {
                    b.Navigation("Posts");
                });

            modelBuilder.Entity("FA.JustBlog.Core.Models.Post", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("PostTagMaps");
                });

            modelBuilder.Entity("FA.JustBlog.Core.Models.Tag", b =>
                {
                    b.Navigation("PostTagMaps");
                });
#pragma warning restore 612, 618
        }
    }
}
