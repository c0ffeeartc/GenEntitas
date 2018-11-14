using System;
using System.Collections.Generic;
using Entitas.CodeGeneration.Attributes;
using GenEntitas;
using NSpec;

namespace Tests.Tests
{
	public class describe_GenEventListenerCompSystem : nspec
	{
		private				Contexts				_contexts;

		private				void					given_empty_context		(  )
		{
			context["when adding context comp entity"] = (  ) =>
			{
				_contexts				= new Contexts(  );
				var system				= new GenEventListenerCompSystem( _contexts );

				var ent					= _contexts.main.CreateEntity(  );
				ent.AddComp( "TestComp1", "TestComp1" );
				ent.AddEventComp( new List<EventInfo>(  )
					{
						new EventInfo(eventTarget: EventTarget.Self, eventType : EventType.Added, priority :  0),
						new EventInfo(eventTarget: EventTarget.Any, eventType : EventType.Removed, priority :  0),
					} );

				ent.AddContextNamesComp( new List<string>{ "Main", "Second" });

				var genFileGroup		= _contexts.main.GetGroup( MainMatcher.GeneratedFileComp );

				it["has 0 generated file comps"] = (  ) =>
				{
					genFileGroup.count.should_be( 0 );
				};

				it["has = context * eventInfo generated file comps"] = (  ) =>
				{
					system.Execute(  );
					genFileGroup.count.should_be( ent.contextNamesComp.Values.Count * ent.eventComp.Values.Count );
				};

				it["Replaces markers in template"] = (  ) =>
				{
					var fileEnts = genFileGroup.GetEntities(  );
					foreach ( var fileEnt in fileEnts )
					{
						fileEnt.generatedFileComp.Contents.IndexOf( '$' ).should_be( -1 );
						//Console.Write( fileEnt.generatedFileComp.Contents );
					}
				};
			};
		}
	}
}