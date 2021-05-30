namespace ChatClient.Models
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

        public override string ToString()
        {
            return $"{Sender}: {Text}\n";
        }
    }
}