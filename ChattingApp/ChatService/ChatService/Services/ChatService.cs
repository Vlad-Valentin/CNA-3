using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

            return Task.FromResult(m_empty);
        }
    }
}
