namespace EasyPollAPI.Models
{
    public class PollGame
    {
        public int Id { get; set; }
        public bool HasStarted { get; set; }
        public bool AdminIsParticipating { get; set; }
        public TempUser AdminUser { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
    }
}
