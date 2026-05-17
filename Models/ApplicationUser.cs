using Microsoft.AspNetCore.Identity;

namespace RailwayReservation.Models
{
    // Inheriting from IdentityUser gives you ID, Username, Email, and PasswordHash automatically
    public class ApplicationUser : IdentityUser
    {
        // Add any extra columns you want in your database here
        public string? FullName { get; set; }
        
        // You don't need a 'Role' string property here because 
        // Identity uses a separate table (AspNetUserRoles) to manage roles.
    }
}