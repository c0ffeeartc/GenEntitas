using System;
using Entitas;
using Entitas.CodeGeneration.Attributes;

[Context(Ids.Main)]
public class TypeComp : IComponent
{
	public					Type					Value;
}