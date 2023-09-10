using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FA.JustBlog.Core.Models
{
    public class JustBlogContext: IdentityDbContext<AppUser, AppRole, Guid>
    {
        //private readonly string _connectionString;
        public JustBlogContext()
        {
            //_connectionString = connectionString;
        }
        public JustBlogContext(DbContextOptions options):base(options)
        {
            
        }
        /// <summary>
        /// Category Dbset.
        /// </summary>
        public virtual DbSet<Category> Categories { get; set; }
        /// <summary>
        /// Post Dbset.
        /// </summary>
        public virtual DbSet<Post> Posts { get; set; }
        /// <summary>
        /// Tag DbSet.
        /// </summary>
        public virtual DbSet<Tag> Tags { get; set; }
        /// <summary>
        /// PostTagMaps DbSet.
        /// </summary>
        public virtual DbSet<PostTagMap> PostTagMaps { get; set; }

        public virtual DbSet<Comment> Comments { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<PostTagMap>().HasKey(pt => new { pt.PostId, pt.TagId });
            modelBuilder.Entity<PostTagMap>().HasOne(pt => pt.Post)
                .WithMany(p => p.PostTagMaps)
                .HasForeignKey(pt => pt.PostId);
            modelBuilder.Entity<PostTagMap>().HasOne(pt => pt.Tag)
                .WithMany(t => t.PostTagMaps)
                .HasForeignKey(pt => pt.TagId);

            modelBuilder.Entity<Category>().HasData(
        new Category { Id = 1, Name = "Technology", UrlSlug = "technology", Description = "Latest tech news and updates." },
        new Category { Id = 2, Name = "Travel", UrlSlug = "travel", Description = "Explore exciting travel destinations and experiences." },
        new Category { Id = 3, Name = "Food", UrlSlug = "food", Description = "Delicious recipes and culinary delights." }
    );

            modelBuilder.Entity<Post>().HasData(
                new Post
                {
                    Id = 1,
                    Title = "Introduction to Artificial Intelligence",
                    ShortDescription = "Discover the world of AI and its applications.",
                    PostContent = "Artificial Intelligence (AI) is transforming...",
                    UrlSlug = "introduction-to-ai",
                    Published = true,
                    PostedOn = DateTime.Now.AddDays(-7),
                    CategoryId = 1
                },
                new Post
                {
                    Id = 2,
                    Title = "Exploring Bali: Paradise on Earth",
                    ShortDescription = "A journey through the stunning landscapes of Bali.",
                    PostContent = "Bali is a tropical paradise known for its lush...",
                    UrlSlug = "exploring-bali",
                    Published = true,
                    PostedOn = DateTime.Now.AddDays(-10),
                    CategoryId = 2
                },
                new Post
                {
                    Id = 3,
                    Title = "Delicious Chocolate Cake Recipe",
                    ShortDescription = "Indulge in the rich flavors of this chocolate cake.",
                    PostContent = "Who can resist a moist and decadent chocolate cake...",
                    UrlSlug = "chocolate-cake-recipe",
                    Published = false,
                    PostedOn = DateTime.Now.AddDays(-5),
                    CategoryId = 3
                }
            );

            modelBuilder.Entity<Tag>().HasData(
                new Tag { Id = 1, Name = "AI", UrlSlug = "ai", Description = "Artificial Intelligence" },
                new Tag { Id = 2, Name = "Technology", UrlSlug = "technology-tag", Description = "Tech and Gadgets" },
                new Tag { Id = 3, Name = "Travel", UrlSlug = "travel-tag", Description = "Travel Destinations" }
            );
            modelBuilder.Entity<PostTagMap>().HasData(
                new PostTagMap { PostId = 1, TagId = 2 },
                new PostTagMap { PostId = 2, TagId = 3 },
                new PostTagMap { PostId = 3, TagId = 1 }
                );
            DataInitializer.SeedUsers(modelBuilder);
            DataInitializer.SeedRoles(modelBuilder);
            DataInitializer.SeedUserRoles(modelBuilder);
            
        }
    }
}
