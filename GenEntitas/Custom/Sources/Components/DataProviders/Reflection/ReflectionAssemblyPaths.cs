using System;
using System.Collections.Generic;
using Entitas;
using Entitas.CodeGeneration.Attributes;

[Settings, Unique]
public class ReflectionAssemblyPaths : IComponent
{
	public					List<String>			Values;
}