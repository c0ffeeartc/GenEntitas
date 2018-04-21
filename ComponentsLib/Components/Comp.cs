using System;
using Entitas;
using Entitas.CodeGeneration.Attributes;

[Context(Ids.Main)]
public class Comp : IComponent
{
	public					String					Name;
	public					String					FullTypeName;
}
