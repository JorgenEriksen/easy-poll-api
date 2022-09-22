using EasyPollAPI.Models;

namespace EasyPollAPI.DTO
{
    public class PollGameDTO
    {
        public int Id { get; set; }
        public bool HasStarted { get; set; }
        public int? CurrentQuestion { get; set; }
        public TempUserDTO AdminUser { get; set; }
        public List<QuestionDTO> Questions { get; set; }
    }
}
