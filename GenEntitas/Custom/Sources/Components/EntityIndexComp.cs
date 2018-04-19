using System;
using System.Collections.Generic;
using Entitas;
using Entitas.CodeGeneration.Attributes;

[Main]
public class EntityIndexComp : IComponent
{
	public					List<EntityIndexInfo>	Values;
}

public class EntityIndexInfo
{
	public					Boolean					IsCustom;
	public					EntityIndexType			EntityIndexType;
	public					FieldInfo				FieldInfo;
}
