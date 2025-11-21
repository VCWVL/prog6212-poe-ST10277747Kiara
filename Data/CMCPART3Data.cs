
using Microsoft.EntityFrameworkCore;

namespace CMCSPART3.Data
{
    public class CMCPART3Data : DbContext
    {
        public CMCPART3Data(DbContextOptions<CMCPART3Data> options) : base(options)
        {
        }

        // TODO: Add DbSet<TEntity> properties for your entities, for example:
         public DbSet<User> Users { get; set; }
         public DbSet<Document> Documents { get; set; }
    }
}
