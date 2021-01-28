using Microsoft.AspNetCore.SignalR;                                         
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestChat.Api.SignalR
{
    public class ChatHub : Hub                                              
    {
        public Task SendMessage(string user, string message)               
        {
            return Clients.All.SendAsync("ReceiveMessage", user, message);   
        }
    }
}
