## Settings

Settings class and string parser using [Sprache](https://github.com/sprache/Sprache)

#### How it works
  - Removes comments from file
  - Parses `Dictionary<String, List<String>>`

#### Syntax example
```
// comment
Key1 = "true"
Key2 =
   "value1",
   "value2",
   "value3",
 
/* another comment */
   "value4",
```
Example [GenComps.settings](../GenComps.settings)

  - `GeneratePath` - path to existing directory in which a new directory `Generated` will be created
  - `SearchPaths` - paths to folders with dll dependencies
  - `RoslynPathToSolution` - optional path to `.sln` file for Roslyn data provider
  - `GenEntitasLangPaths` - optional paths to GenEntitasLang files for GenEntitasLang data provider
  - `ReflectionAssemblyPaths` - optional paths to `.dll` files for Reflection data provider
  - `WriteGeneratedPathsToCsproj` - optional path to `.csproj` file with generated paths
  - `LogGeneratedPaths` - logs paths changes. _Default = true_
  - `RunInDryMode` - don't apply changes to disk. _Default = false_
  - `IgnoreNamespaces` - _Default = false_
  - `GeneratedNamespace` - wraps generated classes into namespace
