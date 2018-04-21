using System;
using System.Collections.Generic;
using Entitas;
using Entitas.CodeGeneration.Attributes;

[Context(Ids.Main)]
public class ContextNamesComp : IComponent
{
	public					List<String>			Values;
}

