using System;
using System.Collections.Generic;
using Entitas;

[Main]
public class PublicFieldsComp : IComponent
{
	public					List<FieldInfoComp>		Values;
}

[Serializable]
public class FieldInfoComp
{
	public					String					TypeName;
	public					String					FieldName;
}
