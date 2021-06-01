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

        public ChatService(ILogger<ChatService> Logger)
        {
            _logger = Logger;
        }

        public override async Task GetMessages(GetMessagesRequest request, IServerStreamWriter<GetMessagesResponse> responseStream, ServerCallContext context)
        {
            _logger.Log(LogLevel.Information, $"Call to {nameof(GetMessages)} from {context.Host}");
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
            _logger.Log(LogLevel.Information,$"{context.Host} sent message: {request.From}: {request.Text}");
            ChatMessage message = new(request.Text, request.From);
            ChatBase.WriteToMessageList(message);
            return Task.FromResult(new SendResponse());
        }


        public override async Task GetUsers(GetUserRequest request, IServerStreamWriter<GetUsersResponse> responseStream, ServerCallContext context)
        {
            _logger.Log(LogLevel.Information,$"{nameof(GetUsers)} called from {context.Host}");
            var userLog = ChatBase.GetAllUsers();
            foreach(var user in userLog)
            {
                await responseStream.WriteAsync(new GetUsersResponse()
                {
                    Username = user
                });
            }
        }

        public override Task<SendUserResponse> SendUser(SendUserRequest request, ServerCallContext context)
        {
            _logger.Log(LogLevel.Information,$"{context.Host} connected with a new user: {request.Username}");
            string userName = request.Username;
            ChatBase.WriteToUserList(userName);
            return Task.FromResult(new SendUserResponse());
        }

        public override async Task GetDisconnectedUsers(GetDisconnectedUsersRequest request, IServerStreamWriter<GetDisconnectedUsersResponse> responseStream, ServerCallContext context)
        {
            _logger.Log(LogLevel.Information,$"{nameof(GetDisconnectedUsers)} called from {context.Host}");
            var disconnectedUsersLog = ChatBase.GetAllDisconnectedUsers();
            foreach(var user in disconnectedUsersLog)
            {
                await responseStream.WriteAsync(new GetDisconnectedUsersResponse()
                {
                    Username = user
                });
            }
        }

        public override Task<SendDisconnectedResponse> SendDisconnectedUser(SendDisconnectedRequest request, ServerCallContext context)
        {
            _logger.Log(LogLevel.Information, $"{context.Host} disconnected user: {request.Username}");
            string username = request.Username;
            ChatBase.WriteToDisconnectedUserList(username);
            return Task.FromResult(new SendDisconnectedResponse());
        }
    }
}
