﻿using System;
using System.Collections.Generic;
using System.IO;
using Entitas;
using Entitas.CodeGeneration.Plugins;
using Ent = MainEntity;

namespace GenEntitas.Sources
{
	public class GenEventListenerInterfaceSystem : ReactiveSystem<Ent>
	{
		public			GenEventListenerInterfaceSystem	( Contexts contexts ) : base( contexts.main )
		{
			_contexts			= contexts;
		}

		private				Contexts				_contexts;

		protected override	ICollector<Ent>			GetTrigger				( IContext<Ent> context )
		{
			return context.CreateCollector( MainMatcher.AllOf( MainMatcher.Comp, MainMatcher.EventComp ) );
		}

		private const		String					TEMPLATE				=
@"public interface I${EventListener} {
    void On${ComponentName}${EventType}(${ContextName}Entity entity${methodParameters});
}
";

		protected override	Boolean					Filter					( Ent entity )
		{
			return entity.hasComp && entity.hasEventComp;
		}

		protected override	void					Execute					( List<Ent> entities )
		{
			foreach ( var ent in entities )
			{
				var contextNames = ent.contextNamesComp.Values;
				foreach ( var contextName in contextNames )
				{
					var eventInfos = ent.eventComp.Values;
					foreach ( var eventInfo in eventInfos )
					{
						var filePath		= "Events" + Path.DirectorySeparatorChar + "Interfaces" + Path.DirectorySeparatorChar + "I" + ent.EventListener(contextName, eventInfo) + ".cs";

						var contents = TEMPLATE
							.Replace("${methodParameters}", ent.GetEventMethodArgs(eventInfo
								, ent.hasPublicFieldsComp
									? ", " + ent.publicFieldsComp.Values.GetMethodParameters(false)
									: ""))
							.Replace(ent, contextName, eventInfo);

						var generatedBy		= GetType().FullName;

						var fileEnt			= _contexts.main.CreateEntity(  );
						fileEnt.AddGeneratedFileComp( filePath, contents, generatedBy );
					}
				}
			}
		}
	}
}