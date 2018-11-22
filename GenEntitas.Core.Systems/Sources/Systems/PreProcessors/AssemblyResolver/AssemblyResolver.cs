using System;
using System.IO;
using System.Reflection;

namespace GenEntitas
{
	public class AssemblyResolver
	{
		public				AssemblyResolver		( string[] basePaths )
		{
			AppDomain.CurrentDomain.AssemblyResolve += OnAssemblyResolve;
			_basePaths			= basePaths;
		}

		private readonly	string[]				_basePaths;

		private				Assembly				OnAssemblyResolve		( object sender, ResolveEventArgs args )
		{
			Assembly assembly	= null;
			try
			{
				Console.WriteLine( "  Loading: " + args.Name );
				assembly		= Assembly.LoadFrom(new AssemblyName(args.Name).Name);
			}
			catch( Exception )
			{
				var name		= new AssemblyName(args.Name).Name;
				if ( !name.EndsWith(".dll", StringComparison.Ordinal )
					&& !name.EndsWith( ".exe", StringComparison.Ordinal ) )
				{
					name		+= ".dll";
				}

				var path		= ResolvePath( name );
				if ( path != null )
				{
					assembly	= Assembly.LoadFrom(path);
				}
			}

			return assembly;
		}

		private				String					ResolvePath				( String assemblyName )
		{
			foreach ( var basePath in _basePaths )
			{
				var path	= basePath + Path.DirectorySeparatorChar + assemblyName;
				if ( File.Exists( path ) )
				{
					Console.WriteLine( "	Resolved: " + path );
					return path;
				}
			}

			Console.WriteLine( "	Could not resolve: " + assemblyName );
			return null;
		}
	}
}