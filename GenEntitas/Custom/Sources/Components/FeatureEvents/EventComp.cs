using System;
using Entitas;
using Entitas.CodeGeneration.Attributes;

[Main]
public class EventComp : IComponent
{
	public					Boolean					BindToEntity;
	public					EventType				EventType;
	public					Int32					Priority;
}