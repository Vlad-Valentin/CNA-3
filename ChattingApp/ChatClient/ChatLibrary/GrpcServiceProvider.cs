using Grpc.Net.Client;
using ChatProtos;
using System;

namespace ChatLibrary
{
    public class GrpcServiceProvider
    {
        private string Url { get; set; }
        private Lazy<GrpcChannel> RpcChannel { get; set; }
        private Greeter.GreeterClient ChatClient { get; set; }

        public GrpcServiceProvider()
        {
            this.Url = "https://localhost:5001";
            this.RpcChannel = new Lazy<GrpcChannel>(GrpcChannel.ForAddress(this.Url));
        }

        public Greeter.GreeterClient GetGreeterClient() => this.ChatClient ??= new Greeter.GreeterClient(this.RpcChannel.Value);
    }
}