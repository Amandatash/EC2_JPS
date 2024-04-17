using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using JPS.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace JPS.Data
{
    public class JPSContext : IdentityDbContext
    {
        public JPSContext (DbContextOptions<JPSContext> options)
            : base(options)
        {
        }

        public DbSet<JPS.Models.Bill> Bill { get; set; }

        internal Task FindAsync(string premisesNumber)
        {
            throw new NotImplementedException();
        }
    }
}
