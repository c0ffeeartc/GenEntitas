using System;
using System.Collections.Generic;
using GenEntitas;
using GenEntitasLang;
using NSpec;
using Sprache;
using FluentAssertions;

namespace Tests.Tests
{
	public class describe_GenEntitasLang : nspec
	{
		private				void					test_Parsers			(  )
		{
			Contexts contexts = null;
			before				= ()=>
			{
				contexts	= new Contexts();
			};

			new Each<String,String, Boolean>
			{
				{ "A", "A", false },
				{ "_abc", "_abc", true },
				{ "abc_", "abc_", false },
				{ "1abc", "1abc", true },
				{ "abc_1", "abc_1", false },
			}.Do( ( given, expected, throws ) => {
			it["parses identifier"] = ()=>
			{
				var parser = new GenEntitasLangParser( contexts );
				if ( throws )
				{
					Action act = (  ) => { parser.Identifier.Parse( given ); };
					act.Should(  ).Throw<ParseException>(  );
					return;
				}

				var result = parser.Identifier.Parse( given );
				result.should_be( expected );
			}; } );

			new Each<String, String, String, Boolean>
			{
				{ "alias int : \"System.Int32\"", "int", "System.Int32", false },
				{ "alias a : \"b\"", "a", "b", false },
				{ " 	alias 	a 	: 	\"b\" 	", "a", "b", false },
				{ "alias a:\"b\"", "a", "b", false },
				{ "aliasa : \"b\"", "a", "b", true },
			}.Do( ( given, expectKey, expectValue, throws ) => {
			it["parses alias"] = (  ) =>
			{
				var parser = new GenEntitasLangParser( contexts );
				if ( throws )
				{
					Action act = (  ) => { parser.Alias.Parse( given ); };
					act.Should(  ).Throw<ParseException>(  );
					return;
				}

				var ent = parser.Alias.Parse( given );
				ent.hasAliasComp.should_be_true(  );
				ent.aliasComp.Values.should_contain( new KeyValuePair<string, string>( expectKey, expectValue ) );
			}; } );

			new Each<String, Dictionary<String, String>, Boolean>
			{
				{
					@"alias a : ""1""
					b : ""2""
					alias c : ""3""",
					new Dictionary<String, String>
					{
						{ "a", "1" },
						{ "b", "2" },
						{ "c", "3" },
					},
  					false
				},
			}.Do( ( given, expected, throws ) => {
			it["parses alias block"] = () =>
			{
				var parser = new GenEntitasLangParser( contexts );
				if ( throws )
				{
					Action act = (  )=> { parser.AliasBlock.Parse( given ); };
					act.Should(  ).Throw<ParseException>(  );
					return;
				}

				var ent = parser.AliasBlock.Parse( given );
				ent.hasAliasComp.should_be_true(  );
				foreach ( var kv in expected )
				{
					ent.aliasComp.Values.should_contain( kv );
				}

			}; } );

			new Each<String, String, String, Boolean>
			{
				{
					@"alias abc : ""1"" alias b : ""2"" alias c : ""3"" ",
					"c",
					"3",
  					false
				},
				{
					@"alias abc : ""1"" alias b : ""2"" alias c : ""3"" ",
					"abc",
					"1",
  					false
				},
				{
					@"alias abc : ""1"" alias b : ""2"" alias c : ""3"" ",
					"z",
					"1",
  					true
				},
			}.Do( ( given, expectKey, expectValue, throws ) => {
			it["parses parses AliasGet, gets alias value"] = () =>
			{
				given += expectKey;
				var parsers = new GenEntitasLangParser( contexts );
				Parser<String> parser =
					from some in parsers.AliasBlock
					from aliasValue in parsers.AliasGet
					select aliasValue;

				if ( throws )
				{
					Action act = (  )=> { parser.Parse( given ); };
					act.Should(  ).Throw<Exception>(  );
					return;
				}

				parser.Parse( given ).should_be( expectValue );
			}; } );

			new Each<String, ContextNamesComp, Boolean>
			{
				{
					"in First, Second",
					new ContextNamesComp
					{
						Values = new List<String>(  )
						{
							"First",
							"Second",
						}
					}, 
  					false
				},
				{
					"in 1First , Second",
					new ContextNamesComp
					{
						Values = new List<String>(  )
						{
							"First",
							"Second",
						}
					}, 
  					true
				},
			}.Do( ( given, expected, throws ) => {
			it["parses contexts comp param"] = () =>
			{
				var parsers			= new GenEntitasLangParser( contexts );
				var parser			= parsers.CompContextNames;

				if ( throws )
				{
					Action act = (  )=> { parser.Parse( given ); };
					act.Should(  ).Throw<Exception>(  );
					return;
				}

				var result = parser.Parse( given ) as ContextNamesComp;
				result.should_not_be_null(  );
				result.Values.Count.should_be( expected.Values.Count );
				for ( var i = 0; i < result.Values.Count; i++ )
				{
					var contextName = result.Values[i];
					contextName.should_be( expected.Values[i] );
				}
			}; } );

			new Each<String, PublicFieldsComp, Boolean>
			{
				{
					@"publicFields :
						a : ""1""
						b : ""2""",
					new PublicFieldsComp(  )
					{
						Values = new List<FieldInfo>(  )
						{
							new FieldInfo( "1", "a" ),
							new FieldInfo( "2", "b" ),
						}
					}, 
  					false
				},
				{
					"publicFields :",
					new PublicFieldsComp{ }, 
  					true
				},
			}.Do( ( given, expected, throws ) => {
			it["parses public fields comp"] = () =>
			{
				var parsers			= new GenEntitasLangParser( contexts );
				var parser			= parsers.CompPublicFields;

				if ( throws )
				{
					Action act = (  )=> { parser.Parse( given ); };
					act.Should(  ).Throw<Exception>(  );
					return;
				}

				var result = parser.Parse( given ) as PublicFieldsComp;
				result.should_not_be_null(  );
				result.Values.Count.should_be( expected.Values.Count );
				for ( var i = 0; i < result.Values.Count; i++ )
				{
					var fieldInfo = result.Values[i];
					fieldInfo.TypeName.should_be( expected.Values[i].TypeName );
					fieldInfo.FieldName.should_be( expected.Values[i].FieldName );
				}
			}; } );

			new Each<String,String, Boolean>
			{
				{ "comp Destroy", "Destroy", false },
				{ " 	comp 	Destroy 	", "Destroy", false },
				{ "compDestroy", "Destroy", true },
			}.Do( ( given, expected, throws ) => {
			it["parses comp"] = () =>
			{
				var parser		= new GenEntitasLangParser( contexts );
				if ( throws )
				{
					Action act = (  ) => { parser.CompEnt.Parse( given ); };
					act.Should(  ).Throw<ParseException>(  );
					return;
				}

				var ent			= parser.CompEnt.Parse( given );
				ent.hasComp.should_be_true(  );
				ent.comp.FullTypeName.should_be( expected );
				ent.comp.Name.should_be( expected );
			}; } );

			new Each<String, Boolean>
			{
				{
					@"comp Destroy in First, Second
							unique
							publicFields :
								x : ""int""
								y : ""float"""
					, false
				},
				{
					@"comp Destroy in First, Second
							unique
							public Fields :
								x : ""int""
								y : ""float"""
					, true
				},
			}.Do( ( given, throws ) => {
			it["parses comp 2"] = () =>
			{
				var parser		= new GenEntitasLangParser( contexts );
				if ( throws )
				{
					Action act = (  ) => { parser.CompEnt.Parse( given ); };
					act.Should(  ).Throw<ParseException>(  );
					return;
				}

				var ent			= parser.CompEnt.Parse( given );

				ent.hasComp.should_be_true(  );
				ent.comp.FullTypeName.should_be( "Destroy" );
				ent.comp.Name.should_be( "Destroy" );

				ent.hasContextNamesComp.should_be_true(  );
				ent.contextNamesComp.Values.should_contain( "First" );
				ent.contextNamesComp.Values.should_contain( "Second" );

				ent.isUniqueComp.should_be_true(  );

				ent.hasPublicFieldsComp.should_be_true(  );
				ent.publicFieldsComp.Values.Count.should_be( 2 );
				ent.publicFieldsComp.Values[0].FieldName.should_be( "x" );
				ent.publicFieldsComp.Values[0].TypeName.should_be( "int" );
				ent.publicFieldsComp.Values[1].FieldName.should_be( "y" );
				ent.publicFieldsComp.Values[1].TypeName.should_be( "float" );
			}; } );
		}

