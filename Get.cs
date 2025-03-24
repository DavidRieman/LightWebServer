using System;
using System.Net.Sockets;
using System.Text;

internal static class Get
{
    private static readonly byte[] favIconBytes, favIconHeaderBytes, favIconIcoBytes, favIconIcoHeaderBytes;
    private static readonly byte[] fileNotBoundBytes, indexBytes, indexHeaderBytes;

    private static readonly Dictionary<string, string> imageContentTypeMap = new()
    {
        { "gif", "image/gif" },
        { "ico", "image/x-icon" },
        { "jpg", "image/jpeg" },
        { "jpeg", "image/jpeg" },
        { "png", "image/png" },
        { "tiff", "image/tiff" },
        { "webp", "image/webp" },
    };

    static Get()
    {
        // Keep in-memory response-stream-ready data for our most commonly served static resources and responses.
        indexBytes = Encoding.UTF8.GetBytes(EmbeddedResource.Read(".index.html"));
        indexHeaderBytes = Encoding.ASCII.GetBytes($"HTTP/1.1 200 OK\r\nContent-Type: text/html; charset=\"utf-8\"\r\nContent-Length: {indexBytes.Length}\r\n\n");
        favIconBytes = EmbeddedResource.ReadFile(".favicon.png");
        favIconHeaderBytes = Encoding.ASCII.GetBytes($"HTTP/1.1 200 OK\r\nContent-Type: image/png\r\nContent-Length: {favIconBytes.Length}\r\n\n");
        favIconIcoBytes = EmbeddedResource.ReadFile(".favicon.ico");
        favIconIcoHeaderBytes = Encoding.ASCII.GetBytes($"HTTP/1.1 200 OK\r\nContent-Type: image/x-icon\r\nContent-Length: {favIconIcoBytes.Length}\r\n\n");
        fileNotBoundBytes = Encoding.ASCII.GetBytes("HTTP/1.1 404 Not Found\r\nContent-Length: 0\r\n\n");
    }

    internal static void Handle(NetworkStream stream, string part)
    {
        int lastSlash = part.LastIndexOf('/');
        if (lastSlash == -1)
            return;

        if (part.StartsWith("/page"))
        {
            stream.Write(indexHeaderBytes, 0, indexHeaderBytes.Length);
            stream.Write(indexBytes, 0, indexBytes.Length);
            stream.Flush();
            Console.WriteLine("Served Page: " + part);
        }
        else if (part.StartsWith("/img/"))
        {
            // Get the filename from the string after the last slash, if any.
            var fileName = part.Substring(5);

            var fullFilePath = Path.Combine(Configuration.BaseImageDir, fileName);
            if (!File.Exists(fullFilePath))
            {
                stream.Write(fileNotBoundBytes);
                stream.Flush();
                return;
            }

            string ext = Path.GetExtension(fileName).TrimStart('.') ?? "";
            string contentType = imageContentTypeMap.TryGetValue(ext, out string? value) ? value : "image/jpeg";

            // TODO: In-memory cache for a few most recently returned files?
            byte[] fileContents = File.ReadAllBytes(fullFilePath);
            StringBuilder header = new();
            header.Append("HTTP/1.1 200 OK\r\n");
            header.Append($"Content-Type: {contentType}\r\n");
            header.Append($"Content-Length: {fileContents.Length}\r\n\n");
            byte[] fileHeaderBytes = Encoding.ASCII.GetBytes(header.ToString());

            stream.Write(fileHeaderBytes, 0, fileHeaderBytes.Length);
            stream.Write(fileContents, 0, fileContents.Length);
            stream.Flush();
            Console.WriteLine("Served Img: " + part);
        }
        else if (part.StartsWith("/favicon.png") || part.StartsWith("/apple-touch-icon.png"))
        {
            stream.Write(favIconHeaderBytes, 0, favIconHeaderBytes.Length);
            stream.Write(favIconBytes, 0, favIconBytes.Length);
            stream.Flush();
            Console.WriteLine("Served FavIcon for: " + part);
        }
        else if (part.StartsWith("/favicon.ico"))
        {
            stream.Write(favIconIcoHeaderBytes, 0, favIconIcoHeaderBytes.Length);
            stream.Write(favIconIcoBytes, 0, favIconIcoBytes.Length);
            stream.Flush();
            Console.WriteLine("Served FavIcon for: " + part);
        }
        else
        {
            // Any other resource or pattern that we're not handling yet...
            stream.Write(fileNotBoundBytes);
            stream.Flush();
            Console.WriteLine("Ignored request with 404: " + part);
        }
    }
}
