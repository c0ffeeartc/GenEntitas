## Roslyn Data Provider

  - Reads solution file
  - Fixes and create copy of `.sln` for OSX
  - Compiles projects and provides List<INamedTypeSymbol> using Roslyn
  - TODO: Provides entities with components from List<INamedTypeSymbol>

### Usage

  - Add `RoslynPathToSolution = ./PathToYourSolution.sln` to settings file
  - use console runner
