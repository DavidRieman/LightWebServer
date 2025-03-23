using System;
using System.Reflection;

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
}
