using System;
using CommandLine;

namespace GenEntitas
{
	public class Args
	{
		[Option( "SettingsPath", Required = true, Default = "", HelpText = "path to settings file. WARNING: If file is provided other command line options are ignored" )]
		public String SettingsPath { get; set; }
	}

	internal class Program
	{
		public static void Main( string[] args )
		{
			Parser.Default.ParseArguments<Args>(args).WithParsed( Run );
		}

		public static void Run( Args args )
		{
			var runner = new Runner();
			runner.Init(  );
			var contexts = runner.Contexts;

			contexts.settings.SetSettingsPath( args.SettingsPath );
			contexts.settings.isConsoleWriteLineGeneratedPaths		= true;

			runner.Systems.Initialize(  );
			runner.Systems.Execute(  );
			runner.Systems.Cleanup(  );
			runner.Systems.TearDown(  );
		}
	}
}