using Google.Protobuf;
using Greet;
using Grpc.Net.Client;

namespace Client
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            for (var i = 0; i < 2; i++)
            {
                Console.WriteLine($"Call {i}");
                await MakeCall();
            }
        }

        private static async Task MakeCall()
        {
            var channel = GrpcChannel.ForAddress("http://127.0.0.1:5246", new GrpcChannelOptions
            {
                MaxReceiveMessageSize = null
            });
            var client = new Greeter.GreeterClient(channel);
            var call = client.SayHello();
            for (var i = 0; i < 150; i++)
            {
                var request = new HelloRequest
                {
                    Content = RandomBytes(1024 * 1024)
                };
                Console.WriteLine($"Sending {request.Content.Length} bytes");
                await call.RequestStream.WriteAsync(request);
            }
            await call.RequestStream.CompleteAsync();

            var response = await call.ResponseAsync;
            Console.WriteLine($"Received {response.Content.Length} bytes");
        }

        private static ByteString RandomBytes(int size)
        {
            var bytes = new byte[size];
            Random.Shared.NextBytes(bytes);
            return ByteString.CopyFrom(bytes);
        }
    }
}