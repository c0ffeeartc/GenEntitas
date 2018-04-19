﻿using System;
using System.Collections.Generic;
using Entitas.CodeGeneration.Attributes;
using GenEntitas.Sources;
using NSpec;

namespace Tests.Tests
{
	public class describe_GenEventEntityApiSystem : nspec
	{
		private				Contexts				_contexts;

		private				void					given_empty_context		(  )
		{
			context["when adding context comp entity"] = (  ) =>
			{
				_contexts				= new Contexts(  );
				var system				= new GenEventEntityApiSystem( _contexts );

				var ent					= _contexts.main.CreateEntity(  );
				ent.AddComp( "TestComp1", "TestComp1" );
				ent.AddEventComp( new List<EventInfo>(  )
					{
						new EventInfo{BindToEntity = true,EventType = EventType.Added, Priority =  0},
						new EventInfo{BindToEntity = false,EventType = EventType.Removed, Priority =  0},
					} );

				ent.AddContextNamesComp( new List<string>{ "Main", "Seconds" });

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