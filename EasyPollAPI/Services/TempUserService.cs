using EasyPollAPI.Models;

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
    }
}
