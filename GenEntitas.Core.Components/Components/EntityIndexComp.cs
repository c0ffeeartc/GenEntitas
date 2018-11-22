using System;
using System.Collections.Generic;
using Entitas;
using Entitas.CodeGeneration.Attributes;
using Entitas.CodeGeneration.Plugins;

namespace GenEntitas
{

[Context(Ids.Main)]
public class EntityIndexComp : IComponent
{
	public					List<EntityIndexInfo>	Values;
}

[Context(Ids.Main)]
public class CustomEntityIndexComp : IComponent
{
	public					EntityIndexData			EntityIndexData;
}

public class EntityIndexInfo
{
	public					EntityIndexData			EntityIndexData;
	public					String					Type;
	public					Boolean					IsCustom;
	public					MethodData[]			CustomMethods;
	public					String					Name;
    public					String[]				ContextNames;
    public					String					ComponentType;
    public					String					MemberType;
	public					String					MemberName;
	public					Boolean					HasMultple;
	
}

}
