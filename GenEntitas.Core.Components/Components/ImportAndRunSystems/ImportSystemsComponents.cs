using System;
using System.Collections.Generic;
using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace GenEntitas
{
[Context(Ids.Settings)]
[Unique]
public class SystemGuids : IComponent
{
	public					List<Guid>			Values;
}

[Context(Ids.Main)]
[Unique]
public class ImportedSystemsComponent : IComponent
{
	public				List<IExecuteSystem>	Values;
}

}