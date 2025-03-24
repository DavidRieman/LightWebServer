using System.Net.Sockets;
using System.Text;

internal class Options
{
    private static readonly byte[] standardOptionsResponse = Encoding.ASCII.GetBytes(
        "HTTP/1.1 204 No Content\r\n" +
        "Access-Control-Allow-Origin: *\r\n" +
        "Access-Control-Allow-Methods: *\r\n" +
        "Vary: Access-Control-Request-Headers\r\n" +
        "Access-Control-Allow-Headers: Content-Type, Accept\r\n" +
        "Content-Length: 0\r\n\n");

    internal static void Handle(NetworkStream stream)
    {
        // Responding to OPTIONS: Trying to help avoid CORS errors...
        // While this seems wrong (probably entirely in strategy still), it may be useful for reference,
        // so the code is left in place for now (but disabled to avoid any extra security issues for now).
        Console.WriteLine("Got OPTIONS request...");
        //stream.Write(standardOptionsResponse);
        //stream.Flush();
    }
}
