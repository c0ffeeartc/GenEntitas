using System;
using System.Collections.Generic;
using Entitas;
using Entitas.CodeGeneration.Attributes;

[Main]
public class EventComp : IComponent
{
	public					List<EventInfo>			Values;
}

public class EventInfo
{
	public					EventInfo				( bool bindToEntity, EventType eventType, int priority )
	{
		BindToEntity		= bindToEntity;
		EventType			= eventType;
		Priority			= priority;
	}

	public					Boolean					BindToEntity;
	public					EventType				EventType;
	public					Int32					Priority;
}
