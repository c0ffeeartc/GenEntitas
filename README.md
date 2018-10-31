# GenEntitas
Entitas generator using [Entitas](https://github.com/sschmid/Entitas-CSharp) ECS framework

### How it works
  - Console Runner reads settings file
  - Systems.Execute is called once
    - DataProvider systems add entities with components
    - Generator systems react to components in entities
    - PostProcessor systems write changes to disk 

### Parts
  - [Tests](./Tests)
  - [Components](./ComponentsLib)
  - [Systems](./GenEntitasLib)
  - [Console Runner](./GenEntitas)
  - [Unity Runner](./UnityRunner)
  - [Settings](GenEntitasSettings)
  - [GenEntitasLang](./GenEntitasLang)
  
  
### Pros
  - familiar ECS architecture for Entitas users
  - easy to add custom generator - create System, add it to Systems
  - multiple input sources during the same run - dlls for reflection, GenEntitasLang files
  - option to wrap generated classes into namespace

### Cons
  - unofficial
  - tied to specific entitas version
  - there is no settings auto creation. You'll have to provide paths to folders with unity dlls. After done once, settings can be copy/paste/tweaked for other projects
