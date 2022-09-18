namespace EasyPollAPI.DTO
{
    public class PollGameResultToClientDTO
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public List<QuestionDTO> Questions { get; set; }
        public List<TempUserDTO> TempUsers { get; set; }

    }
}
