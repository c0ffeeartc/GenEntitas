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
				ent.AddContextComp( "Main" );

				var ent2				= _contexts.main.CreateEntity(  );
				ent2.AddContextComp( "Second" );

				var contextGroup		= _contexts.main.GetGroup( MainMatcher.ContextComp );
				var genFileGroup		= _contexts.main.GetGroup( MainMatcher.GeneratedFileComp );

				it["has 0 generated file comps"] = (  ) =>
				{
					genFileGroup.count.should_be( 0 );
				};

				it["has contexts.Count generated file comps"] = (  ) =>
				{
					system.Execute(  );
					genFileGroup.count.should_be( contextGroup.count );
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