using Google.Protobuf;
using Greet;
using Grpc.Core;

namespace Server.Services
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;
        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }

        public override async Task<HelloReply> SayHello(IAsyncStreamReader<HelloRequest> requestStream, ServerCallContext context)
        {
            await foreach (var request in requestStream.ReadAllAsync())
            {
                _logger.LogInformation($"Received message");
            }

            _logger.LogInformation("Received all messages");

            return new HelloReply
            {
                Content = RandomBytes(100 * 1024 * 1024)
            };
        }

        private ByteString RandomBytes(int size)
        {
            var bytes = new byte[size];
            Random.Shared.NextBytes(bytes);
            return ByteString.CopyFrom(bytes);
        }
    }
}