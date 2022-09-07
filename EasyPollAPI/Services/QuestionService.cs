using EasyPollAPI.DTO;
using EasyPollAPI.Models;

namespace EasyPollAPI.Services
{
    public class QuestionService
    {

        private readonly EasyPollContext _ctx;
        public QuestionService(EasyPollContext easyPollContext)
        {
            _ctx = easyPollContext;
        }


        // usertoken is valid here
        public async Task<QuestionToClientDTO> GetQuestionByUserToken(string accessToken)
        {
            var currentUser = _ctx.TempUsers.FirstOrDefault(tu => tu.AccessToken == accessToken);
            if (currentUser == null)
                throw new Exception("Not valid user! (this should never happen)");

            var pollGame = _ctx.PollGames.FirstOrDefault(pg => pg.Id == currentUser.PollGameId);
            if (pollGame == null)
                throw new Exception("Can't find poll game! (this should never happen)");

            


            var question = _ctx.Questions.FirstOrDefault(q => q.PollGame.Id == pollGame.Id && q.QuestionOrder == pollGame.CurrentQuestionOrder);
            if (question == null)
                throw new Exception("Can't find question! (this should never happen)");

            var questionAlternatives = _ctx.QuestionAlternatives.Where(qa => qa.Question.Id == question.Id).ToList();
            if (questionAlternatives.Count < 1)
                throw new Exception("Can't find question alternatives! (this should never happen)");

            var questionToClientDTO = new QuestionToClientDTO()
            {
                Id = question.Id,
                Title = question.Title,
                HasStarted = pollGame.HasStarted,
                InviteCode = pollGame.InviteCode,
                IsAdmin = currentUser.isAdmin,
                TempUsers = pollGame.TempUsers.Select(tu => new TempUserDTO() { Id = tu.Id, DisplayName = tu.DisplayName}).ToList(),
                AdminIsParticipating = pollGame.AdminIsParticipating,
                QuestionAlternatives = questionAlternatives.Select(a => new QuestionAlternativeDTO() { Id = a.Id, AlternativeText = a.AlternativeText}).ToList(),
            };
            return questionToClientDTO;
        }
    }
}
