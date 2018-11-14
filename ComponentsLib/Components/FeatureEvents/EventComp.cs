using System;
using System.Collections.Generic;
using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace GenEntitas
{

[Context(Ids.Main)]
public class EventComp : IComponent
{
	public					List<EventInfo>			Values;
}

public class EventInfo
{
	public					EventInfo				( EventTarget eventTarget, EventType eventType, int priority )
	{
		EventTarget			= eventTarget;
		EventType			= eventType;
		Priority			= priority;
	}

	public					EventTarget				EventTarget;
	public					EventType				EventType;
	public					Int32					Priority;
}

[Context(Ids.Main)]
public class EventListenerComp : IComponent
{
}
}
