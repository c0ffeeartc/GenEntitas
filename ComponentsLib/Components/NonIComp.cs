using System;
using Entitas;
using Entitas.CodeGeneration.Attributes;

[Context(Ids.Main)]
public class NonIComp : IComponent
{
	public					String					FullCompName;
	public					String					FieldTypeName;
}
