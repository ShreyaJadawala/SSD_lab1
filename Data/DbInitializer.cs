using lab1.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace lab1.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string[] roleNames = { "Manager", "Employee" };

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    var roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                    if (!roleResult.Succeeded)
                    {
                        Console.WriteLine($"Failed to create role: {roleName}");
                        foreach (var error in roleResult.Errors)
                        {
                            Console.WriteLine(error.Description);
                        }
                    }
                }
            }

            // Create Manager user
            var manager = new ApplicationUser
            {
                UserName = "manager@test.com",
                Email = "manager@test.com",
                FirstName = "Manager",
                LastName = "User"
            };

            // Create Employee user
            var employee = new ApplicationUser
            {
                UserName = "employee@test.com",
                Email = "employee@test.com",
                FirstName = "Employee",
                LastName = "User"
            };

            // Seed Manager user if not already present
            if (userManager.Users.All(u => u.Email != manager.Email))
            {
                var result = await userManager.CreateAsync(manager, "Test@123");
                if (result.Succeeded)
                {
                    var roleResult = await userManager.AddToRoleAsync(manager, "Manager");
                    if (!roleResult.Succeeded)
                    {
                        // Log or handle role assignment failure
                        Console.WriteLine("Failed to assign Manager role.");
                    }
                    else
                    {
                        Console.WriteLine("Manager role successfully assigned.");
                        var isManager = await userManager.IsInRoleAsync(manager, "Manager");
                        Console.WriteLine($"Is manager in Manager role: {isManager}");
                    }
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine(error.Description);
                    }
                }
            }

            // Seed Employee user if not already present
            if (userManager.Users.All(u => u.Email != employee.Email))
            {
                var result = await userManager.CreateAsync(employee, "Test@123");
                if (result.Succeeded)
                {
                    var roleResult = await userManager.AddToRoleAsync(employee, "Employee");
                    if (!roleResult.Succeeded)
                    {
                        Console.WriteLine($"Failed to assign Employee role to user: {employee.UserName}");
                    }
                    else
                    {
                        Console.WriteLine("Employee role successfully assigned.");
                        var isEmployee = await userManager.IsInRoleAsync(employee, "Employee");
                        Console.WriteLine($"Is employee in Employee role: {isEmployee}");
                    }
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine(error.Description);
                    }
                }
            }
        }
    }
}
