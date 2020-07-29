using System;
using System.ComponentModel.DataAnnotations;

namespace BlazorAppPWAAuthHostCore.Client.Domain
{
    public class Message
    {
        private Message()
        {
        }

        public Message(string username, string body, bool mine)
        {
            Id = new Guid();
            Username = username;
            Body = body;
            Mine = mine;
        }

        [Key]
        public Guid Id { get; private set; }
        public string Username { get; private set; }
        public string Body { get; private set; }
        public bool Mine { get; private set; }

        public string CSS => Mine ? "sent" : "received";
    }
}