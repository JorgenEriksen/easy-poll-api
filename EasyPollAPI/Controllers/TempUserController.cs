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
        public async Task<ActionResult<TempUserDTO>> AuthenticateAccessToken()
        {
            var key = Request.Headers.TryGetValue("authorization", out var accessToken);
            if (!key)
                return NotFound("missing accesstoken");

            try
            {
                var tempUserDTO = await _tempUserService.GetTempUserByAccessToken(accessToken);
                return Ok(tempUserDTO);
            } catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost]
        public async Task<ActionResult<TempUserDTO>> CreateUserAndJoinPollGame(JoinPollGameDTO joinPollGameDTO)
        {
            var tempUserDTO = await _tempUserService.CreateUserAndJoinPollGame(joinPollGameDTO);
            return Ok(tempUserDTO);
        }

        [HttpGet("PollGameData")]
        public async Task<ActionResult> GetPollGameDataByUserToken()
        {
            var key = Request.Headers.TryGetValue("Authorization", out var accessToken);
            if (!key)
                return NotFound("missing accesstoken");


            var isValid = await _tempUserService.AuthenticateAccessToken(accessToken);
            if (!isValid)
                return NotFound("unvalid accesstoken");

            try
            {
                var pollGameData = _tempUserService.GetPollGameDataByUserToken(accessToken);
                return Ok(pollGameData);
            } catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
