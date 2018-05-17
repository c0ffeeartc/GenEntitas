public static class SettingsComponentsLookup {

    public const int ConsoleWriteLineGeneratedPaths = 0;
    public const int GeneratePath = 1;
    public const int IgnoreNamespaces = 2;
    public const int ReflectionAssemblyPaths = 3;
    public const int RunInDryMode = 4;

    public const int TotalComponents = 5;

    public static readonly string[] componentNames = {
        "ConsoleWriteLineGeneratedPaths",
        "GeneratePath",
        "IgnoreNamespaces",
        "ReflectionAssemblyPaths",
        "RunInDryMode"
    };

    public static readonly System.Type[] componentTypes = {
        typeof(GenEntitas.ConsoleWriteLineGeneratedPaths),
        typeof(GenEntitas.GeneratePath),
        typeof(GenEntitas.IgnoreNamespaces),
        typeof(GenEntitas.ReflectionAssemblyPaths),
        typeof(GenEntitas.RunInDryMode)
    };
}
