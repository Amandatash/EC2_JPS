using NCB.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace NCB.Data
{
    public class SeedData
    {
        public static async Task EnsureSeedData(IServiceProvider provider)
        {
            var dbContext = provider.GetRequiredService<ApplicationDbContext>();
            await dbContext.Database.MigrateAsync();
            var id = "";
            if (!await dbContext.Roles.Where(c => c.Name == "Admin").AnyAsync())
            {

                var role = new IdentityRole { Id = Guid.NewGuid().ToString(), Name = "Admin", NormalizedName = "ADMIN" };
                id = role.Id;
                await dbContext.Roles.AddAsync(role);
                await dbContext.SaveChangesAsync();
            }
            if (!await dbContext.Roles.Where(c => c.Name == "Customer").AnyAsync())
            {
                var role = new IdentityRole { Id = Guid.NewGuid().ToString(), Name = "Customer", NormalizedName = "Customer".ToUpper() };
                await dbContext.Roles.AddAsync(role);
                await dbContext.SaveChangesAsync();
            }
            if (!await dbContext.Roles.Where(c => c.Name == "Teller").AnyAsync())
            {
                var role = new IdentityRole { Id = Guid.NewGuid().ToString(), Name = "Teller", NormalizedName = "Teller".ToUpper() };
                await dbContext.Roles.AddAsync(role);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
