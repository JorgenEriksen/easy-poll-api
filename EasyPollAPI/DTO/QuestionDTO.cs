namespace EasyPollAPI.DTO
{
    public class QuestionDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int QuestionOrder { get; set; }
        public int TimeInSeconds { get; set; }
        public List<QuestionAlternativeDTO> QuestionAlternative { get; set; }
    }
}
