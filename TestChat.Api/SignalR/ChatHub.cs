using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;                                         
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestChat.Api.SignalR
{
    [Authorize]
    public class ChatHub : Hub                                              
    {
        public async Task SendChatMessage(string who, string message)
        {
            string name = Context.User.Identity.Name;

            await Clients.Group(who).SendAsync(name + ": " + message);
        }

        public override async Task OnConnectedAsync()
        {
            string groupName = Context.User.Identity.Name;

            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }
    }
}
