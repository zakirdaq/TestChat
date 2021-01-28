using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestChat.Core.Models.DomainModels
{
    public class ChatModel
    {
        public Guid RecieverId { get; set; }
        public string Message { get; set; }
    }
}
