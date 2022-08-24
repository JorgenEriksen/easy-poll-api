namespace EasyPollAPI.Models
{
    public class TempUser
    {
        public int Id { get; set; }
        public string AccessToken { get; set; }
        public string DisplayName { get; set; }
        public PollGame PollGame { get; set; }
    }
}
