namespace EasyPollAPI.Scripts
{
    public class PollGameUtils
    {
        private static readonly Random random = new Random();
        public static string GenerateInviteCode(int length)
        {

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
