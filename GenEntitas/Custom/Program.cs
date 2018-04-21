using System;
using System.Collections.Generic;

namespace GenEntitas
{
	internal class Program
	{
		public static void Main( string[] args )
		{
			var runner = new Runner();
			runner.Init(  );

			var contexts = runner.Contexts;
			FillContexts( contexts );

			runner.Systems.Initialize(  );
			runner.Systems.Execute(  );
			runner.Systems.Cleanup(  );
			runner.Systems.TearDown(  );
		}

		public static void FillContexts( Contexts contexts )
		{
			contexts.settings.isConsoleWriteLineGeneratedPaths		= true;
			contexts.settings.SetGeneratePath( "./" );
			{
				var ent			= contexts.main.CreateEntity(  );
				ent.AddContextComp( "Main" );
			}

			{
				var ent			= contexts.main.CreateEntity(  );
				ent.AddComp( "TestComp1", "TestComp1" );
				ent.AddContextNamesComp( new List<String>{ "Main" } );
			}
		}
	}
}