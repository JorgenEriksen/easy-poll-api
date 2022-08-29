using EasyPollAPI.DTO;
using EasyPollAPI.Models;
using EasyPollAPI.Scripts;

namespace EasyPollAPI.Services
{
    public class PollGameService
    {
        private readonly EasyPollContext _ctx;
        public PollGameService(EasyPollContext easyPollContext)
        {
            _ctx = easyPollContext;
        }
        public async Task CreateNewPollGame(PollGameDTO pollGameDTO)
        {
            var newTempUser = new TempUser()
            {

                AccessToken = TempUserUtils.GenerateAccessToken(),
                DisplayName = pollGameDTO.AdminUser.DisplayName,
            };

            System.Diagnostics.Debug.WriteLine(newTempUser.AccessToken);

            //_ctx.TempUser.AddAsync(newTempUser);

        }
    }
}
