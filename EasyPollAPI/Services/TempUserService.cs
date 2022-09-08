using EasyPollAPI.DTO;
using EasyPollAPI.Models;
using EasyPollAPI.Scripts;

namespace EasyPollAPI.Services
{
    public class TempUserService
    {

        private readonly EasyPollContext _ctx;
        public TempUserService(EasyPollContext easyPollContext)
        {
            _ctx = easyPollContext;
        }

        public async Task<Boolean> AuthenticateAccessToken(string accessToken)
        {
            var user = _ctx.TempUsers.FirstOrDefault(tu => tu.AccessToken == accessToken);

            if (user == null)
                return false;
            return true;

        }

        public async Task<string> JoinPollGame(JoinPollGameDTO joinPollGameDTO)
        {
            var pollGame = _ctx.PollGames.FirstOrDefault(pg => pg.InviteCode == joinPollGameDTO.InviteCode);
            if (pollGame == null)
                throw new Exception("Invalid invite code");

            var newTempUser = new TempUser()
            {

                AccessToken = TempUserUtils.GenerateAccessToken(),
                DisplayName = joinPollGameDTO.DisplayName,
                isAdmin = true,
            };
            newTempUser.PollGame = pollGame;
            await _ctx.TempUsers.AddAsync(newTempUser);
            await _ctx.SaveChangesAsync();

            return newTempUser.AccessToken;
        }
    }
}
