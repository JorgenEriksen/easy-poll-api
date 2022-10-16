using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EasyPollAPI.DTO;
using EasyPollAPI.Services;

namespace EasyPollAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PollGameController : ControllerBase
    {
        PollGameService _pollGameService;

        public PollGameController(PollGameService pollGameService)
        {
            _pollGameService = pollGameService;
        }

        [HttpPost]
        public async Task<ActionResult> CreateNewPollGame(PollGameDTO pollGameDTO)
        {
            try
            {
                var accessToken = await _pollGameService.CreateNewPollGame(pollGameDTO);
                return Ok(accessToken);
            } catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost("Authenticate")]
        public async Task<ActionResult> AuthenticateInviteCode(string inviteCode)
        {   
            try
            {
                await _pollGameService.AuthenticateInviteCode(inviteCode);
                return Ok();
            } catch(Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("Start")]
        public async Task<ActionResult> StartPollGame()
        {
            var key = Request.Headers.TryGetValue("Authorization", out var accessToken);
            if (!key)
                return NotFound("missing accesstoken");

            try
            {
                await _pollGameService.StartPollGame(accessToken);
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<ActionResult> DeletePollGame()
        {
            var key = Request.Headers.TryGetValue("Authorization", out var accessToken);
            if (!key)
                return NotFound("missing accesstoken");

            try
            {
                await _pollGameService.DeletePollGame(accessToken);
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }



    }
}
