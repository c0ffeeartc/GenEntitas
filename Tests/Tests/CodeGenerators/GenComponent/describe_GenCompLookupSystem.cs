using System;
using System.Collections.Generic;
using GenEntitas.Sources;
using NSpec;

namespace Tests.Tests
{
	public class describe_GenCompLookupSystem : nspec
	{
		private				Contexts				_contexts;

		private				void					given_empty_context		(  )
		{
			context["when adding context comp entity"] = (  ) =>
			{
				_contexts				= new Contexts();
				var system				= new GenCompLookupSystem( _contexts );

				var ent					= _contexts.main.CreateEntity(  );
				ent.AddComp( "TestComp1", "TestComp1" );
				ent.AddContextNamesComp( new List<String>{ "Main", "Second" } );
				ent.AddPublicFieldsComp( new List<FieldInfo>{ new FieldInfo( fieldName : "Value", typeName : "int" ) } );

				var genFileGroup		= _contexts.main.GetGroup( MainMatcher.GeneratedFileComp );

				it["has 0 generated file comps"] = (  ) =>
				{
					genFileGroup.count.should_be( 0 );
				};

				it["has contextNames.Count generated file comps"] = (  ) =>
				{
					system.Execute(  );
					genFileGroup.count.should_be( ent.contextNamesComp.Values.Count );
				};

				it["Replaces markers in template"] = (  ) =>
				{
					var fileEnts = genFileGroup.GetEntities(  );
					foreach ( var fileEnt in fileEnts )
					{
						fileEnt.generatedFileComp.Contents.IndexOf( '$' ).should_be( -1 );
					}
				};
			};
		}
	}
}