using Microsoft.EntityFrameworkCore;
using CMCSP3.Models;

namespace CMCSP3.Data
{
    public class CMCSDbContext : DbContext
    {
        public CMCSDbContext(DbContextOptions<CMCSDbContext> options)
            : base(options)
        {
        }

        public DbSet<Claim> Claims { get; set; }
        public DbSet<Claim> Documents { get; set; }
        
        public DbSet<User> Users { get; set; }



    }
}
