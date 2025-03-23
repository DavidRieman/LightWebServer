using System;
using System.Net.Sockets;
using System.Text;

internal class PutOrPost
{
    // Open a file for continuous logging.
    private static readonly StreamWriter log = new("log.txt", append: true);

    private static readonly byte[] standardResponse = Encoding.ASCII.GetBytes("HTTP/1.1 200 OK\r\nContent-Length: 0\r\n\n");

    internal static void Handle(NetworkStream stream, string part)
    {
        // Simulating a restful API: Simply logging the requests.
        // (Imagine this was requests to sort a file, mark metadata for a file, etc.)
        var cleansedPart = !part.Contains(" HTTP/") ? part : part.Substring(0, part.LastIndexOf(" HTTP/"));
        Console.WriteLine(cleansedPart);
        log.WriteLine(cleansedPart);
        log.Flush();
        stream.Write(standardResponse);
        stream.Flush();
    }
}
