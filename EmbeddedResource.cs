﻿using System.Reflection;

internal class EmbeddedResource
{
    public static string Read(string resourceName)
    {
        var assembly = Assembly.GetExecutingAssembly();
        string foundResourceName = assembly.GetManifestResourceNames().Single(s => s.EndsWith(resourceName));
        using Stream stream = assembly.GetManifestResourceStream(foundResourceName)!;
        using StreamReader reader = new(stream);
        return reader.ReadToEnd();
    }

    public static byte[] ReadFile(string resourceName)
    {
        var assembly = Assembly.GetExecutingAssembly();
        string foundResourceName = assembly.GetManifestResourceNames().Single(s => s.EndsWith(resourceName));
        using Stream stream = assembly.GetManifestResourceStream(foundResourceName)!;
        byte[] bytes = new byte[stream.Length];
        stream.ReadExactly(bytes, 0, bytes.Length);
        return bytes;
    }
}
