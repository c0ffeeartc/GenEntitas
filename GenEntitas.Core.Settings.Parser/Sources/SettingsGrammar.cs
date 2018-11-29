using System;
using System.Collections.Generic;
using System.Linq;
using Sprache;

namespace GenEntitas
{
	public class SettingsGrammar : ISettingsService
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
		public					CommentParser			Comments;
		public					Parser<String>			RemoveCommentsParser;
		private					Contexts				_contexts;

		public		Dictionary<String,List<String>>		Parse					( String str )
		{
			var withoutComments = RemoveCommentsParser.Parse( str );
			return DictParser.Parse( withoutComments );
		}

		public					Boolean					BoolFromStr				( String s )
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