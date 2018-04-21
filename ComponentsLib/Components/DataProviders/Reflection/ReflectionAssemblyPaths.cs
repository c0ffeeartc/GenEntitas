using System;
using System.Collections.Generic;
using Entitas;
using Entitas.CodeGeneration.Attributes;

[Context(Ids.Settings), Unique]
public class ReflectionAssemblyPaths : IComponent
{
	public					List<String>			Values;
}