using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BNS.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BNS.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<BNS.Models.Accounts> Accounts { get; set; }
        public DbSet<BNS.Models.Transactions> Transactions { get; set; }
    }
}