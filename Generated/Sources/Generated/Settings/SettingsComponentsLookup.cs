namespace GenEntitas {
public static class SettingsComponentsLookup {

    public const int ConsoleWriteLineGeneratedPaths = 0;
    public const int CsprojPath = 1;
    public const int GenEntitasLangPaths = 2;
    public const int GeneratedNamespace = 3;
    public const int GeneratePath = 4;
    public const int IgnoreNamespaces = 5;
    public const int ReflectionAssemblyPaths = 6;
    public const int RoslynPathToSolution = 7;
    public const int RunInDryMode = 8;
    public const int SearchPaths = 9;
    public const int SettingsParseInput = 10;
    public const int SettingsPath = 11;

    public const int TotalComponents = 12;

    public static readonly string[] componentNames = {
        "ConsoleWriteLineGeneratedPaths",
        "CsprojPath",
        "GenEntitasLangPaths",
        "GeneratedNamespace",
        "GeneratePath",
        "IgnoreNamespaces",
        "ReflectionAssemblyPaths",
        "RoslynPathToSolution",
        "RunInDryMode",
        "SearchPaths",
        "SettingsParseInput",
        "SettingsPath"
    };

    public static readonly System.Type[] componentTypes = {
        typeof(GenEntitas.ConsoleWriteLineGeneratedPaths),
        typeof(GenEntitas.CsprojPath),
        typeof(GenEntitas.GenEntitasLangPaths),
        typeof(GenEntitas.GeneratedNamespace),
        typeof(GenEntitas.GeneratePath),
        typeof(GenEntitas.IgnoreNamespaces),
        typeof(GenEntitas.ReflectionAssemblyPaths),
        typeof(GenEntitas.RoslynPathToSolution),
        typeof(GenEntitas.RunInDryMode),
        typeof(GenEntitas.SearchPaths),
        typeof(GenEntitas.SettingsParseInput),
        typeof(GenEntitas.SettingsPath)
    };
}

}
