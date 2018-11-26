using System;
using System.Collections.Generic;
using FluentAssertions;
using NSpec;
using GenEntitas;
using Sprache;

namespace Tests.Tests.GenEntitasSprache
{
	public class describe_SettingsParser : nspec
	{
		private				void					test_BoolParser(  )
		{
			new Each<String, Boolean, Boolean>
			{
				{ "TruE", true, false },
				{ "true", true, false },
				{ "truu", false, true },
				{ "truez", false, true },
				{ "false", false, false },
				{ "falSe", false, false },
				{ "false1", false, true },
				{ "falsea", false, true },
			}.Do( ( given, expected, throws )
			=> it["parses Bool. throws {2}, given {0}, expected {1}".With( given, expected, throws )] = (  ) =>
			{
				if ( throws )
				{
					Action act = (  )=> { SettingsGrammar.BoolFromStr( given ); };
					act.Should(  ).Throw<Exception>(  );
					return;
				}
				var result = SettingsGrammar.BoolFromStr( given );

				result.should_be( expected );
			} );
		}

		private				void					test_KStrVListStrParser(  )
		{
			Contexts contexts	= null;
			before				= (  ) =>
			{
				contexts		= new Contexts(  );
			};

			new Each<String, String, List<String>, Boolean>
			{
				{
					"K = V",
					"K",
					new List<String>{ "V" },
					true
				},
				{
					"K _ V",
					"K",
					new List<String>{ "V" },
					true
				},
				{
					"K = \"V\"",
					"K",
					new List<String>{ "V" },
					false
				},
				{
					@"K = ""ABC"", ""DEF""",
					"K",
					new List<String>{ "ABC", "DEF" },
					false
				},
				{
					@"K =
					""ABC"",
					""DEF"",
					",
					"K",
					new List<String>{ "ABC", "DEF" },
					false
				},
			}.Do( ( given, expectK, expectV, throws )
			=> it["parses Kv"] = (  ) =>
			{
				var parsers		= new SettingsGrammar( contexts );
				var parser		= parsers.KStrVListStr;
				if ( throws )
				{
					Action act = (  )=> { parser.Parse( given ); };
					act.Should(  ).Throw<Exception>(  );
					return;
				}
				var result = parser.Parse( given );

				result.Key.should_be( expectK );
				foreach ( var v in expectV )
				{
					result.Value.should_contain( v );
				}
				result.Value.Count.should_be( expectV.Count );
			} );
		}

		private				void					test_DictParser(  )
		{
			Contexts contexts	= null;
			before				= (  ) =>
			{
				contexts		= new Contexts(  );
			};

			new Each<String, Dictionary<String, List<String>>, Boolean>
			{
				{
					"K = \"V\"\nA = \"B\"\nC = \"D\"",
					new Dictionary<String,List<String>>
					{
						{ "K", new List<String>{ "V" }},
						{ "A",new List<String>{ "B" }},
						{ "C",new List<String>{ "D" }},
					},
					false
				},
			}.Do( ( given, expectKv, throws )
			=> it["parses Dict. given = \n{0}\n, throws = {1}".With( given, throws )] = (  ) =>
			{
				var parsers		= new SettingsGrammar( contexts );
				var parser		= parsers.DictParser;
				if ( throws )
				{
					Action act = (  )=> { parser.Parse( given ); };
					act.Should(  ).Throw<Exception>(  );
					return;
				}
				var result = parser.Parse( given );

				foreach ( var kv in expectKv )
				{
					result.should_contain( kv );
				}
			} );
		}

		private				void					test_SettingsStr(  )
		{
			Contexts contexts	= null;
			before				= (  ) =>
			{
				contexts		= new Contexts(  );
			};

			new Each<String, Boolean, String, Boolean>
			{
				{
@"IgnoreNamespaces = ""true""
ReflectionAssemblyPaths =
	""path1"",
	""path2"",
GeneratePath = ""path""
",
					true , "path" , false
				},
				{
@"IgnoreNamespaces = ""true""
ReflectionAssemblyPaths =
	""path1"",
	""path2""
GeneratePath = ""path""
",
					true , "path" , false
				},

			}.Do( ( given, ignoreNamespaces, generatePath, throws )
			=> it["parse ignore namespace. IsThrow = {2}, Given =\n{0}\n".With( given, generatePath, throws )] = (  ) =>
			{
				var parsers		= new SettingsGrammar( contexts );
				var parser		= parsers.SettingsParser;
				if ( throws )
				{
					Action act = (  )=> { parser.Parse( given ); };
					act.Should(  ).Throw<Exception>(  );
					return;
				}

				parser.Parse( given );

				contexts.settings.isIgnoreNamespaces.should_be( ignoreNamespaces );
				contexts.settings.generatePath.Value.should_be( generatePath );
				contexts.settings.reflectionAssemblyPaths.Values.should_contain( "path1" );
				contexts.settings.reflectionAssemblyPaths.Values.should_contain( "path2" );
				contexts.settings.reflectionAssemblyPaths.Values.Count.should_be( 2 );
			} );
		}
	}
}