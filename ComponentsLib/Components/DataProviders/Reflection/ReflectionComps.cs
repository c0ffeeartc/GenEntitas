using System;
using System.Collections.Generic;
using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace GenEntitas
{

[Context(Ids.Main)]
public class TypeComp : IComponent
{
	[EntityIndex]
	public					Type					Value;
}

[Context(Ids.Main), Unique]
public class ReflectionComponentTypes : IComponent
{
	public					List<Type>				Values;
}

[Context(Ids.Main), Unique]
public class ReflectionLoadableTypes : IComponent
{
	public					List<Type>				Values;
}

[Context(Ids.Settings), Unique]
public class ReflectionAssemblyPaths : IComponent
{
	public					List<String>			Values;
}

}