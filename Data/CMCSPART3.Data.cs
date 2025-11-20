using CMCSPART3.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Claims;

namespace CMCSPART3.Data
{
    public class CMCSPART3DbContext : DbContext
    {
        public CMCSPART3DbContext(DbContextOptions<CMCSPART3DbContext> options)
            : base(options)
        {
        }

        public DbSet<Claim> Claims { get; set; }

        // If you have a Document model, use this:
        // public DbSet<Document> Documents { get; set; }

        public DbSet<User> Users { get; set; }
    }
}
