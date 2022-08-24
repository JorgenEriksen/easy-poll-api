namespace EasyPollAPI.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int QuestionOrder { get; set; }
        public int TimeInSeconds { get; set; }
        public PollGame PollGame { get; set; }
        public virtual ICollection<QuestionAlternative> QuestionAlternative { get; set; }

    }
}
