using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace DemoWebApp.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        public ChatHub()
        {
        }

        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }

        public async Task Join(string chatId)
        {

            await Groups.AddToGroupAsync(Context.ConnectionId, chatId);
        }

        public async Task Leave(string chatId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId);
        }

        public async Task SendMessage(string chatId, string message)
        {

            var user = Context.User.Identity.Name;
            await Clients.Group(chatId).SendAsync("Message", new { user, message });

        }
    }
}
