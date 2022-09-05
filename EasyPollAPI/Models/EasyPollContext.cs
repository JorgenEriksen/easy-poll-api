using Microsoft.EntityFrameworkCore;

namespace EasyPollAPI.Models
{
    public class EasyPollContext : DbContext
    {
        public EasyPollContext(DbContextOptions<EasyPollContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }


        public DbSet<TempUser> TempUsers { get; set; }
        public DbSet<PollGame> PollGames { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionAlternative> QuestionAlternatives { get; set; }
        public DbSet<UserAnswer> UserAnswers { get; set; }

    }
}
