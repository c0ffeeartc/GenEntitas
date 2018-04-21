using System;
using Entitas;
using Entitas.CodeGeneration.Attributes;

[Context(Ids.Main)]
public class UniquePrefixComp : IComponent
{
	public					String					Value;
}