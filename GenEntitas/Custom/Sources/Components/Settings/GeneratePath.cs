using System;
using Entitas;
using Entitas.CodeGeneration.Attributes;

[Settings, Unique]
public class GeneratePath : IComponent
{
	public					String					Value;
}