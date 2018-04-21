using System;
using System.Collections.Generic;
using Entitas;

[Main]
public class PublicFieldsComp : IComponent
{
	public					List<FieldInfo>			Values;
}

[Serializable]
public class FieldInfo
{
	public					FieldInfo				( String typeName, String fieldName )
	{
		TypeName			= typeName;
		FieldName			= fieldName;
	}

	public					String					TypeName;
	public					String					FieldName;
}
