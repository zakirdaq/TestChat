﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace TestChat.Core.Models.EntityModels
{
    public partial class UserChats
    {
        public Guid Id { get; set; }
        public Guid SenderId { get; set; }
        public Guid RecieverId { get; set; }
        public string Message { get; set; }
        public DateTime MessageTime { get; set; }
        public bool? ReadStatus { get; set; }

        public virtual Users Reciever { get; set; }
        public virtual Users Sender { get; set; }
    }
}