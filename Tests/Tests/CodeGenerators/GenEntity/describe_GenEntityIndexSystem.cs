using System;
using System.Collections.Generic;
using DesperateDevs.CodeGeneration;
using Entitas;
using Entitas.CodeGeneration.Attributes;
using GenEntitas;
using NSpec;

[Context("Main")]
[Context("Second")]
internal class TestEntityIndexComp1 : IComponent
{
	[EntityIndex]
	public int Indexed;
}

[Context("Main")]
[Context("Second")]
internal class TestEntityIndexComp2 : IComponent
{
	[PrimaryEntityIndex]
	public int PrimaryIndexed;
	[EntityIndex]
	public int EntityIndexed;
}

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
				var systems				= new Systems(  )
					.Add( new ReflectionToCompsSystem( _contexts ) )
					.Add( new ReflectionToEntityIndexSystem( _contexts ) )
					.Add( new GenEntityIndexSystem( _contexts ) )
					;

				var ent					= _contexts.main.CreateEntity(  );
				ent.AddReflectionComponentTypes( new List<Type>
				{
					typeof(TestEntityIndexComp1)
				} );
				ent.AddReflectionLoadableTypes( new List<Type>
				{
					typeof(TestEntityIndexComp1)
				} );

				var genFileGroup		= _contexts.main.GetGroup( MainMatcher.GeneratedFileComp );

				it["has 0 generated file comps"] = (  ) =>
				{
					genFileGroup.count.should_be( 0 );
				};

				it["has contexts.Count generated file comps"] = (  ) =>
				{
					systems.Execute(  );
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

		private				void					test_multiple_indexes	(  )
		{
			before					= (  )=>
			{
				_contexts			= new Contexts(  );
			};

			it["Contains two indexes of one component"] = (  ) =>
			{
				// given
				var systems				= new Systems(  )
					.Add( new ReflectionToCompsSystem( _contexts ) )
					.Add( new ReflectionToEntityIndexSystem( _contexts ) )
					.Add( new GenEntityIndexSystem( _contexts ) )
					;

				var ent					= _contexts.main.CreateEntity(  );
				ent.AddReflectionComponentTypes( new List<Type>
				{
					typeof(TestEntityIndexComp2)
				} );
				ent.AddReflectionLoadableTypes( new List<Type>
				{
					typeof(TestEntityIndexComp2)
				} );

				// when
				systems.Execute(  );

				// then
				var genFileGroup		= _contexts.main.GetGroup( MainMatcher.GeneratedFileComp );
				var genFileEnts			= genFileGroup.GetEntities(  );
				var hasContextsFile		= false;
				foreach ( var genFile in genFileEnts )
				{
					if ( !genFile.generatedFileComp.FilePath.EndsWith( "Contexts.cs" ) )
					{
						continue;
					}
					hasContextsFile = true;
					genFile.generatedFileComp.Contents.Contains( "PrimaryIndexed" ).should_be_true(  );
					genFile.generatedFileComp.Contents.Contains( "EntityIndexed" ).should_be_true(  );
				}
				hasContextsFile.should_be( true );
			};
		}
	}
}