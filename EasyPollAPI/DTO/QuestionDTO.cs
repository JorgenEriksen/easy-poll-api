namespace EasyPollAPI.DTO
{
    public class QuestionDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int? QuestionOrder { get; set; }
        public List<QuestionAlternativeDTO> QuestionAlternatives { get; set; }
    }
}
