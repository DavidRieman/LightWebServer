using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

Console.WriteLine("Starting server at port 32123...");
TcpListener tcpListener = new(IPAddress.Any, 32123);
tcpListener.Start();

const int bufferSize = 512;
byte[] bytes = new byte[bufferSize];

// TODO: Should probably track a List<TcpClient> for connected clients. Basically, each loop:
// * Check for closed clients to cull them.
// * Check for new pending clients to add them.
// * Check each client for new available data.
TcpClient? client = null;

Console.WriteLine("Listening indefinitely for connections. Press 'Q' to quit.");
while (true)
{
    // If the user is currently pressing down the 'Q' key...
    if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Q)
    {
        Console.WriteLine("Shutting down.");
        break; // Break the loop to quit the program.
    }

    // Accept the next incoming request from tcpListener.
    if (tcpListener.Pending())
    {
        client = tcpListener.AcceptTcpClient();
        Console.WriteLine("Connected!");
    }

    if (client == null)
    {
        Thread.Sleep(1);
        continue;
    }

    if (client.Available > 0)
    {
        NetworkStream stream = client.GetStream();
        while (client.Connected && stream.DataAvailable)
        {
            // Technically this process splits the data on the bufferSize boundaries, which could in some
            // extreme edge cases cause us to fail to recognize one of the requests that we care about.
            // However in practice this hasn't been observed, so for simplicity, we'll leave it as is until
            // we have a good test case to ensure we handle it correctly and safely when we really need to.
            if (stream.Read(bytes, 0, bufferSize) > 0)
            {
                string received = new(ASCIIEncoding.UTF8.GetString(bytes));
                var parts = received.Split(['\r', '\n']);
                foreach (var part in parts)
                {
                    int httpPartIndex = part.LastIndexOf(" HTTP/");
                    if (!part.Contains(" HTTP/"))
                        continue;

                    if (part.StartsWith("OPTIONS "))
                    {
                        Options.Handle(stream);
                    }
                    else if (part.StartsWith("GET "))
                    {
                        Get.Handle(stream, part.Substring(4, httpPartIndex - 4));
                    }
                    else if (part.StartsWith("POST "))
                    {
                        PutOrPost.Handle(stream, part.Substring(5, httpPartIndex - 5));
                    }
                    else if (part.StartsWith("PUT "))
                    {
                        PutOrPost.Handle(stream, part.Substring(4, httpPartIndex - 4));
                    }
                }
            }
        }
    }
    else
    {
        Thread.Sleep(1);
    }
}
