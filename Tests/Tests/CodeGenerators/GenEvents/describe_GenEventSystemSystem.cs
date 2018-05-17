using System;
using System.Collections.Generic;
using Entitas.CodeGeneration.Attributes;
using GenEntitas;
using NSpec;

namespace Tests.Tests
{
	public class describe_GenEventSystemSystem : nspec
	{
		private				Contexts				_contexts;

		private				void					given_empty_context		(  )
		{
			context["when adding context comp entity"] = (  ) =>
			{
				_contexts				= new Contexts(  );
				var system				= new GenEventSystemSystem( _contexts );

				var ent					= _contexts.main.CreateEntity(  );
				ent.AddComp( "TestComp1", "TestComp1" );
				ent.AddEventComp( new List<EventInfo>(  )
					{
						new EventInfo(bindToEntity : true,eventType : EventType.Added, priority :  0),
						new EventInfo(bindToEntity : false,eventType : EventType.Removed, priority :  0),
					} );
				ent.AddContextNamesComp( new List<string>{ "Main", "Second" });

//				var ent2					= _contexts.main.CreateEntity(  );
//				ent2.AddComp( "TestComp2", "TestComp2" );
//				ent2.AddEventComp( new List<EventInfo>(  )
//					{
//						new EventInfo{BindToEntity = true, EventType = EventType.Added, Priority =  0},
//						new EventInfo{BindToEntity = false, EventType = EventType.Removed, Priority =  0},
//					} );
//				ent2.AddContextNamesComp( new List<string>{ "Main", "Second" });

				var genFileGroup		= _contexts.main.GetGroup( MainMatcher.GeneratedFileComp );

				it["has 0 generated file comps"] = (  ) =>
				{
					genFileGroup.count.should_be( 0 );
				};

				it["has contexts.Count * eventInfo.Count generated file comps"] = (  ) =>
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