		private				void					test_ParseCompBlock			(  )
		{
			Contexts contexts = null;
			before				= ()=>
			{
				contexts	= new Contexts();
			};

			new Each<String, Boolean>
			{
				{
					@"comp A in First
							unique
							publicFields :
								x : ""int""
								y : ""float""

					comp B in Second
							publicFields :
								x : ""int""
								y : ""float"""
					, false
				},
			}.Do( ( given, throws ) => {
			it["parses comp block"] = () =>
			{
				var parsers		= new GenEntitasLangParser( contexts );
				var parser		= parsers.CompBlock;
				if ( throws )
				{
					Action act = (  ) => { parser.Parse( given ); };
					act.Should(  ).Throw<ParseException>(  );
					return;
				}

				var ents			= parser.Parse( given );

				var hasCheckedUnique = false;
				foreach ( var ent in ents )
				{
					if ( !hasCheckedUnique && ent.isUniqueComp )
					{
						hasCheckedUnique	= true;
						ent.hasComp.should_be_true(  );
						ent.comp.FullTypeName.should_be( "A" );
						ent.comp.Name.should_be( "A" );

						ent.hasContextNamesComp.should_be_true(  );
						ent.contextNamesComp.Values.should_contain( "First" );

						ent.isUniqueComp.should_be_true(  );

						ent.hasPublicFieldsComp.should_be_true(  );
						ent.publicFieldsComp.Values.Count.should_be( 2 );
						ent.publicFieldsComp.Values[0].FieldName.should_be( "x" );
						ent.publicFieldsComp.Values[0].TypeName.should_be( "int" );
						ent.publicFieldsComp.Values[1].FieldName.should_be( "y" );
						ent.publicFieldsComp.Values[1].TypeName.should_be( "float" );
					}
					else
					{
						ent.hasComp.should_be_true(  );
						ent.comp.FullTypeName.should_be( "B" );
						ent.comp.Name.should_be( "B" );

						ent.hasContextNamesComp.should_be_true(  );
						ent.contextNamesComp.Values.should_contain( "Second" );

						ent.isUniqueComp.should_be_false(  );

						ent.hasPublicFieldsComp.should_be_true(  );
						ent.publicFieldsComp.Values.Count.should_be( 2 );
						ent.publicFieldsComp.Values[0].FieldName.should_be( "x" );
						ent.publicFieldsComp.Values[0].TypeName.should_be( "int" );
						ent.publicFieldsComp.Values[1].FieldName.should_be( "y" );
						ent.publicFieldsComp.Values[1].TypeName.should_be( "float" );
					}
				}
			}; } );

		}

		private				void					test_ParsesComment		(  )
		{
			Contexts contexts = null;
			before				= ()=>
			{
				contexts	= new Contexts();
			};

			new Each<String, String, Boolean>
			{
				{
@"1 2/*3 4*//* zzz */ 5
 6// a // b
7",
@"1 2 5
 6
7",
					false
				},
			}.Do( ( given, expected, throws ) => {
			it["parses comment.\ngiven \n{0}\nexpected \n{1}".With( given, expected )] = (  ) =>
			{
				var parsers		= new GenEntitasLangParser( contexts );
				var parser		= parsers.RemoveCommentsParser;
				if ( throws )
				{
					Action act = (  ) => { parser.Parse( given ); };
					act.Should(  ).Throw<ParseException>(  );
					return;
				}

				var result = parser.Parse( given );
				result.should_be( expected );
			}; } );
		}

		private				void					test_ParsesRoot			(  )
		{
			Contexts contexts = null;
			before				= ()=>
			{
				contexts	= new Contexts();
			};

			new Each<String, Boolean>
			{
				{
					@"	alias integer : ""int""
						alias single : ""float""
						alias double : ""double""
						/*alias x : ""0""
						alias y : ""1""*/
						//alias z : ""2""

					//comp Commented in First

					comp A in First
							unique
							publicFields :
								x : integer
								y : single

					comp B in Second
							publicFields :
								x : ""int""
								y : ""double"""
					, false
				},
			}.Do( ( given, throws ) => {
			it["parses root"] = () =>
			{
				var parsers		= new GenEntitasLangParser( contexts );
				if ( throws )
				{
					Action act = (  ) => { parsers.ParseWithComments( given ); };
					act.Should(  ).Throw<ParseException>(  );
					return;
				}

				parsers.ParseWithComments( given );

				contexts.main.hasAliasComp.should_be_true(  );
				var aliases = contexts.main.aliasComp.Values;
				aliases.Count.should_be( 3 );

				var ents = contexts.main.GetGroup( MainMatcher.Comp ).GetEntities(  );
				ents.Length.should_be( 2 );

				{
					var ent				= ents[0].isUniqueComp ? ents[0] : ents[1];
					ent.hasComp.should_be_true(  );
					ent.comp.FullTypeName.should_be( "A" );
					ent.comp.Name.should_be( "A" );

					ent.hasContextNamesComp.should_be_true(  );
					ent.contextNamesComp.Values.should_contain( "First" );

					ent.isUniqueComp.should_be_true(  );

					ent.hasPublicFieldsComp.should_be_true(  );
					ent.publicFieldsComp.Values.Count.should_be( 2 );
					ent.publicFieldsComp.Values[0].FieldName.should_be( "x" );
					ent.publicFieldsComp.Values[0].TypeName.should_be( "int" );
					ent.publicFieldsComp.Values[1].FieldName.should_be( "y" );
					ent.publicFieldsComp.Values[1].TypeName.should_be( "float" );
				}

				{
					var ent				= ents[0].isUniqueComp ? ents[1] : ents[0];
					ent.hasComp.should_be_true(  );
					ent.comp.FullTypeName.should_be( "B" );
					ent.comp.Name.should_be( "B" );

					ent.hasContextNamesComp.should_be_true(  );
					ent.contextNamesComp.Values.should_contain( "Second" );

					ent.isUniqueComp.should_be_false(  );

					ent.hasPublicFieldsComp.should_be_true(  );
					ent.publicFieldsComp.Values.Count.should_be( 2 );
					ent.publicFieldsComp.Values[0].FieldName.should_be( "x" );
					ent.publicFieldsComp.Values[0].TypeName.should_be( "int" );
					ent.publicFieldsComp.Values[1].FieldName.should_be( "y" );
					ent.publicFieldsComp.Values[1].TypeName.should_be( "double" );
				}
			}; } );

		}
	}
}