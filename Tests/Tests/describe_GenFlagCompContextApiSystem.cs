﻿using System;
using System.Collections.Generic;
using GenEntitas.Sources;
using NSpec;

namespace Tests.Tests
{
	public class describe_GenFlagCompContextApiSystem : nspec
	{
		private				Contexts				_contexts;

		private				void					given_empty_context		(  )
		{
			context["when adding context comp entity"] = (  ) =>
			{
				_contexts				= new Contexts();
				var system				= new GenFlagCompContextApiSystem( _contexts );

				var ent					= _contexts.main.CreateEntity(  );
				ent.AddComp( "TestComp1", "TestComp1" );
				ent.AddContextNamesComp( new List<String>{ "Main" } );

				var genFileGroup		= _contexts.main.GetGroup( MainMatcher.GeneratedFileComp );

				it["has 0 generated file comps"] = (  ) =>
				{
					genFileGroup.count.should_be( 0 );
				};

				it["has 1 generated file comps"] = (  ) =>
				{
					system.Execute(  );
					genFileGroup.count.should_be( 1 );
				};

				it["Replaces markers in template"] = (  ) =>
				{
					genFileGroup.GetSingleEntity(  ).generatedFileComp.Contents.IndexOf( '$' ).should_be( -1 );
				};
			};
		}
	}
}