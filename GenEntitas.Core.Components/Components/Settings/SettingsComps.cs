using System;
using System.Collections.Generic;
using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace GenEntitas
{

[Context(Ids.Settings), Unique]
public class SearchPaths : IComponent
{
	public					List<String>			Value;
}

[Context(Ids.Settings)]
[Unique]
public class CsprojPath : IComponent
{
	public					String					Value;
}

[Context(Ids.Settings), Unique]
public class IgnoreNamespaces : IComponent
{
}

[Context(Ids.Settings), Unique]
public class RunInDryMode : IComponent
{
}

[Context(Ids.Settings), Unique]
public class ConsoleWriteLineGeneratedPaths : IComponent
{
}

[Context(Ids.Settings), Unique]
public class GeneratePath : IComponent
{
	public					String					Value;
}

[Context(Ids.Settings), Unique]
public class GeneratedNamespace : IComponent
{
	public					String					Value;
}

}