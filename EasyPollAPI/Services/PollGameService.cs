using EasyPollAPI.DTO;
using EasyPollAPI.Hubs;
using EasyPollAPI.Models;
using EasyPollAPI.Scripts;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace EasyPollAPI.Services
{
    public class PollGameService
    {
        private readonly EasyPollContext _ctx;
        private readonly IHubContext<PollGameHub> _hubContext;
        public PollGameService(EasyPollContext easyPollContext, IHubContext<PollGameHub> hubContext)
        {
            _ctx = easyPollContext;
            _hubContext = hubContext;
        }
        public async Task<TempUserDTO> CreateNewPollGame(PollGameDTO pollGameDTO)
        {
            var status = _ctx.PollGameStatusTypes.FirstOrDefault(pgst => pgst.Type == Constant.Constants.NotStarted);
            if (status == null)
                throw new Exception("Can't find status type! (this should never happen)");

            var newPollGame = new PollGame()
            {
                Status = status,
                AdminIsParticipating = pollGameDTO.AdminIsParticipating,
                CurrentQuestionOrder = 0,
                InviteCode = PollGameUtils.GenerateInviteCode(6),
            };
            await _ctx.PollGames.AddAsync(newPollGame);
            await _ctx.SaveChangesAsync();

            var newTempUser = new TempUser()
            {

                AccessToken = TempUserUtils.GenerateAccessToken(),
                DisplayName = pollGameDTO.AdminUser.DisplayName,
                isAdmin = true,
            };
            newTempUser.PollGame = newPollGame;
            await _ctx.TempUsers.AddAsync(newTempUser);
            await _ctx.SaveChangesAsync();

            foreach (var question in pollGameDTO.Questions.Select((value, i) => new { i, value }))
            {
                var newQuestion = new Question()
                {
                    Title = question.value.Title,
                    QuestionOrder = question.i,
                    PollGame = newPollGame,
                };
                await _ctx.Questions.AddAsync(newQuestion);
                await _ctx.SaveChangesAsync();

                foreach (var alternative in question.value.QuestionAlternatives)
                {
                    var newQuestionAlternative = new QuestionAlternative()
                    {
                        AlternativeText = alternative.AlternativeText,
                        Question = newQuestion,
                    };

                    await _ctx.QuestionAlternatives.AddAsync(newQuestionAlternative);
                    await _ctx.SaveChangesAsync();
                }
            }

            return new TempUserDTO() { AccessToken = newTempUser.AccessToken, DisplayName = newTempUser.DisplayName, isAdmin = newTempUser.isAdmin };
        }


        public async Task AuthenticateInviteCode(string inviteCode)
        {
            var pollGame = _ctx.PollGames.FirstOrDefault(pg => pg.InviteCode == inviteCode);
            if (pollGame == null)
                throw new Exception("Invalid invite code");
            return;
        }

        public async Task<PollGameDataToClientDTO> GetGameDataByGameId(int pollGameId)
        {
            var pollGame = _ctx.PollGames.FirstOrDefault(pg => pg.Id == pollGameId);
            if (pollGame == null)
                throw new Exception("Can't find poll game! (this should never happen)");

            var status = _ctx.PollGameStatusTypes.FirstOrDefault(pgst => pgst.Id == pollGame.StatusId);
            if (status == null)
                throw new Exception("Can't find status type! (this should never happen)");

            var tempUsers = _ctx.TempUsers.Where(tu => tu.PollGameId == pollGame.Id).Select(tu => new TempUserDTO() { Id = tu.Id, DisplayName = tu.DisplayName, isAdmin = tu.isAdmin }).ToList();
            if (tempUsers == null || tempUsers.Count < 1)
                throw new Exception("Can't find users! (this should never happen)");

            var admin = tempUsers.FirstOrDefault(tu => tu.isAdmin == true);
            if (admin == null)
                throw new Exception("Can't find admin! (this should never happen)");

            var adminTempUser = new TempUserDTO() { Id = admin.Id, DisplayName = admin.DisplayName };

            var question = _ctx.Questions.FirstOrDefault(q => q.PollGame.Id == pollGame.Id && q.QuestionOrder == pollGame.CurrentQuestionOrder);
            if (question == null)
                throw new Exception("Can't find question! (this should never happen)");

            var questionAlternatives = _ctx.QuestionAlternatives.Where(qa => qa.Question.Id == question.Id).Select(qa => new QuestionAlternativeDTO() { Id = qa.Id, AlternativeText = qa.AlternativeText }).ToList();
            if (questionAlternatives.Count < 1)
                throw new Exception("Can't find question alternatives! (this should never happen)");

            var questionDTO = new QuestionDTO() { Id = question.Id, Title = question.Title, QuestionOrder = question.QuestionOrder, QuestionAlternatives = questionAlternatives };

            var questionAlternativeIds = questionAlternatives.Select(qa => qa.Id);

            var numberOfAnswers = _ctx.UserAnswers.Where(ua => questionAlternativeIds.Contains(ua.QuestionAlternativeId)).Count();

            var pollGameDataToClientDTO = new PollGameDataToClientDTO()
            {
                Id = question.Id,
                Status = status.Type,
                InviteCode = pollGame.InviteCode,
                Admin = adminTempUser,
                TempUsers = tempUsers,
                NumberOfAnswers = numberOfAnswers,
                Question = questionDTO,
                AdminIsParticipating = pollGame.AdminIsParticipating,
            };
            return pollGameDataToClientDTO;

        }

        public async Task<PollGameResultToClientDTO> GetGameResultByGameId(int pollGameId)
        {
            var pollGame = _ctx.PollGames.FirstOrDefault(pg => pg.Id == pollGameId);
            if (pollGame == null)
                throw new Exception("Can't find poll game! (this should never happen)");

           

            var questions = _ctx.Questions.Where(q => q.PollGameId == pollGameId);
            var questionDTOs = new List<QuestionDTO>();
            foreach(var question in questions)
            {
                var questionDTO = new QuestionDTO() { Title = question.Title };
                var alternatives = _ctx.QuestionAlternatives.Where(qa => qa.Id == question.Id);
                foreach(var alternative in alternatives)
                {
                    var questionAlternativeDTO = new QuestionAlternativeDTO() { AlternativeText = alternative.AlternativeText };
                    var userAnswers = _ctx.UserAnswers.Where(ua => ua.QuestionAlternativeId == alternative.Id).ToList();
                    questionAlternativeDTO.usersAnswered = userAnswers.Select(ua => ua.Id).ToList();
                    questionDTO.QuestionAlternatives.Add(questionAlternativeDTO);
                }
                questionDTOs.Add(questionDTO);
            };

            var tempUsers = _ctx.TempUsers.Where(tu => tu.PollGameId == pollGame.Id).Select(tu => new TempUserDTO(){ Id = tu.Id, DisplayName = tu.DisplayName, isAdmin = tu.isAdmin}).ToList();
            var pollGameResultToClientDTO = new PollGameResultToClientDTO()
            {
                Status = Constant.Constants.Ended,
                Questions = questionDTOs,
                TempUsers = tempUsers,
            };
            return pollGameResultToClientDTO;
        }

        public async Task UpdateClientsWithGameData(int pollGameId)
        {
            var pollGame = _ctx.PollGames.FirstOrDefault(pg => pg.Id == pollGameId);
            if (pollGame == null)
                throw new Exception("Can't find poll game! (this should never happen)");

            var status = _ctx.PollGameStatusTypes.FirstOrDefault(pgst => pgst.Id == pollGame.StatusId);
            if (status == null)
                throw new Exception("Can't find status type! (this should never happen)");

            var connectionString = "Socket-PollGameId-" + pollGameId;
            if (status.Type != Constant.Constants.Ended)
            {
                PollGameDataToClientDTO pollGameDataToClientDTO = await GetGameDataByGameId(pollGameId);
                await _hubContext.Clients.All.SendAsync(connectionString, pollGameDataToClientDTO);
            } else
            {
                PollGameResultToClientDTO pollGameDataToClientDTO = await GetGameResultByGameId(pollGameId);
                await _hubContext.Clients.All.SendAsync(connectionString, pollGameDataToClientDTO);
            }
            
            
            
        }

        public async Task EndPoll(int pollGameId)
        {
            var endedStatus = await _ctx.PollGameStatusTypes.FirstOrDefaultAsync(pgst => pgst.Type == Constant.Constants.Ended);
            if (endedStatus == null)
                throw new Exception("Can't find ended status type! (this should never happen)");

            var pollGame = await _ctx.PollGames.FirstOrDefaultAsync(pg => pg.Id == pollGameId);
            if (pollGame == null)
                throw new Exception("Can't find poll game! (this should never happen)");

            pollGame.Status = endedStatus;
            await _ctx.SaveChangesAsync();
            UpdateClientsWithGameData(pollGameId);
        }

        public async Task StartPollGame(string accessToken)
        {
            var user = _ctx.TempUsers.FirstOrDefault(tu => tu.AccessToken == accessToken);
            if (user == null)
                throw new Exception("Invalid accesstoken");
            if (!user.isAdmin)
                throw new Exception("You do not have access to start poll");

            var pollGame = _ctx.PollGames.FirstOrDefault(pg => pg.Id == user.PollGameId);
            if (pollGame == null)
                throw new Exception("Can't find poll game! (this should never happen)");

            var status = _ctx.PollGameStatusTypes.FirstOrDefault(pgst => pgst.Type == Constant.Constants.Started);
            if (status == null)
                throw new Exception("Can't find status type! (this should never happen)");

            pollGame.Status = status;
            await _ctx.SaveChangesAsync();
            await UpdateClientsWithGameData(pollGame.Id);

        }
    }
}
