﻿using System;
using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace GenEntitas
{
[Context(Ids.Settings), Unique]
public class SettingsPath : IComponent
{
	public					String					Value;
}

[Context(Ids.Settings), Unique]
public class SettingsParseInput : IComponent
{
	public					String					Value;
}

}