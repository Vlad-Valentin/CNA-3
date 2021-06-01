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
        //private readonly ILogger<ChatService> _logger;

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


        public override async Task GetUsers(GetUserRequest request, IServerStreamWriter<GetUsersResponse> responseStream, ServerCallContext context)
        {
            Console.WriteLine($"{nameof(GetUsers)} called from {context.Host}");
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
            Console.WriteLine($"{context.Host} connected with a new user: {request.Username}");
            string userName = request.Username;
            ChatBase.WriteToUserList(userName);
            return Task.FromResult(new SendUserResponse());
        }

        public override async Task GetDisconnectedUsers(GetDisconnectedUsersRequest request, IServerStreamWriter<GetDisconnectedUsersResponse> responseStream, ServerCallContext context)
        {
            Console.WriteLine($"{nameof(GetDisconnectedUsers)} called from {context.Host}");
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
            Console.WriteLine($"{context.Host} disconnected user: {request.Username}");
            string username = request.Username;
            ChatBase.WriteToDisconnectedUserList(username);
            return Task.FromResult(new SendDisconnectedResponse());
        }
        /* private readonly Empty m_empty = new Empty();

         private ChatServiceHelper serviceHelper = new();

         public ChatService(ILogger<ChatService> logger)
         {
             _logger = logger;
         }

         public override Task<Empty> GetUser(GetUserRequest request, ServerCallContext context)
         {
             User user = new() { Name = request.User.Name, Id = 0 };

             Console.WriteLine($"{user.Name} Logged In");

             ChatBase.WriteToUserList(user);

             return Task.FromResult(m_empty);
         }

         public override Task<SendUserListResponse> SendUserList(Empty request, ServerCallContext context)
         {
             SendUserListResponse sendUserListResponse = new SendUserListResponse();
             sendUserListResponse.Users.AddRange(ChatBase.UserList);

             Console.WriteLine("Sent user list.");
             return Task.FromResult(sendUserListResponse);
         }

         public override Task<Empty> SendMessage(SendRequest request, ServerCallContext context)
         {
             Console.WriteLine("Got message from " + request.Message.User.Name);

             ChatBase.WriteToMessageList(request.Message);

             return Task.FromResult(m_empty);
         }

         public override Task<SendMessageListResponse> SendMessageList(Empty request, ServerCallContext context)
         {
             SendMessageListResponse sendMessageListResponse = new SendMessageListResponse();
             sendMessageListResponse.Messages.AddRange(ChatBase.ChatLog);

             Console.WriteLine("Sent message list.");
             return Task.FromResult(sendMessageListResponse);
         }

         public override Task<Empty> ClientToServer(Message request, ServerCallContext context)
         {
             string name = request.User.Name;
             string text = request.Text;
             ChatBase.ChatLog.Add(request);

             _logger.Log(LogLevel.Information, "\n# " + name + " said: " + text + "\n");

             return Task.FromResult(new Empty());
         }

         public override async Task ServerToClient(Empty request, IServerStreamWriter<Message> responseStream, ServerCallContext context)
         {
             bool open = true;

             try
             {
                 while (open)
                 {
                     if (ChatBase.ChatLog.Last() != ChatBase.LastMessageSent)
                     {
                         var message = new Message()
                         {
                             Text = "\n" + ChatBase.ChatLog.Last()
                         };

                         ChatBase.LastMessageSent = ChatBase.ChatLog.Last();

                         await responseStream.WriteAsync(message);
                     }
                 }
             }
             catch (Exception e)
             {
                 Console.WriteLine("\n" + e.Message + "\n");
             }
         }

         public override Task<Empty> Join(Message message, ServerCallContext context)
         {
             string infoAboutJoining = "\n# " + message.User.Name + " joined the chat." + "\n";

             _logger.Log(LogLevel.Information, infoAboutJoining);

             ChatBase.ChatLog.Add(new Message()
             {
                 Text = message.User.Name + " joined the chat."
             });

             return Task.FromResult(new Empty());
         }

         public override Task<Empty> Leave(Message message, ServerCallContext context)
         {
             string infoAboutLeaving = "\n# " + message.User.Name + " left the chat." + "\n";

             _logger.Log(LogLevel.Information, infoAboutLeaving);

             ChatBase.ChatLog.Add(new Message()
             {
                 Text = message.User.Name + " left the chat."
             });

             return Task.FromResult(new Google.Protobuf.WellKnownTypes.Empty());
         }

         public override async Task Enter(Empty request, IServerStreamWriter<Message> responseStream, ServerCallContext context)
         {
             var connectedClient = context.Peer;
             Console.WriteLine($"{ connectedClient} has connected!");

             try
             {
                 await ChatBase.ChatLog.ForEach.write
             }
         }
     }*/
    }
}
