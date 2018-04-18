//-------------------------------------------------
// Copyright (C) 0000-2018, Yegor c0ffee
// Email: c0ffeeartc@gmail.com
//-------------------------------------------------

using System;
using Entitas;

[Main]
public class Comp : IComponent
{
	public					String					Name;
	public					String					FullTypeName;
}

[Main]
public class CompUniquePrefix : IComponent
{
	public					String					Value;
}
