using Microsoft.EntityFrameworkCore;

namespace EasyPollAPI.Models
{
    public class EasyPollContext : DbContext
    {
        public EasyPollContext(DbContextOptions<EasyPollContext> options) : base(options)
        {
        }


        public DbSet<TempUser> Users { get; set; }
    }
}
