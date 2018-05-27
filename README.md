# GenEntitas
Entitas generator using [Entitas](https://github.com/sschmid/Entitas-CSharp) ECS framework

### Benefits
  - easy to extend - write Entitas System!
  - multiple input sources during the same run - dlls for reflection, GenEntitasLang files
  - generated classes can be wrapped in optional namespace

### Parts
  - [Tests](./Tests)
  - [Components](./ComponentsLib)
  - [Systems](./GenEntitasLib)
  - [Console Runner](./GenEntitas)
  - [Unity Runner](./UnityRunner)
  - [Settings](GenEntitasSettings)
  - [GenEntitasLang](./GenEntitasLang)

### Needs to be done

  - More tests
  - UI
  - Settings
    - on/off generators, change systems order
  - PostProcessor
    - Update .csproj. Instead of this postprocessor glob can be used for non-unity csproj files
