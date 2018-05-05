# GenEntitas
Entitas generator using Entitas ECS framework

### Workflow
  - Open solution
  - Create class library project for Component classes
  - Fill components class library with components. Same as in Unity, use [Context("Game")] instead of [Game] attribute
  - (Optional) Reference components class library project in GenEntitas project. Helps Assembly.LoadFrom function to get needed types
  - Build
  - Run `mono GenEntitas.exe` and follow console help - fill dllPaths, and generatePath
  - Done
 
### Benefits
  - easy to extend - write Entitas System!

### Done
  - DataProviders
  - Generators
  - PostProcessors

### Needs to be done
  - More tests
  - UI
  - Settings
    - read from file
    - on/off generators, data providers etc
  - PreProcessors
  - PostProcessors
    - Update .csproj. Instead of this postprocessor glob can be used for non-unity csproj files
