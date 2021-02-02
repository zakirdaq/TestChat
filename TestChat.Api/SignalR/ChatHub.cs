using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;                                         
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestChat.Api.SignalR
{
    public class ChatHub : Hub                                              
    {
        public async Task SendChatMessage(string who, string message)
        {
            await Clients.Group(who).SendAsync(message);
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            if (httpContext != null)
            {
                try
                {
                    var groupName = httpContext.Request.Query["groupName"].ToString();

                    await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
                }
                catch (Exception) { }
            }

        }
    }
}
