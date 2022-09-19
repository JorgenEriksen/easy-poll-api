namespace EasyPollAPI.Models
{
    public class UserAnswer
    {
        public int Id { get; set; }
        public int QuestionAlternativeId { get; set; }
        public QuestionAlternative QuestionAlternative { get; set; }

        public int TempUserId { get; set; }
        public TempUser? TempUser { get; set; }
    }
}
