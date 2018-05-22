using System;
using System.Collections.Generic;
using System.Linq;
using Entitas;
using GenEntitas;
using Sprache;

namespace GenEntitasLang
{
	public class GenEntitasLangParser
	{
		public				GenEntitasLangParser	( Contexts contexts )
		{
			_contexts = contexts;
			for ( var i = 0; i < _contexts.main.contextInfo.componentTypes.Length; i++ )
			{
				_mainTypesToI[_contexts.main.contextInfo.componentTypes[i]] = i;
			}

			Identifier = 
				from first in Parse.Letter.Once()
				from rest in Parse.LetterOrDigit.XOr(Parse.Char('-')).XOr(Parse.Char('_')).Many()
				select new string(first.Concat(rest).ToArray());

			QuotedString =
				from openQuote in Parse.Char( '"' )
				from content in Parse.AnyChar.Except( Parse.Char( '"' ) ).Many(  )
				from closeQuote in Parse.Char( '"' )
				select new String( content.ToArray(  ) );

			QuotedIdentifier =
				from openQuote in Parse.Char( '"' )
				from content in Identifier
				from closeQuote in Parse.Char( '"' )
				select new String( content.ToArray(  ) );

			AliasGet = 
				( from aliasKey in Identifier
				select aliasKey )
				.Select( key =>
				{
					if ( !_contexts.main.hasAliasComp || !_contexts.main.aliasComp.Values.ContainsKey( key ) )
					{
						throw new KeyNotFoundException( $"Alias '{key}' not found" );
					}
					return _contexts.main.aliasComp.Values[key];
				} );

			CompContextNames =
				from contextsKeyword in Parse.String( "in" )
				from ws in Parse.WhiteSpace.AtLeastOnce(  )
				from contextNames in Identifier.DelimitedBy( Parse.Char( ',' ).Token(  ) )
				select new ContextNamesComp
				{
					Values = contextNames.ToList(  )
				};

			FieldInfo =
				from fieldName in Identifier
				from colon in Parse.Char( ':' ).Token(  )
				from typeName in AliasGet.Or( QuotedString )
				select new FieldInfo( typeName, fieldName );

			CompPublicFields =
				from publicFieldsKeyword in Parse.String( "publicFields" )
				from colon in Parse.Char( ':' ).Token(  )
				from fieldInfos in FieldInfo.Token(  ).Many(  )
				select new PublicFieldsComp
					{
						Values = fieldInfos.ToList(  )
					};

			CompUnique =
				from uniqueKeyword in Parse.String( "unique" ).Token(  )
				select new UniqueComp(  );

			CompParam =
				CompContextNames
				.Or( CompPublicFields )
				.Or( CompUnique )
				;

			CompEnt = 
				from compKeyword in Parse.String( "comp" ).Token(  )
				from id in Identifier.Token(  )
				from comps in CompParam.Many(  ).Optional(  )
				select AddComp( _contexts, id, comps.GetOrElse( null ) );

			Alias =
				from ws11 in Parse.WhiteSpace.Many(  )
				from aliasKeyword in Parse.String( "alias" )
				from ws in Parse.WhiteSpace.AtLeastOnce(  )
				from id in Identifier.Token(  )
				from colon in Parse.Char( ':' ).Token(  )
				from str in QuotedString.Token(  )
				select AddAlias( _contexts, id, str );

			AliasBlock =
				from aliasList in Alias.Many(  )
				select AddAliasBlock( _contexts );

		}

		public readonly		Parser<MainEntity>		CompEnt;
		public readonly		Parser<IComponent>		CompParam;
		public readonly		Parser<IComponent>		CompContextNames;
		public readonly		Parser<IComponent>		CompUnique;
		public readonly		Parser<IComponent>		CompPublicFields;
		public readonly		Parser<FieldInfo>		FieldInfo;
		public readonly		Parser<KeyValuePair<String,String>>	IdToValue;
		public readonly		Parser<String>			Identifier;
		public readonly		Parser<String>			QuotedString;
		public readonly		Parser<String>			QuotedIdentifier;
		public readonly		Parser<MainEntity>		Alias;
		public readonly		Parser<String>			AliasGet;
		public readonly		Parser<MainEntity>		AliasBlock;
		private				Int32					_parseCompId;
		private				Contexts				_contexts;
		private				Dictionary<Type,Int32>	_mainTypesToI			= new Dictionary<Type, Int32>(  );

		private				MainEntity				AddComp					( Contexts contexts, String id, IEnumerable<IComponent> comps )
		{
			_parseCompId++;
			var ent = _contexts.main.CreateEntity(  );
			ent.AddComp( id, id );
			if ( comps == null )
			{
				return ent;
			}
			foreach ( var comp in comps )
			{
				if ( comp == null )
				{
					Console.WriteLine( "comp == null" );
					continue;
				}
				var compI = _mainTypesToI[comp.GetType(  )];
				ent.AddComponent( compI, comp );
			}
			return ent;
		}

		private				MainEntity				AddAlias				( Contexts contexts, String key, String value )
		{
			if ( !contexts.main.hasAliasComp )
			{
				contexts.main.SetAliasComp( new Dictionary<String, String>(  ) );
			}

			var values			= _contexts.main.aliasComp.Values;
			values[key]			= value;
			_contexts.main.ReplaceAliasComp( values );
			return _contexts.main.aliasCompEntity;
		}

		private				MainEntity				AddAliasBlock			( Contexts contexts )
		{
			return contexts.main.aliasCompEntity;
		}

	}
}