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
            await _pollGameService.CreateNewPollGame(pollGameDTO);
            return Ok();
        }
    }
}
