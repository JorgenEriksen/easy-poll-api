namespace EasyPollAPI.DTO
{
    public class TempUserDTO
    {
        public int? Id { get; set; }
        public string? AccessToken { get; set; }
        public bool? isAdmin { get; set; }
        public string DisplayName { get; set; }
    }
}
