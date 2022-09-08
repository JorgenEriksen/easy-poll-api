using EasyPollAPI.DTO;
using EasyPollAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EasyPollAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TempUserController : ControllerBase
    {
        TempUserService _tempUserService;

        public TempUserController(TempUserService tempUserService)
        {
            _tempUserService = tempUserService;
        }

        [HttpPost("Authenticate")]
        public async Task<ActionResult> AuthenticateAccessToken()
        {
            var key = Request.Headers.TryGetValue("authorization", out var accessToken);

            if (!key)
                return NotFound("missing accesstoken");


            var isValid = await _tempUserService.AuthenticateAccessToken(accessToken);
            return Ok(new {isValid = isValid});
        }

        [HttpPost]
        public async Task<ActionResult> JoinPollGame(JoinPollGameDTO joinPollGameDTO)
        {
            var accessToken = await _tempUserService.JoinPollGame(joinPollGameDTO);
            return Ok(accessToken);
        }

    }
}
