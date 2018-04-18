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
	public					String					TypeName;
	public					String					FieldName;
}
