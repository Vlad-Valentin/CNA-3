using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatService.Utility
{
    public static class ChatBase
    {
        public static List<string> ChatLog { get; set; }

        static ChatBase()
        {
            ChatLog = new List<string>();
        }
        
        public static void WriteToList(ChatService.Message message)
        {
            StringBuilder stringBuilder = new();
            stringBuilder.Append(message.User.Name).Append(": ").Append(message.Text);
            ChatLog.Add(stringBuilder.ToString());
        }
    }
}
