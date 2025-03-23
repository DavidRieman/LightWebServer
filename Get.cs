﻿using System;
using System.Net.Sockets;
using System.Text;

internal static class Get
{
    private static readonly byte[] indexBytes, indexHeaderBytes;

    static Get()
    {
        indexBytes = Encoding.UTF8.GetBytes(EmbeddedResource.Read(".index.html"));
        indexHeaderBytes = Encoding.ASCII.GetBytes($"HTTP/1.1 200 OK\r\nContent-Type: text/html; charset=\"utf-8\"\r\nContent-Length: {indexBytes.Length}\r\n\n");
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

        if (part.StartsWith("/img/"))
        {
            // Get the filename from the string after the last slash, if any.
            var fileName = part.Substring(lastSlash + 1);

            var fullFilePath = Path.Combine(Configuration.BaseImageDir, fileName);
            if (!File.Exists(fullFilePath))
                return; // TODO: 404?

            // TODO: In-memory cache for a few most recently returned files?
            byte[] file_content = File.ReadAllBytes(fullFilePath);
            string mime_type = "image/jpeg"; // TODO: Base from supported extensions. (jpg, png, gif, webp, etc.)
            StringBuilder header = new();
            header.Append("HTTP/1.1 200 OK\r\n");
            header.Append($"Content-Type: {mime_type}\r\n");
            header.Append($"Content-Length: {file_content.Length}\r\n\n");
            byte[] fileHeaderBytes = Encoding.ASCII.GetBytes(header.ToString());

            stream.Write(fileHeaderBytes, 0, fileHeaderBytes.Length);
            stream.Write(file_content, 0, file_content.Length);
            stream.Flush();
            Console.WriteLine("Served Img: " + part);
        }
    }
}
