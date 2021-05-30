using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatService.Models
{
    public class ChatMessage
    {
        public string Text { get; set; }
        public string Sender { get; set; }

        public ChatMessage(string text, string sender)
        {
            Text = text;
            Sender = sender;
        }
    }
}
