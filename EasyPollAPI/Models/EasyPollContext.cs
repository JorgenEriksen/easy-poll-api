using Microsoft.EntityFrameworkCore;

namespace EasyPollAPI.Models
{
    public class EasyPollContext : DbContext
    {
        public EasyPollContext(DbContextOptions<EasyPollContext> options) : base(options)
        {
        }


        public DbSet<TempUser> Users { get; set; }
        public DbSet<PollGame> PollGames { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionAlternative> QuestionAlternative { get; set; }
        public DbSet<UserAnswer> UserAnswer { get; set; }

    }
}
