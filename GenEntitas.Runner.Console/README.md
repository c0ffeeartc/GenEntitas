## Console Runner

Builds into executable, run by `mono GenEntitas.exe`.

#### See also
  - [Program.cs](./Custom/Program.cs) - `Main` function and console arguments
  - [Runner.cs](./Custom/Runner.cs) - systems and order of execution

### Building

  - Open `CodeGen.sln` solution file
  - Build solution
  - After successful build `GenEntitas/bin` will contain `GenEntitas.exe` along with dependencies

### Using console runner to generate from external dll with components

  - Create library project for Component classes
  - Fill it with components. Same as in Unity, use `[Context("Game")]` instead of `[Game]` attribute
  - Build components dll
  - Run `mono GenEntitas.exe` and follow help - provide settings file
