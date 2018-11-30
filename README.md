# GenEntitas
Entitas generator using [Entitas](https://github.com/sschmid/Entitas-CSharp) ECS framework

### How it works
  - Console Runner reads settings file
  - Systems are imported from dlls
  - Systems `Initialize/Execute/Cleanup/TearDown` once
    - DataProvider systems add entities with components
    - Generator systems react to components in entities
    - PostProcessor systems write changes to disk, log, etc.

### Parts
  - [Console Runner](./GenEntitas.Runner.Console)
  - [Settings](GenEntitas.Core.Settings.Parser)
  - [Systems](./GenEntitas.Core.Systems)
  - [Components](./GenEntitas.Core.Components)
  - [DataProvider.Reflection](./GenEntitas.DataProvider.Reflection.Systems)
  - [DataProvider.Roslyn](./GenEntitas.DataProvider.Roslyn.Systems)
  - [DataProvider.GenEntitasLang](./GenEntitas.DataProvider.GenEntitasLang.Parser)
  - [HelloWorld Plugin](./GenEntitas.Plugins.HelloWorld.Systems)
  - [Tests](./Tests)

### Pros
  - familiar ECS architecture for Entitas users
  - easy to add custom generator - create System, add it to Systems
  - multiple input sources - dlls for reflection, GenEntitasLang files, Roslyn
  - option to wrap generated classes into namespace

### Cons
  - unofficial
  - tied to specific entitas version
  - there is no settings auto creation. You'll have to provide paths to folders with unity dlls. After done once, settings can be copy/paste/tweaked for other projects
  - Roslyn data provider lacks server mode, and because of that is much slower compared to Entitas Jenny

### Usage:
 Before using build solution. See [Console Runner](./GenEntitas.Runner.Console) for details
  - create settings. See [Settings](GenEntitas.Core.Settings.Parser)
  - call `mono ./path_here/GenEntitas.exe --SettingsPath=./pathToSettings` . See [genComps.sh](genComps.sh)
