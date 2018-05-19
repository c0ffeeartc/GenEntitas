using System;
using System.Collections.Generic;
using NSpec;
using GenEntitas;
using Sprache;

namespace Tests.Tests.GenEntitasSprache
{
	public class describe_SettingsParser : nspec
	{
		private				void					test_KvParser(  )
		{
			new Each<String, String, String, Boolean>
			{
				{
					"K = V",
					"K",
					"V",
					false
				},
				{
					"K _ V",
					"K",
					"V",
					true
				},
			}.Do( ( given, expectK, expectV, throws )
			=> it["parses Kv"] = (  ) =>
			{
				if ( throws )
				{
					expect<Exception>( (  )=> { SettingsGrammar.KvParser.Parse( given ); } );
					return;
				}
				var result = SettingsGrammar.KvParser.Parse( given );

				result.Key.should_be( expectK );
				result.Value.should_be( expectV );
			} );
		}

		private				void					test_DictParser(  )
		{
			new Each<String, Dictionary<String, String>, Boolean>
			{
				{
					"K = V\nA = B\nC = D",
					new Dictionary<String,String>
					{
						{ "K","V"},
						{ "A","B"},
						{ "C","D"},
					},
					false
				},
			}.Do( ( given, expectKv, throws )
			=> it["parses Dict. given = \n{0}\n, throws = {1}".With( given, throws )] = (  ) =>
			{
				if ( throws )
				{
					expect<Exception>( (  )=> { SettingsGrammar.DictParser.Parse( given ); } );
					return;
				}
				var result = SettingsGrammar.DictParser.Parse( given );
				foreach ( var kv in expectKv )
				{
					result.should_contain( kv );
				}
			} );
		}

		private				void					test_SettingsStr(  )
		{
			new Each<String, Settings, Boolean>
			{
				{
@"GeneratePath = path
IgnoreNamespaces = false"
					, new Settings()
						{ IgnoreNamespaces =		false
						, GeneratePath =			"path"
						}
					, false
				},

				{
@"
IgnoreNamespaces = false
GeneratePath = path"
					, new Settings()
						{ IgnoreNamespaces =		false
						, GeneratePath =			"path"
						}
					, false
				},

				{
@"GeneratePath = path
IgnoreNamespaces = true"
					, new Settings()
						{ IgnoreNamespaces =		true
						, GeneratePath =			"path"
						}
					, false
				},

				{

@"IgnoreNamespaces = true
GeneratePath = path"
					, new Settings()
						{ IgnoreNamespaces =		true
						, GeneratePath =			"path"
						}
					, false
				},

			}.Do( ( given, expected, isWaitException )
			=> it["parse ignore namespace. IsThrow = {2}, Given =\n{0}\n".With( given, expected, isWaitException )] = (  ) =>
			{
				if ( isWaitException )
				{
					expect<Exception>( (  )=> { SettingsGrammar.SettingsParser.Parse( given ); } );
					return;
				}
				var result = SettingsGrammar.SettingsParser.Parse( given );

				result.IgnoreNamespaces.should_be( expected.IgnoreNamespaces );
				result.GeneratePath.should_be( expected.GeneratePath );
			} );
		}
	}
}