using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NCB.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NCB.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<NCB.Models.Accounts> Accounts { get; set; }
        public DbSet<NCB.Models.Transactions> Transactions { get; set; }
    }
}