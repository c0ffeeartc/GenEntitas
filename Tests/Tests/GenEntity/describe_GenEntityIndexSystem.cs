﻿using System;
using System.Collections.Generic;
using Entitas.CodeGeneration.Attributes;
using GenEntitas.Sources;
using NSpec;

namespace Tests.Tests
{
	public class describe_GenEntityIndexSystem : nspec
	{
		private				Contexts				_contexts;

		private				void					given_empty_context		(  )
		{
			context["when adding context comp entity"] = (  ) =>
			{
				_contexts				= new Contexts(  );
				var system				= new GenEntityIndexSystem( _contexts );

				var ent					= _contexts.main.CreateEntity(  );
				ent.AddComp( "TestComp1", "TestComp1" );
				ent.AddEntityIndexComp(
					new List<EntityIndexInfo>
					{
						new EntityIndexInfo
						{
							EntityIndexType = EntityIndexType.EntityIndex,
							FieldInfo = new FieldInfo
							{
								FieldName = "test",
								TypeName = "int",
							}
						}
					} );
				ent.AddPublicFieldsComp( new List<FieldInfo>{new FieldInfo{FieldName = "test", TypeName = "int"}} );
				ent.AddContextNamesComp( new List<String>{"Main", "Second"} );

				var genFileGroup		= _contexts.main.GetGroup( MainMatcher.GeneratedFileComp );

				it["has 0 generated file comps"] = (  ) =>
				{
					genFileGroup.count.should_be( 0 );
				};

				it["has contexts.Count generated file comps"] = (  ) =>
				{
					system.Execute(  );
					genFileGroup.count.should_be( 1 );
				};

				it["Replaces markers in template"] = (  ) =>
				{
					var fileEnts = genFileGroup.GetEntities(  );
					foreach ( var fileEnt in fileEnts )
					{
						fileEnt.generatedFileComp.Contents.IndexOf( '$' ).should_be( -1 );
						fileEnt.generatedFileComp.Contents.IndexOf( "FIXME", StringComparison.Ordinal ).should_be( -1 );
						//Console.Write( fileEnt.generatedFileComp.Contents );
					}
				};
			};
		}
	}
}