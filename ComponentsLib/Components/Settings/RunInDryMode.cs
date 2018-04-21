using Entitas;
using Entitas.CodeGeneration.Attributes;

[Context(Ids.Settings), Unique]
public class RunInDryMode : IComponent
{
}
