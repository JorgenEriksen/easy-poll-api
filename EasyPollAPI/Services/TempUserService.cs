using EasyPollAPI.DTO;
using EasyPollAPI.Hubs;
using EasyPollAPI.Models;
using EasyPollAPI.Scripts;
using Microsoft.AspNetCore.SignalR;

namespace EasyPollAPI.Services
{
    public class TempUserService
    {

        private readonly EasyPollContext _ctx;
        private readonly IHubContext<PollGameHub> _hubContext;
        private readonly PollGameService _pollGameService;
        public TempUserService(EasyPollContext easyPollContext, IHubContext<PollGameHub> hubContext, PollGameService pollGameService)
        {
            _ctx = easyPollContext;
            _hubContext = hubContext;
            _pollGameService = pollGameService;
        }

        public async Task<Boolean> AuthenticateAccessToken(string accessToken)
        {
            var user = _ctx.TempUsers.FirstOrDefault(tu => tu.AccessToken == accessToken);

            if (user == null)
                return false;
            return true;

        }

        public async Task<PollGameDataToClientDTO> GetPollGameDataByUserToken(string accessToken)
        {
            var currentUser = _ctx.TempUsers.FirstOrDefault(tu => tu.AccessToken == accessToken);
            if (currentUser == null)
                throw new Exception("Not valid user! (this should never happen)");

            var pollGame = _ctx.PollGames.FirstOrDefault(pg => pg.Id == currentUser.PollGameId);
            if (pollGame == null)
                throw new Exception("Can't find poll game! (this should never happen)");
            PollGameDataToClientDTO PollGameData = await _pollGameService.GetGameDataByGameId(pollGame.Id);
            return PollGameData;
        }

        public async Task<string> CreateUserAndJoinPollGame(JoinPollGameDTO joinPollGameDTO)
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
            //await _ctx.SaveChangesAsync();
            var connectionString = "Socket-PollGameId" + pollGame.Id;


            await _hubContext.Clients.All.SendAsync(connectionString, "test test");

            return newTempUser.AccessToken;
        }
    }
}
