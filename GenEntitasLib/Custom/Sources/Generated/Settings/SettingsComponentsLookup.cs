namespace GenEntitas {
public static class SettingsComponentsLookup {

    public const int ConsoleWriteLineGeneratedPaths = 0;
    public const int GeneratedNamespace = 1;
    public const int GeneratePath = 2;
    public const int IgnoreNamespaces = 3;
    public const int ReflectionAssemblyPaths = 4;
    public const int RunInDryMode = 5;

    public const int TotalComponents = 6;

    public static readonly string[] componentNames = {
        "ConsoleWriteLineGeneratedPaths",
        "GeneratedNamespace",
        "GeneratePath",
        "IgnoreNamespaces",
        "ReflectionAssemblyPaths",
        "RunInDryMode"
    };

    public static readonly System.Type[] componentTypes = {
        typeof(GenEntitas.ConsoleWriteLineGeneratedPaths),
        typeof(GenEntitas.GeneratedNamespace),
        typeof(GenEntitas.GeneratePath),
        typeof(GenEntitas.IgnoreNamespaces),
        typeof(GenEntitas.ReflectionAssemblyPaths),
        typeof(GenEntitas.RunInDryMode)
    };
}

}
