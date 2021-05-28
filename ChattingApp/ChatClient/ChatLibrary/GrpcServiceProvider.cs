using Grpc.Net.Client;
using ChatClient;
using System;

namespace ChatLibrary
{
    public class GrpcServiceProvider
    {
        private string Url { get; set; }
        private Lazy<GrpcChannel> RpcChannel { get; set; }
        private MessengerService.MessengerServiceClient ChatClient { get; set; }

        public GrpcServiceProvider()
        {
            this.Url = "https://localhost:5001";
            this.RpcChannel = new Lazy<GrpcChannel>(GrpcChannel.ForAddress(this.Url));
        }

        public MessengerService.MessengerServiceClient GetMessengerClient() => 
            this.ChatClient ??= new MessengerService.MessengerServiceClient(this.RpcChannel.Value);
    }
}