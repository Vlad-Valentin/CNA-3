using Grpc.Net.Client;
using ChatProtos;
using System;

namespace ChatLibrary
{
    public class GrpcServiceProvider
    {
        private string Url { get; set; }
        private Lazy<GrpcChannel> RpcChannel { get; set; }
        private ChatService.MessengerService.MessengerServiceClient ChatClient { get; set; }

        public GrpcServiceProvider()
        {
            this.Url = "https://localhost:5001";
            this.RpcChannel = new Lazy<GrpcChannel>(GrpcChannel.ForAddress(this.Url));
        }

        public ChatService.MessengerService.MessengerServiceClient GetMessengerClient() => 
            this.ChatClient ??= new ChatService.MessengerService.MessengerServiceClient(this.RpcChannel.Value);
    }
}