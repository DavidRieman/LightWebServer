using System;

internal class Configuration
{
    // This could read a configuration file (like an App.config) but to get started, for simplicity, we'll
    // just hard-code the configuration properties here. At least this will still provide a consolidation
    // of highly-system-specific values in one place, to make it easier to avoid committing specific system
    // details into source control.
    public static string BaseImageDir { get; } = "D:\\Img\\";
}
