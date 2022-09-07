using EasyPollAPI.DTO;
using EasyPollAPI.Models;
using EasyPollAPI.Scripts;

namespace EasyPollAPI.Services
{
    public class PollGameService
    {
        private readonly EasyPollContext _ctx;
        public PollGameService(EasyPollContext easyPollContext)
        {
            _ctx = easyPollContext;
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
    }
}
