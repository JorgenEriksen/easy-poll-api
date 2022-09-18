using EasyPollAPI.Models;
using EasyPollAPI.Constant;

namespace EasyPollAPI.DbInitializer
{
    public class DbInitializer
    {
        public EasyPollContext _context;
        public DbInitializer(EasyPollContext ctx)
        {
            _context = ctx;
        }

        public void Seed()
        {

            if (_context.PollGameStatusTypes.Any())
            {
                return;
            }
            var status = new PollGameStatusType()
            {
                Type = Constant.Constants.NotStarted,
            };
            var status2 = new PollGameStatusType()
            {
                Type = Constant.Constants.Started,
            };
            var status3 = new PollGameStatusType()
            {
                Type = Constant.Constants.Ended,
            };

            _context.PollGameStatusTypes.Add(status);
            _context.PollGameStatusTypes.Add(status2);
            _context.PollGameStatusTypes.Add(status3);
            _context.SaveChanges();

        }
    }
}
