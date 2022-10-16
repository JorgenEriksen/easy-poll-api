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

        [HttpPost]
        public async Task<ActionResult> SubmitQuestion(SubmitQuestionDTO submitQuestionDTO)
        {
            var key = Request.Headers.TryGetValue("Authorization", out var accessToken);
            if (!key)
                return NotFound("missing accesstoken");


            var isValid = await _tempUserService.AuthenticateAccessToken(accessToken);
            if (!isValid)
                return NotFound("unvalid accesstoken");

            try { 
                await _questionService.SubmitQuestion(accessToken, submitQuestionDTO);
                return Ok();
            } catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
