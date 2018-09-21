using System;
using System.Collections.Generic;
using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace GenEntitas
{

[Context(Ids.Main)]
public class Comp : IComponent
{
	public					String					Name;
	[PrimaryEntityIndex]
	public					String					FullTypeName;
}

[Context(Ids.Main)]
public class ContextComp : IComponent
{
	public					String					Name;
}

[Context(Ids.Main)]
public class AlreadyImplementedComp : IComponent
{
}

[Context(Ids.Main)]
public class ContextNamesComp : IComponent
{
	public					List<String>			Values;
}

[Context(Ids.Main)]
public class Destroy : IComponent
{
}

[Context(Ids.Main)]
public class DontGenerateComp : IComponent
{
	//TODO
	//public					Boolean					GenerateIndex;
}

[Context(Ids.Main)]
public class GeneratedFileComp : IComponent
{
	public					String					FilePath;
	public					String					Contents;
	public					String					GeneratedBy;
}

[Context(Ids.Main)]
public class NonIComp : IComponent
{
	public					String					FullCompName;
	public					String					FieldTypeName;
}

[Context(Ids.Main)]
public class UniqueComp : IComponent
{
}

[Context(Ids.Main)]
public class UniquePrefixComp : IComponent
{
	public					String					Value;
}

[Context(Ids.Main)]
public class GenCompEntApiInterface_ForSingleContext : IComponent
{
}

}