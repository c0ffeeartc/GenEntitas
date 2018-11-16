using System;
using System.Collections.Generic;
using Entitas;
using Entitas.CodeGeneration.Attributes;
using Microsoft.CodeAnalysis;

// TODO: move Roslyn component files and Generated folder to GenEntitas.DataProvider.Roslyn project
namespace GenEntitas
{

[Context(Ids.Settings)]
[Unique]
public class RoslynPathToSolution : IComponent
{
	public					String					Value;
}

[Context(Ids.Main)]
[Unique]
public class RoslynAllTypes : IComponent
{
	public					List<INamedTypeSymbol>	Values;
}

[Context(Ids.Main)]
[Unique]
public class RoslynComponentTypes : IComponent
{
	public					List<INamedTypeSymbol>	Values;
}

}