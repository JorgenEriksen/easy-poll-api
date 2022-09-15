namespace EasyPollAPI.DTO
{
    public class PollGameDataToClientDTO
    {
        public int Id { get; set; }
        public bool HasStarted { get; set; }
        public string InviteCode { get; set; }
        public TempUserDTO Admin { get; set; }
        public bool AdminIsParticipating { get; set; }
        public int NumberOfAnswers { get; set; }
        public QuestionDTO Question { get; set; }
        public List<TempUserDTO> TempUsers { get; set; }

    }
}
