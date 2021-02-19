using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportPlatform.Database
{
    public class UserSeeder
    {
        public static async Task SeedUsers(UserManager<UserEntity> userManager, RoleManager<RoleEntity> roleManager)
        {
            string roleName = "Employee";
            var employees = await userManager.GetUsersInRoleAsync(roleName);
            var roleExist = await roleManager.RoleExistsAsync(roleName);

            if(employees.Count == 0)
            {
                List<UserEntity> users = new List<UserEntity>
                {
                    new UserEntity
                    {
                        UserName = "admin",
                        Email = "admin@supportplatformapp.com",
                    },
                    new UserEntity
                    {
                        UserName = "employee",
                        Email = "employee@supportplatformapp.com",
                    }
                };

                foreach (var user in users)
                {
                    await userManager.CreateAsync(user, "password");

                    if (!roleExist)
                    {
                        await roleManager.CreateAsync(new RoleEntity
                        {
                            Name = roleName
                        });
                    }

                    await userManager.AddToRoleAsync(user, roleName);
                }
            }
        }

    }
}
