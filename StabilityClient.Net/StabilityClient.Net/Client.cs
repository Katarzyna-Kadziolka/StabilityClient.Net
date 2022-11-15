using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;

namespace StabilityClient.Net;

public class Client  {
    public Client() {
        using var channel = GrpcChannel.ForAddress("https://localhost:7042");
    }
}