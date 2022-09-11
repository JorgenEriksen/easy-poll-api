using Microsoft.AspNetCore.SignalR;

namespace EasyPollAPI.Hubs
{
    public class PollGameHub : Hub
    {
        public async Task SendMessage(string user, string message)
        => await Clients.All.SendAsync("ReceiveMessage", user, message);
    }
}
