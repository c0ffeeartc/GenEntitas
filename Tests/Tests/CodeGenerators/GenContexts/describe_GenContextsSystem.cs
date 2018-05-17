using System;
using GenEntitas;
using NSpec;

namespace Tests.Tests
{
	public class describe_GenContextsSystem : nspec
	{
		private				Contexts				_contexts;

		private				void					given_empty_context		(  )
		{
			context["when adding context comp entity"] = (  ) =>
			{
				_contexts				= new Contexts(  );
				var system				= new GenContextsSystem( _contexts );

				var ent					= _contexts.main.CreateEntity(  );
				ent.AddContextComp( "Main" );

				var ent2				= _contexts.main.CreateEntity(  );
				ent2.AddContextComp( "Second" );

				var genFileGroup		= _contexts.main.GetGroup( MainMatcher.GeneratedFileComp );

				it["has 0 generated file comps"] = (  ) =>
				{
					genFileGroup.count.should_be( 0 );
				};

				it["has 1 generated file comps"] = (  ) =>
				{
					system.Execute(  );
					genFileGroup.count.should_be( 1 );
					genFileGroup.GetSingleEntity(  ).generatedFileComp.FilePath.should_be( "Contexts.cs" );
				};

				it["Replaces markers in template"] = (  ) =>
				{
					var fileEnt		= genFileGroup.GetSingleEntity(  );
					fileEnt.generatedFileComp.Contents.IndexOf( '$' ).should_be( -1 );
					//Console.Write( fileEnt.generatedFileComp.Contents );
				};
			};
		}
	}
}