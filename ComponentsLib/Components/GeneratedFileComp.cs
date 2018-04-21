using System;
using Entitas;
using Entitas.CodeGeneration.Attributes;

[Context(Ids.Main)]
public class GeneratedFileComp : IComponent
{
	public					String					FilePath;
	public					String					Contents;
	public					String					GeneratedBy;
}
