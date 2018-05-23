using System;
using CommandLine;

namespace GenEntitas
{
	public class Args
	{
		[Option( "SettingsPath", Required = true, HelpText = "Path to settings file" )]
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