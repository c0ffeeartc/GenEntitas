﻿using System;
using System.IO;
using System.Linq;
using CommandLine;

namespace GenEntitas
{
	public class Options
	{
		[Option( "ignoreNamespaces", Default = true, HelpText = "do not add namespace to component name" )]
		public Boolean IgnoreNamespaces { get; set; }

		[Option( "dllPaths", Required = true, HelpText = "Reflection dlls with Component classes" )]
		public String DllPaths { get; set; }

		[Option( "generatePath", Required = true, HelpText = "Where to place Generated folder. WARNING: removes and creates Generated folder on each run" )]
		public String GeneratePath { get; set; }

		[Option( "generatedNamespace", Default = "", HelpText = "do not add namespace to component name" )]
		public String GeneratedNamespace { get; set; }

		[Option( "dryMode", HelpText = "Do not remove or write anything on disk. WARNING: plugin devs should handle this option" )]
		public Boolean RunInDryMode { get; set; }
	}

	internal class Program
	{
		public static void Main( string[] args )
		{
			Parser.Default.ParseArguments<Options>(args).WithParsed( Run );
		}

		public static void Run( Options options )
		{
			if ( !Directory.Exists( options.GeneratePath ) )
			{
				Console.WriteLine( $"Generate path does not exist: '{options.GeneratePath}'" );
				return;
			}

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
			contexts.settings.isIgnoreNamespaces					= options.IgnoreNamespaces;
			contexts.settings.isRunInDryMode						= options.RunInDryMode;
			contexts.settings.SetGeneratePath( String.IsNullOrEmpty( options.GeneratePath ) ? "./" : options.GeneratePath );
			contexts.settings.SetGeneratedNamespace( options.GeneratedNamespace );
			contexts.settings.SetReflectionAssemblyPaths( options.DllPaths.Split(',').ToList(  ) );
		}
	}
}