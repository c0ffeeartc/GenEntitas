using System;
using System.Collections.Generic;
using Entitas;
using Entitas.CodeGeneration.Attributes;

[Context(Ids.Main), Unique]
public class ReflectionComponentTypes : IComponent
{
	public					List<Type>				Values;
}
