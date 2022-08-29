namespace EasyPollAPI.Scripts
{
    public class TempUserUtils
    {
        public static string GenerateAccessToken()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}
