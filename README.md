# GenEntitas
Entitas generator using Entitas ECS framework

### Benefits
  - easy to extend - write Entitas System!

### Parts
  - [Tests](./Tests)
  - [Components](./ComponentsLib)
  - [Systems](./GenEntitasLib)
  - [Console Runner](./GenEntitas)
  - [Unity Runner](./UnityRunner)

### Workflows

#### Building generator

  - Open `CodeGen.sln` solution file
  - Build solution
  - After successful build `GenEntitas/bin` will contain `GenEntitas.exe` along with dependencies

#### Generating from game components dll

  - Create library project for Component classes
  - Fill components library project with components. Same as in Unity, use `[Context("Game")]` instead of `[Game]` attribute
  - (Optional) Reference components project in GenEntitasLib project. Helps `Assembly.LoadFrom` function to get needed types
  - Build components dll
  - Run `mono GenEntitas.exe` and follow console help - fill dllPaths, and generatePath

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

