using EasyPollAPI.DTO;
using EasyPollAPI.Hubs;
using EasyPollAPI.Models;
using EasyPollAPI.Scripts;
using Microsoft.AspNetCore.SignalR;

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

            var newPollGame = new PollGame()
            {
                HasStarted = pollGameDTO.HasStarted,
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

            var pollGameDataToClientDTO = new PollGameDataToClientDTO()
            {
                Id = question.Id,
                HasStarted = pollGame.HasStarted,
                InviteCode = pollGame.InviteCode,
                Admin = adminTempUser,
                TempUsers = tempUsers,
                Question = questionDTO,
                AdminIsParticipating = pollGame.AdminIsParticipating,
            };
            return pollGameDataToClientDTO;

        }

        public async Task UpdateClientsWithGameData(int pollGameId)
        {
            PollGameDataToClientDTO pollGameDataToClientDTO = await GetGameDataByGameId(pollGameId);
            var connectionString = "Socket-PollGameId-" + pollGameDataToClientDTO.Id;
            await _hubContext.Clients.All.SendAsync(connectionString, pollGameDataToClientDTO);
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

            pollGame.HasStarted = true;
            await _ctx.SaveChangesAsync();
            await UpdateClientsWithGameData(pollGame.Id);

        }
    }
}
