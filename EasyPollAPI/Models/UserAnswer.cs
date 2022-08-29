namespace EasyPollAPI.Models
{
    public class UserAnswer
    {
        public int Id { get; set; }
        public QuestionAlternative QuestionAlternative { get; set; }
        public TempUser TempUser { get; set; }
    }
}
