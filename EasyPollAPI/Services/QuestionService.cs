using EasyPollAPI.DTO;
using EasyPollAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EasyPollAPI.Services
{
    public class QuestionService
    {

        private readonly EasyPollContext _ctx;
        private readonly PollGameService _pollGameService;
        public QuestionService(EasyPollContext easyPollContext, PollGameService pollGameService)
        {
            _ctx = easyPollContext;
            _pollGameService = pollGameService;
        }

        public async Task SubmitQuestion(string accessToken, SubmitQuestionDTO submitQuestionDTO)
        {
            var tempUser = _ctx.TempUsers.FirstOrDefault(tu => tu.AccessToken == accessToken);
            if (tempUser == null)
                throw new Exception("Can't find user! (this should never happen)");

            var pollGame = _ctx.PollGames.FirstOrDefault(pg => pg.Id == tempUser.PollGameId);
            if (pollGame == null)
                throw new Exception("Can't find poll game! (this should never happen)");

            var question = _ctx.Questions.FirstOrDefault(q => q.PollGame.Id == pollGame.Id && q.QuestionOrder == pollGame.CurrentQuestionOrder);
            if (question == null)
                throw new Exception("Can't find question! (this should never happen)");

            var questionAlternatives = _ctx.QuestionAlternatives.Where(qa => qa.Question.Id == question.Id).Select(qa => new QuestionAlternativeDTO() { Id = qa.Id, AlternativeText = qa.AlternativeText }).ToList();
            if (questionAlternatives.Count < 1)
                throw new Exception("Can't find question alternatives! (this should never happen)");

            var questionAlternativesId = questionAlternatives.Select(qa => qa.Id).ToList();

            var userAnswer = _ctx.UserAnswers.FirstOrDefault(ua => ua.TempUser.Id == tempUser.Id && questionAlternativesId.Contains(ua.QuestionAlternativeId));

            // if user has already answered
            if (userAnswer != null)
            {
                _ctx.UserAnswers.Remove(userAnswer);
                await _ctx.SaveChangesAsync();
            }
                

            var alternative = _ctx.QuestionAlternatives.FirstOrDefault(qa => qa.Id == submitQuestionDTO.AlternativeId);
            if (alternative == null)
                throw new Exception("Can't find question alternatives!");

            var newUserAnswer = new UserAnswer()
            {
                TempUser = tempUser,
                QuestionAlternative = alternative,

            };
            _ctx.UserAnswers.Add(newUserAnswer);
            await _ctx.SaveChangesAsync();
            await CheckIfAllUsersHasAnswered(pollGame.Id);
            _pollGameService.UpdateClientsWithGameData(pollGame.Id);


        }

        public async Task CheckIfAllUsersHasAnswered(int pollGameId)
        {
            var pollGame = _ctx.PollGames.FirstOrDefault(pg => pg.Id == pollGameId);
            var currentQuestion = _ctx.Questions.FirstOrDefault(q => q.PollGame.Id == pollGame.Id && q.QuestionOrder == pollGame.CurrentQuestionOrder);
            var questionAlternativesId = _ctx.QuestionAlternatives.Where(qa => qa.QuestionId == currentQuestion.Id).Select(qa => qa.Id);
            var userAnswers = _ctx.UserAnswers.Where(ua => questionAlternativesId.Contains(ua.QuestionAlternativeId));

            var numberOfUsers = _ctx.TempUsers.Where(tu => tu.PollGameId == pollGame.Id).Count();

            if (userAnswers.Count() == numberOfUsers)
                await NextQuestion(pollGameId);
  
        }

        public async Task NextQuestion(int pollGameId)
        {
            var pollGame = _ctx.PollGames.FirstOrDefault(pg => pg.Id == pollGameId);
            var questions = _ctx.Questions.Where(q => q.PollGameId == pollGameId).ToList();
            var currentQuestion = _ctx.Questions.FirstOrDefault(q => q.PollGame.Id == pollGame.Id && q.QuestionOrder == pollGame.CurrentQuestionOrder);

            // if on last question, end poll.
            if (pollGame.CurrentQuestionOrder >= questions.Count - 1)
            {
                var endedStatus = await _ctx.PollGameStatusTypes.FirstOrDefaultAsync(pgst => pgst.Type == Constant.Constants.Ended);
                if (endedStatus == null)
                    throw new Exception("Can't find ended status type! (this should never happen)");
                pollGame.Status = endedStatus;
                await _ctx.SaveChangesAsync();
                return;
            }
            pollGame.CurrentQuestionOrder += 1;
            await _ctx.SaveChangesAsync();
        }



        // usertoken is valid here
        /*
        public async Task<QuestionToClientDTO> GetQuestionByUserToken(string accessToken)
        {
            var currentUser = _ctx.TempUsers.FirstOrDefault(tu => tu.AccessToken == accessToken);
            if (currentUser == null)
                throw new Exception("Not valid user! (this should never happen)");

            var pollGame = _ctx.PollGames.FirstOrDefault(pg => pg.Id == currentUser.PollGameId);
            if (pollGame == null)
                throw new Exception("Can't find poll game! (this should never happen)");

            var tempUsers = _ctx.TempUsers.Where(tu => tu.PollGameId == pollGame.Id).Select(tu => new TempUserDTO() { Id = tu.Id, DisplayName = tu.DisplayName }).ToList();
            if (tempUsers == null || tempUsers.Count < 1)
                throw new Exception("Can't find users! (this should never happen)");

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
                TempUsers = tempUsers,
                AdminIsParticipating = pollGame.AdminIsParticipating,
                QuestionAlternatives = questionAlternatives.Select(a => new QuestionAlternativeDTO() { Id = a.Id, AlternativeText = a.AlternativeText}).ToList(),
            };
            return questionToClientDTO;
        }
        */
    }
}
