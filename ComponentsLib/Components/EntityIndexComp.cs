using System;
using System.Collections.Generic;
using Entitas;
using Entitas.CodeGeneration.Attributes;
using Entitas.CodeGeneration.Plugins;

[Context(Ids.Main)]
public class EntityIndexComp : IComponent
{
	public					List<EntityIndexInfo>	Values;
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
