//-------------------------------------------------
// Copyright (C) 0000-2018, Yegor c0ffee
// Email: c0ffeeartc@gmail.com
//-------------------------------------------------

using System;
using Entitas;

[Main]
public class Comp : IComponent
{
	public					String					FileName;
}

[Serializable]
public class FieldInfoComp
{
	public					String					TypeName;
	public					String					FieldName;
}
