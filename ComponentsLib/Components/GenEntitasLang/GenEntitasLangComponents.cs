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

	[Context(Ids.Main)]
	public class GenEntitasLangInputString : IComponent
	{
		public				String					Value;
	}

	[Context(Ids.Main)]
	public class ParsedByGenEntitasLang : IComponent
	{
	}

	[Context(Ids.Settings), Unique]
	public class GenEntitasLangPaths : IComponent
	{
		public				List<String>			Values;
	}
}