using EasyPollAPI.DTO;
using EasyPollAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EasyPollAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        QuestionService _questionService;
        TempUserService _tempUserService;

        public QuestionController(QuestionService questionService, TempUserService tempUserService)
        {
            _questionService = questionService;
            _tempUserService = tempUserService;
        }

        /*

        [HttpGet]
        public async Task<ActionResult<QuestionToClientDTO>> GetQuestionByUserToken()
        {
            var key = Request.Headers.TryGetValue("Authorization", out var accessToken);
            if (!key)
                return NotFound("missing accesstoken");


            var isValid = await _tempUserService.AuthenticateAccessToken(accessToken);
            if (!isValid)
                return NotFound("unvalid accesstoken");

            var questionToClientDTO = await _questionService.GetQuestionByUserToken(accessToken);
            return Ok(questionToClientDTO);
        }
        */

    }
}
