using System;
using System.IO;
using System.Linq;
using CommandLine;
using Sprache;

namespace GenEntitas
{
	public class Args
	{
		[Option( "SettingsPath", Default = "", HelpText = "path to settings file. WARNING: If file is provided other command line options are ignored" )]
		public String SettingsPath { get; set; }

		[Option( "IgnoreNamespaces", Default = true, HelpText = "do not add namespace to component name" )]
		public Boolean IgnoreNamespaces { get; set; }

		[Option( "DllPaths", HelpText = "Reflection dlls with Component classes" )]
		public String DllPaths { get; set; }

		[Option( "GeneratePath", HelpText = "Where to place Generated folder. WARNING: removes and creates Generated folder on each run" )]
		public String GeneratePath { get; set; }

		[Option( "GeneratedNamespace", Default = "", HelpText = "do not add namespace to component name" )]
		public String GeneratedNamespace { get; set; }

		[Option( "DryRun", HelpText = "Do not remove or write anything on disk. WARNING: plugin devs should handle this option" )]
		public Boolean DryRun { get; set; }
	}

	internal class Program
	{
		public static void Main( string[] args )
		{
			Parser.Default.ParseArguments<Args>(args).WithParsed( Run );
		}

		public static void Run( Args args )
		{
			Settings settings		= null;
			if ( String.IsNullOrEmpty( args.SettingsPath )  )
			{
				settings						= new Settings( );
				settings.IgnoreNamespaces		= args.IgnoreNamespaces;
				settings.DllPaths				= args.DllPaths;
				settings.GeneratedNamespace		= args.GeneratedNamespace;
				settings.GeneratePath			= args.GeneratePath;
				settings.DryRun					= args.DryRun;
			}
			else
			{
				if ( !File.Exists( args.SettingsPath ) )
				{
					Console.WriteLine( $"Settings file does not exist: '{args.SettingsPath}'\nTry --help" );
					return;
				}
				var settingsStr = File.ReadAllText( args.SettingsPath );
				settings = SettingsGrammar.SettingsParser.Parse( settingsStr );
			}

			if ( !Directory.Exists( settings.GeneratePath ) )
			{
				Console.WriteLine( $"Generate path does not exist: '{settings.GeneratePath}'\nTry --help" );
				return;
			}

			var runner = new Runner();
			runner.Init(  );

			var contexts = runner.Contexts;
			FillContexts( contexts, settings );

			runner.Systems.Initialize(  );
			runner.Systems.Execute(  );
			runner.Systems.Cleanup(  );
			runner.Systems.TearDown(  );
		}

		private static void FillContexts( Contexts contexts, Settings settings )
		{
			contexts.settings.isConsoleWriteLineGeneratedPaths		= true;
			contexts.settings.isIgnoreNamespaces					= settings.IgnoreNamespaces;
			contexts.settings.isRunInDryMode						= settings.DryRun;
			contexts.settings.SetGeneratePath( String.IsNullOrEmpty( settings.GeneratePath ) ? "./" : settings.GeneratePath );
			contexts.settings.SetGeneratedNamespace( settings.GeneratedNamespace );
			contexts.settings.SetReflectionAssemblyPaths( settings.DllPaths.Split(',').ToList(  ) );
		}
	}
}