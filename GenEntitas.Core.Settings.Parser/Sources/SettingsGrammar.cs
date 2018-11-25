using System;
using System.Collections.Generic;
using System.Linq;
using Sprache;

namespace GenEntitas
{
	public class SettingsGrammar
	{
		public					SettingsGrammar			( Contexts contexts )
		{
			_contexts = contexts;

			Identifier =
			(	from first in Parse.Letter.Or( Parse.Char( '_' ) ).Once()
				from rest in Parse.LetterOrDigit.Or( Parse.Char( '_' ) ).Many().Text()
				select new String ( first.Concat( rest).ToArray(  ) )
			).Token(  );

			KvParser	=
				(	from id in Identifier
					from delim in Parse.Char( '=' ).Token(  )
					from value in Parse.AnyChar.Except( Parse.LineEnd.Or( Parse.LineTerminator ) ).Many(  ).Text(  )
					select new KeyValuePair<String, String>( id, value )
				).Token(  );

			DictParser	=
				from kv in KvParser.Many(  )
				select kv.ToDictionary( k => k.Key, k => k.Value );

			SettingsParser			=
				( 	from dict in DictParser
					select dict )
				.Select( d =>
				{
					if ( d.ContainsKey( nameof( LogGeneratedPaths ) ) )
					{
						_contexts.settings.isLogGeneratedPaths = BoolFromStr( d[nameof( LogGeneratedPaths )] );
					}
					else
					{
						_contexts.settings.isLogGeneratedPaths = true;
					}

					if ( d.ContainsKey( nameof( IgnoreNamespaces ) ) )
					{
						_contexts.settings.isIgnoreNamespaces = BoolFromStr( d[nameof( IgnoreNamespaces )] );
					}
					else
					{
						_contexts.settings.isIgnoreNamespaces = false;
					}

					if ( d.ContainsKey( nameof( RunInDryMode ) ) )
					{
						_contexts.settings.isRunInDryMode = BoolFromStr( d[nameof( RunInDryMode )] );
					}
					else
					{
						_contexts.settings.isRunInDryMode = false;
					}

					_contexts.settings.ReplaceGeneratedNamespace( d.ContainsKey( nameof( GeneratedNamespace ) )
						? d[nameof( GeneratedNamespace )]
						: "" );

					_contexts.settings.ReplaceRoslynPathToSolution( d.ContainsKey( nameof( RoslynPathToSolution ) )
						? d[nameof( RoslynPathToSolution )]
						: "" );

					_contexts.settings.ReplaceGeneratePath( d.ContainsKey( nameof( GeneratePath ) )
						? d[nameof( GeneratePath )]
						: "" );

					_contexts.settings.ReplaceReflectionAssemblyPaths( d.ContainsKey( nameof( ReflectionAssemblyPaths ) )
						? d[nameof( ReflectionAssemblyPaths )].Split(',').ToList(  )
						: new List<string>(  ) );

					_contexts.settings.ReplaceGenEntitasLangPaths( d.ContainsKey( nameof( GenEntitasLangPaths ) )
						? d[nameof( GenEntitasLangPaths )].Split(',').ToList(  )
						: new List<string>(  ) );

					_contexts.settings.ReplaceSearchPaths( d.ContainsKey( nameof( SearchPaths ) )
						? d[nameof( SearchPaths )].Split(',').ToList(  )
						: new List<string>(  ) );

					_contexts.settings.ReplaceWriteGeneratedPathsToCsProj( d.ContainsKey( nameof( WriteGeneratedPathsToCsProj ) )
						? d[nameof( WriteGeneratedPathsToCsProj )]
						: "" );

					return _contexts;
				} );
		}

		public readonly			Parser<String>			Identifier;
		public readonly		Parser<KeyValuePair<String,String>>	KvParser;
		public readonly		Parser<Dictionary<String,String>>	DictParser;
		public readonly			Parser<Contexts>		SettingsParser;
		private					Contexts				_contexts;

		public static			Boolean					BoolFromStr				( String s )
		{
			if ( String.Compare( s, "true", StringComparison.OrdinalIgnoreCase ) == 0 )
			{
				return true;
			}

			if ( String.Compare( s, "false", StringComparison.OrdinalIgnoreCase ) == 0 )
			{
				return false;
			}

			throw new ParseException( $"Can't parse \"{s}\" to type bool" );
		}
	}
}