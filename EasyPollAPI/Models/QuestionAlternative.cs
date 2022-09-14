namespace EasyPollAPI.Models
{
    public class QuestionAlternative
    {
        public int Id { get; set; }
        public string AlternativeText { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; set; }

    }
}
