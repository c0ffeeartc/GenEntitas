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
			InitSettingsParser(  );
			InitCommentsParser(  );
		}

		public					Parser<String>			Identifier;
		public					Parser<String>			QuotedString;
		public		Parser<KeyValuePair<String,List<String>>>	KStrVListStr;
		public		Parser<Dictionary<String,List<String>>>	DictParser;
		public					Parser<Contexts>		SettingsParser;
		public					Parser<Contexts>		SystemGuidsSettingsParser;
		public					CommentParser			Comments;
		public					Parser<String>			RemoveCommentsParser;
		private					Contexts				_contexts;

		public					Contexts				Parse					( String str )
		{
			var withoutComments = RemoveCommentsParser.Parse( str );
			return SettingsParser.Parse( withoutComments );
		}

		public					Contexts				ParseSystemGuids		( String str )
		{
			var withoutComments = RemoveCommentsParser.Parse( str );
			return SystemGuidsSettingsParser.Parse( withoutComments );
		}

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

		private					void					InitSettingsParser		(  )
		{
			Identifier =
			(	from first in Sprache.Parse.Letter.Or( Sprache.Parse.Char( '_' ) ).Once()
				from rest in Sprache.Parse.LetterOrDigit.Or( Sprache.Parse.Char( '_' ) ).Many().Text()
				select new String ( first.Concat( rest).ToArray(  ) )
			).Token(  );

			QuotedString =
				from openQuote in Sprache.Parse.Char( '"' )
				from content in Sprache.Parse.AnyChar.Except( Sprache.Parse.Char( '"' ) ).Many(  )
				from closeQuote in Sprache.Parse.Char( '"' )
				select new String( content.ToArray(  ) );

			KStrVListStr	=
				(	from id in Identifier
					from delim in Sprache.Parse.Char( '=' ).Token(  )
					from values in QuotedString.DelimitedBy( Sprache.Parse.Char( ',' ).Token(  ) ).Token(  )
					from trailingComa in Sprache.Parse.Char( ',' ).Token(  ).Optional(  )
					select new KeyValuePair<String, List<String>>( id, values.ToList(  ) )
				).Token(  );

			DictParser	=
				from kv in KStrVListStr.Many(  )
				select kv.ToDictionary( k => k.Key, k => k.Value );

			SystemGuidsSettingsParser			=
				(	from dict in DictParser
					select dict )
				.Select( d =>
				{
					if ( d.ContainsKey( nameof( SystemGuids ) ) )
					{
						var guids = new List<Guid>(  );
						foreach ( var str in d[ nameof ( SystemGuids )])
						{
							var guid	= new Guid( str );
							guids.Add( guid );
						}
						_contexts.settings.ReplaceSystemGuids( guids ); 
					}
					else
					{
						_contexts.settings.ReplaceSystemGuids( new List<Guid>(  ) ); 
					}
					return _contexts;
				} );

			SettingsParser			=
				( 	from dict in DictParser
					select dict )
				.Select( d =>
				{
					if ( d.ContainsKey( nameof( LogGeneratedPaths ) ) )
					{
						_contexts.settings.isLogGeneratedPaths = BoolFromStr( d[nameof( LogGeneratedPaths )].FirstOrDefault(  ) );
					}
					else
					{
						_contexts.settings.isLogGeneratedPaths = true;
					}

					if ( d.ContainsKey( nameof( IgnoreNamespaces ) ) )
					{
						_contexts.settings.isIgnoreNamespaces = BoolFromStr( d[nameof( IgnoreNamespaces )].FirstOrDefault(  ) );
					}
					else
					{
						_contexts.settings.isIgnoreNamespaces = false;
					}

					if ( d.ContainsKey( nameof( RunInDryMode ) ) )
					{
						_contexts.settings.isRunInDryMode = BoolFromStr( d[nameof( RunInDryMode )].FirstOrDefault(  ) );
					}
					else
					{
						_contexts.settings.isRunInDryMode = false;
					}

					_contexts.settings.ReplaceGeneratedNamespace( d.ContainsKey( nameof( GeneratedNamespace ) )
						? d[nameof( GeneratedNamespace )].FirstOrDefault(  )
						: "" );

					_contexts.settings.ReplaceRoslynPathToSolution( d.ContainsKey( nameof( RoslynPathToSolution ) )
						? d[nameof( RoslynPathToSolution )].FirstOrDefault(  )
						: "" );

					_contexts.settings.ReplaceGeneratePath( d.ContainsKey( nameof( GeneratePath ) )
						? d[nameof( GeneratePath )].FirstOrDefault(  )
						: "" );

					_contexts.settings.ReplaceReflectionAssemblyPaths( d.ContainsKey( nameof( ReflectionAssemblyPaths ) )
						? d[nameof( ReflectionAssemblyPaths )]
						: new List<string>(  ) );

					_contexts.settings.ReplaceGenEntitasLangPaths( d.ContainsKey( nameof( GenEntitasLangPaths ) )
						? d[nameof( GenEntitasLangPaths )]
						: new List<string>(  ) );

					_contexts.settings.ReplaceSearchPaths( d.ContainsKey( nameof( SearchPaths ) )
						? d[nameof( SearchPaths )]
						: new List<string>(  ) );

					_contexts.settings.ReplaceWriteGeneratedPathsToCsProj( d.ContainsKey( nameof( WriteGeneratedPathsToCsProj ) )
						? d[nameof( WriteGeneratedPathsToCsProj )].FirstOrDefault(  )
						: "" );

					return _contexts;
				} );
		}

		private					void					InitCommentsParser		(  )
		{
			Comments = new CommentParser(  );

			var anyCharTillComment =
				Sprache.Parse.AnyChar.Until( Comments.AnyComment ).Text(  );

			var anyCharTillLineEnd =
				from s in Sprache.Parse.AnyChar.Until( Sprache.Parse.LineEnd ).Text(  )
				select s + '\n';

			RemoveCommentsParser =
				( from nonComment in
					anyCharTillComment
					.Or( anyCharTillLineEnd )
					.Or( Sprache.Parse.AnyChar.Many(  ).Text(  ) )
				select nonComment ).Many(  )
				.Select( values => String.Concat( values ) );
		}
	}
}