using System;
using Entitas;
using Entitas.CodeGeneration.Attributes;

[Context(Ids.Settings), Unique]
public class GeneratePath : IComponent
{
	public					String					Value;
}