using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FA.JustBlog.Core.Models
{
    public class DataInitializer
    {
        public static void SeedUsers(ModelBuilder builder)
        {
            var storeOwner = new AppUser()
            {
                Id = new Guid("A4B127A5-5538-4C45-921B-11F144D1B5E9"),
                UserName = "storeowner@northwind.com",
                FirstName = "Store",
                LastName = "Owner",
                Email = "storeowner@northwind.com",
                LockoutEnabled = false,
                PhoneNumber = "0123456789"
            };



            PasswordHasher<AppUser> passwordHasher = new PasswordHasher<AppUser>();



            storeOwner.PasswordHash = passwordHasher.HashPassword(storeOwner, "StoreOwner#123");
            storeOwner.NormalizedUserName = storeOwner.UserName.ToUpper();
            storeOwner.NormalizedEmail = storeOwner.Email.ToUpper();
            storeOwner.EmailConfirmed = true;
            storeOwner.PhoneNumberConfirmed = true;
            storeOwner.SecurityStamp = Guid.NewGuid().ToString(); //// To avoid null exception
            builder.Entity<AppUser>().HasData(storeOwner);
        }
        public static void SeedRoles(ModelBuilder builder)
        {
            builder.Entity<AppRole>().HasData(
                new AppRole() { Id = new Guid("1104047D-BFD5-48FE-9396-D3C16C680A45"), Name=UserRoles.StoreOwner },
                new AppRole() { Id = new Guid("E739CC8B-3828-494A-A746-A63B504C0F8A"), Name =UserRoles.Employee },
                new AppRole() { Id = new Guid("D8D25B26-2CBF-482A-8536-5BF9F4534074") },
                new AppRole() { Id = new Guid("C2CBEAE2-F552-42D1-B913-C5071594068E"), Name = UserRoles.Customer }
                );
        }
        public static void SeedUserRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityUserRole<Guid>>().HasData(
                new IdentityUserRole<Guid>() { RoleId = new Guid("1104047D-BFD5-48FE-9396-D3C16C680A45"), UserId = new Guid("A4B127A5-5538-4C45-921B-11F144D1B5E9") }
                );
        }
    }
}
