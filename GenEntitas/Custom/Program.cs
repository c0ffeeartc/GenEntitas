using System;
using System.Linq;
using CommandLine;

namespace GenEntitas
{
	class Options
	{
		[Option( "dllPaths", Required = true, HelpText = "Reflection dlls with Component classes" )]
		public String DllPaths { get; set; }
	}

	internal class Program
	{
		public static void Main( string[] args )
		{
			Parser.Default.ParseArguments<Options>(args).WithParsed( WithParsedArgs );
		}
		private static void WithParsedArgs( Options options )
		{
			var runner = new Runner();
			runner.Init(  );

			var contexts = runner.Contexts;
			FillContexts( contexts, options );

			runner.Systems.Initialize(  );
			runner.Systems.Execute(  );
			runner.Systems.Cleanup(  );
			runner.Systems.TearDown(  );
		}

		private static void FillContexts( Contexts contexts, Options options )
		{
			contexts.settings.isConsoleWriteLineGeneratedPaths		= true;
			contexts.settings.SetGeneratePath( "./" );
			contexts.settings.SetReflectionAssemblyPaths( options.DllPaths.Split(',').ToList(  ) );
		}
	}
}