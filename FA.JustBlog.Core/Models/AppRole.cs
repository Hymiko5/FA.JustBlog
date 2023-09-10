using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FA.JustBlog.Core.Models
{
    public class AppRole: IdentityRole<Guid>
    {
        public string? Description { get; set; }
    }
    public class UserRoles
    {
        public const string StoreOwner = "Store Owner";
        public const string Employee = "Employee";
        public const string Shipper = "Shipper";
        public const string Customer = "Customer";
        public const string EmployeeOrOwner = "Employee, Store Owner";
    }
}
