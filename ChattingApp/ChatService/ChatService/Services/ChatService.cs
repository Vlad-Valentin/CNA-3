using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatService.Utility;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Core.Logging;
using Microsoft.Extensions.Logging;

namespace ChatService.Services
{
    public class ChatService : MessengerService.MessengerServiceBase
    {
        private readonly ILogger<ChatService> _logger;
        private readonly Empty m_empty = new Empty();

        public ChatService(ILogger<ChatService> logger)
        {
            _logger = logger;
        }

        public override Task<Empty> SendMessage(SendRequest request, ServerCallContext context)
        {
            Console.WriteLine("Got message from " + request.Message.User.Name);

            ChatBase.WriteToList(request.Message);

            return Task.FromResult(m_empty);
        }

        public override Task<DisplayMessageResponse> DisplayMessage(Empty request, ServerCallContext context)
        {
            DisplayMessageResponse displayMessageResponse = new DisplayMessageResponse();
            displayMessageResponse.ReturnedMessages.AddRange(ChatBase.ChatLog);
            return Task.FromResult(displayMessageResponse);
        }
    }
}
