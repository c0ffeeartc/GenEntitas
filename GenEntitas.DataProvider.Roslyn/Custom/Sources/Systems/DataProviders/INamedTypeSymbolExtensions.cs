using System;
using System.Collections.Generic;
using System.Linq;
using Entitas.CodeGeneration.Attributes;
using Microsoft.CodeAnalysis;

namespace GenEntitas
{
	public static class INamedTypeSymbolExtensions
	{
		public static		Boolean					Implements				( this INamedTypeSymbol typeSymbol, Type t )
		{
			return typeSymbol.AllInterfaces
				.Any( i => i.ToString(  ) == t.FullName );
		}

		public static		Boolean					HasAttribute			( this ISymbol typeSymbol, Type t )
		{
			return typeSymbol.GetAttributes(  )
				.Any( attr => attr.AttributeClass.IsTypeOrHasBaseType( t ) );
		}

		public static		Boolean					IsTypeOrHasBaseType		( this ITypeSymbol typeSymbol, Type t )
		{
			if ( typeSymbol.ToString(  ) == t.FullName )
			{
				return true;
			}
			if ( typeSymbol.BaseType != null )
			{
				return typeSymbol.BaseType.IsTypeOrHasBaseType( t );
			}
			return false;
		}

		public static		Int32					CountAttribute			( this ISymbol typeSymbol, Type t )
		{
			return typeSymbol.GetAttributes(  )
				.Count( attr => attr.AttributeClass.IsTypeOrHasBaseType( t ) );
		}

		public static		AttributeData			GetAttribute			( this ISymbol typeSymbol, Type t )
		{
			return typeSymbol.GetAttributes(  )
				.Single( attr => attr.AttributeClass.IsTypeOrHasBaseType( t ) );
		}

		public static		List<String>			GetContextNames			( this ISymbol t )
		{
			return t.GetAttributes(  )
				.Where( attr => attr.AttributeClass.IsTypeOrHasBaseType( typeof( ContextAttribute ) ) )
				.Select( attr => (String)attr.ConstructorArguments[0].Value )
				.ToList(  );
		}
	}
}