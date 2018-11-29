## HelloWorld plugin
  - Calls `Console.WriteLine("HelloWorld")`
  - Calls `Console.WriteLine` for each `HelloWorld` setting value

### Plugin Requirements:
  - Dll name matches pattern `GenEntitas.*.dll`
  - Dll is located in the same folder as `GenEntitas.exe`
  - System has attribute `[Export(typeof(ISystem))]`
  - System has `Guid` attribute with unique guid
  - System has parameterless constructor, so it can be created during import
