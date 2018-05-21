using System;
using System.Collections.Generic;
using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace GenEntitas
{
	[Context(Ids.Main)]
	[Unique]
	public class AliasComp : IComponent
	{
		public			Dictionary<String,String>	Values;
	}
}