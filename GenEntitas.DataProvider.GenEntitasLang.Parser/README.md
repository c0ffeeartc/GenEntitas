## GenEntitasLang

A string parser using [Sprache](https://github.com/sprache/Sprache) that provides components required by GenEntitas generators.
Syntax is mostly based on [Entitas-Lang](https://github.com/mzaks/Entitas-Lang), skipping some features like Systems, Target, Version keywords.

Example of acceptable string
```
// Aliases allow cleaner write style 
alias
    int : "System.Int32"
    single : "System.Single"
alias gameObj : "UnityEngine.GameObject"

/* Comps provide info for generation.
 Parser can be extended using Sprache */

comp Destroy in Game

comp Player in Game
    unique

comp Position in Game, Input
    publicFields :
        x : int
        y : "System.Int32"

```

All keywords and grammar may be found in [GenEntitasLangParser.cs](./Sources/GenEntitasLangParser.cs).
Later syntax will be better explained in a readme

### Usage
  - create files with GenEntitasLang syntax
  - add `GenEntitasLangPaths = "./pathsToYourFileHere", "./anotherPath"` to a settings file
