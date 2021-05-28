using System.Collections.Generic;
using System.Text;

namespace ChatService.Utility
{
    public static class ChatBase
    {
        public static List<string> ChatLog { get; set; }
        public static List<User> UserList { get; set; }

        static ChatBase()
        {
            ChatLog = new List<string>();
            UserList = new List<User>();
        }

        public static void WriteToList(Message message)
        {
            StringBuilder stringBuilder = new();
            stringBuilder.Append(message.User.Name).Append(": ").Append(message.Text);
            ChatLog.Add(stringBuilder.ToString());
        }

        public static void WriteToUserList(User user)
        {
            UserList.Add(user);
        }
    }
}