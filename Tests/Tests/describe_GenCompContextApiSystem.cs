﻿using System;
using System.Collections.Generic;
using GenEntitas.Sources;
using NSpec;

namespace Tests.Tests
{
	public class describe_GenCompContextApiSystem : nspec
	{
		private				Contexts				_contexts;

		private				void					given_empty_context		(  )
		{
			context["when adding context comp entity"] = (  ) =>
			{
				_contexts				= new Contexts();
				var system				= new GenCompContextApiSystem( _contexts );

				var ent					= _contexts.main.CreateEntity(  );
				ent.AddComp( "TestComp1", "TestComp1" );
				ent.AddContextNamesComp( new List<String>{ "Main" } );
				ent.AddPublicFieldsComp( new List<FieldInfo>{ new FieldInfo(){ FieldName = "Value", TypeName = "int" } } );

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