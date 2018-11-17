using System;
using System.Linq;
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
				.Any( attr => attr.AttributeClass.ToString(  ) == t.FullName );
		}
	}
}