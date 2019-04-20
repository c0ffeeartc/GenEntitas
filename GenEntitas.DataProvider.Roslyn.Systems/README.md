## Roslyn Data Provider

### How it works
  - Reads solution file
  - For OSX - fixes and creates copy of `.sln`
  - Compiles projects and provides `List<INamedTypeSymbol>` using Roslyn
  - Provides entities with components from `List<INamedTypeSymbol>`

### Usage

  - Add `RoslynPathToSolution = "./PathToYourSolution.sln"` to settings file
  - use console runner
