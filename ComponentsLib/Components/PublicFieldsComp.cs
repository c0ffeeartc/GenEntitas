using System;
using System.Collections.Generic;
using Entitas.CodeGeneration.Attributes;
using Entitas;

namespace GenEntitas
{

[Context(Ids.Main)]
public class PublicFieldsComp : IComponent
{
	public					List<FieldInfo>			Values;
}

[Serializable]
public partial class FieldInfo
{
	public					FieldInfo				( String typeName, String fieldName )
	{
		TypeName			= typeName;
		FieldName			= fieldName;
	}

	public					String					TypeName;
	public					String					FieldName;
}

public partial class FieldInfo
{
	public					EntityIndexInfo			EntityIndexInfo;
}

}
