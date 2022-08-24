namespace EasyPollAPI.Models
{
    public class QuestionAlternative
    {
        public int Id { get; set; };
        public string AlternativeText { get; set; }
        public Question Question { get; set; }

    }
}
