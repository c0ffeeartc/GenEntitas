using System;
using System.Collections.Generic;
using System.Linq;
using Sprache;

namespace GenEntitas
{
	public class SettingsGrammar
	{
		public static readonly	Parser<String>			Identifier				=
			(	from first in Parse.Letter.Once()
				from rest in Parse.LetterOrDigit.Many().Text()
				select new String ( first.Concat( rest).ToArray(  ) )
			).Token(  );

		public static readonly	Parser<KeyValuePair<String,String>>	KvParser	=
			(	from id in Identifier
				from delim in Parse.Char( '=' ).Token(  )
				from value in Parse.AnyChar.Except( Parse.LineEnd.Or( Parse.LineTerminator ) ).Many(  ).Text(  )
				select new KeyValuePair<String, String>( id, value )
			).Token(  );

		public static readonly	Parser<Dictionary<String,String>>	DictParser	=
			from kv in KvParser.Many(  )
			select kv.ToDictionary( k => k.Key, k => k.Value );

		public static readonly	Parser<Settings>		SettingsParser			=
			( 	from dict in DictParser
				select new Settings(
					// ReSharper disable once SimplifyConditionalTernaryExpression
					ignoreNamespaces:		dict.ContainsKey( nameof(Settings.IgnoreNamespaces) )
						? BoolFromStr( dict[nameof(Settings.IgnoreNamespaces)] )
						: false,
					generatePath:			dict.ContainsKey( nameof( Settings.GeneratePath ) )
						? dict[nameof( Settings.GeneratePath )]
						: "",
					dllPaths:			dict.ContainsKey( nameof( Settings.DllPaths ) )
						? dict[nameof( Settings.DllPaths )]
						: "",
					generatedNamespace:		dict.ContainsKey( nameof( Settings.GeneratedNamespace ) )
						? dict[nameof( Settings.GeneratedNamespace )]
						: "",
					// ReSharper disable once SimplifyConditionalTernaryExpression
					dryRun:					dict.ContainsKey( nameof(Settings.DryRun) )
						? BoolFromStr( dict[nameof(Settings.DryRun)] )
						: false
				)
			).Token(  );

		private static			Boolean					BoolFromStr				( String s )
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