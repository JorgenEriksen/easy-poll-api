namespace EasyPollAPI.DTO
{
    public class QuestionToClientDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool HasStarted { get; set; }
        public string InviteCode { get; set; }
        public bool IsAdmin { get; set; }
        public bool AdminIsParticipating { get; set; }
        public int? QuestionOrder { get; set; }
        public List<TempUserDTO> TempUsers { get; set; }
        public List<QuestionAlternativeDTO> QuestionAlternatives { get; set; }
    }
}
