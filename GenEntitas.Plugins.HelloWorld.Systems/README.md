## HelloWorld plugin
  - Calls `Console.WriteLine(">> Hello world system")`
  - Calls `Console.WriteLine` for each `HelloWorld` setting value

Run `mono ./pathToExe/GenEntitas.exe --SettingsPath=./HelloWorld.settings` to see it in action

### Plugin Requirements:
  - Dll name matches pattern `GenEntitas.*.dll`
  - Dll is located in the same folder as `GenEntitas.exe`
  - System has attribute `[Export(typeof(ISystem))]`
  - System has `Guid` attribute with unique guid
  - System has parameterless constructor, so it can be created during import
  - To run system add it to `SystemGuids` in settings. See [HelloWorld.settings](./HelloWorld.settings)
