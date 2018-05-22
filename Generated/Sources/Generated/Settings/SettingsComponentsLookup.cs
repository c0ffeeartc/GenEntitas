namespace GenEntitas {
public static class SettingsComponentsLookup {

    public const int ConsoleWriteLineGeneratedPaths = 0;
    public const int GenEntitasLangPaths = 1;
    public const int GeneratedNamespace = 2;
    public const int GeneratePath = 3;
    public const int IgnoreNamespaces = 4;
    public const int ReflectionAssemblyPaths = 5;
    public const int RunInDryMode = 6;

    public const int TotalComponents = 7;

    public static readonly string[] componentNames = {
        "ConsoleWriteLineGeneratedPaths",
        "GenEntitasLangPaths",
        "GeneratedNamespace",
        "GeneratePath",
        "IgnoreNamespaces",
        "ReflectionAssemblyPaths",
        "RunInDryMode"
    };

    public static readonly System.Type[] componentTypes = {
        typeof(GenEntitas.ConsoleWriteLineGeneratedPaths),
        typeof(GenEntitas.GenEntitasLangPaths),
        typeof(GenEntitas.GeneratedNamespace),
        typeof(GenEntitas.GeneratePath),
        typeof(GenEntitas.IgnoreNamespaces),
        typeof(GenEntitas.ReflectionAssemblyPaths),
        typeof(GenEntitas.RunInDryMode)
    };
}

}
