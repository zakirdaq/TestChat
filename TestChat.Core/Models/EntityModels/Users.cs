﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace TestChat.Core.Models.EntityModels
{
    public partial class Users
    {
        public Users()
        {
            UserChatsReciever = new HashSet<UserChats>();
            UserChatsSender = new HashSet<UserChats>();
        }

        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool? Status { get; set; }

        public virtual ICollection<UserChats> UserChatsReciever { get; set; }
        public virtual ICollection<UserChats> UserChatsSender { get; set; }
    }
}