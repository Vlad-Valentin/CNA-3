using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatService.Utility;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Logging;
using System.Reactive.Linq;
using ChatService.Models;

namespace ChatService.Services
{
    public class ChatService : MessengerService.MessengerServiceBase
    {
        private readonly ILogger<ChatService> _logger;

        public override async Task GetMessages(GetMessagesRequest request, IServerStreamWriter<GetMessagesResponse> responseStream, ServerCallContext context)
        {
            Console.WriteLine($"Call to {nameof(GetMessages)} from {context.Host}");
            var chatLog = ChatBase.GetAllMessages();
            foreach(var msg in chatLog)
            {
                await responseStream.WriteAsync(new GetMessagesResponse()
                {
                    From = msg.Sender,
                    Text = msg.Text
                });
            }
        }


        public override Task<SendResponse> SendMessage(SendRequest request, ServerCallContext context)
        {
            Console.WriteLine($"{context.Host} sent message: {request.From}: {request.Text}");
            ChatMessage message = new(request.Text, request.From);
            ChatBase.WriteToMessageList(message);
            return Task.FromResult(new SendResponse());
        }
    }
}
