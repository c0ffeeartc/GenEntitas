using System;
using System.Collections.Generic;
using System.Linq;
using Entitas;
using Entitas.CodeGeneration.Attributes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Ent = GenEntitas.MainEntity;

namespace GenEntitas.DataProvider.Roslyn
{
	public class RoslynToCompsSystem : ReactiveSystem<Ent>
	{
		public				RoslynToCompsSystem	( Contexts contexts ) : base( contexts.main )
		{
			_contexts			= contexts;
		}

		private				Contexts				_contexts;

		protected override	ICollector<Ent>			GetTrigger				( IContext<Ent> context )
		{
			return context.CreateCollector( MainMatcher.RoslynComponentTypes );
		}

		protected override	Boolean					Filter					( Ent ent )
		{
			return ent.hasRoslynComponentTypes;
		}

		protected override	void					Execute					( List<Ent> ents )
		{
			var typeSymbols = ents[0].roslynComponentTypes.Values;
			foreach ( var t in typeSymbols )
			{
				var ent = _contexts.main.CreateEntity();
				ent.AddINamedTypeSymbol( t );

				ProvideDontGenerate( ent );
				ProvideComp( ent );
				ProvideContextNamesComp( ent );
				ProvideEventComp( ent );
				ProvideUniqueComp( ent );
				ProvidePublicFieldsComp( ent );
				ProvideFlagPrefix( ent );
				ProvideGenCompEntApiInterface_ForSingleContextAttr( ent );
			}
		}

		private				void					ProvideDontGenerate		( Ent ent )
		{
			ent.isDontGenerateComp		= ent.iNamedTypeSymbol.Value.HasAttribute( typeof( DontGenerateAttribute ) );
		}

		private				void					ProvideComp				( MainEntity ent )
		{
			var t						= ent.iNamedTypeSymbol.Value;
			if ( t.Implements( typeof( IComponent ) ) )
			{
				ent.AddComp( t.Name, t.ContainingNamespace.Name + "." + t.Name );
				ent.isAlreadyImplementedComp	= true;
			}
			else
			{
				ent.AddNonIComp( t.Name, t.ContainingNamespace.Name + "." + t.Name );
			}
		}

		private				void					ProvideContextNamesComp	( MainEntity ent )
		{
			var t					= ent.iNamedTypeSymbol.Value;
			var contextNames		= t.GetContextNames(  );
			if ( contextNames.Count == 0 )
			{
				contextNames.Add( "Undefined" );
				return;
			}

			ent.AddContextNamesComp( contextNames );
		}

		private				void					ProvideUniqueComp		( Ent ent )
		{
			ent.isUniqueComp		= ent.iNamedTypeSymbol.Value.HasAttribute( typeof( UniqueAttribute ) );
		}

		private				void					ProvideFlagPrefix		( Ent ent )
		{
			var prefix				= "is";
			foreach ( var attr in ent.iNamedTypeSymbol.Value.GetAttributes(  ) )
			{
				if ( attr.AttributeClass.ToString(  ) == typeof( Entitas.CodeGeneration.Attributes.FlagPrefixAttribute ).FullName )
				{
					prefix = (String)attr.ConstructorArguments[0].Value;
					break;
				}
			}
			ent.AddUniquePrefixComp( prefix );
		}

		private				void					ProvideGenCompEntApiInterface_ForSingleContextAttr( Ent ent )
		{
			ent.isGenCompEntApiInterface_ForSingleContext = ent.iNamedTypeSymbol.Value.HasAttribute( typeof( GenCompEntApiInterface_ForSingleContextAttribute ) );
		}

		private				void					ProvidePublicFieldsComp	( Ent ent )
		{
			var type					= ent.iNamedTypeSymbol.Value;
			var memberInfos				= GetPublicFieldAndPropertyInfos( type );
			if ( memberInfos.Count == 0 )
			{
				return;
			}

			ent.AddPublicFieldsComp( memberInfos );
		}

		public static		List<ISymbol>	GetPublicFieldAndPropertySymbols ( INamedTypeSymbol type )
		{
			return type.GetMembers(  )
				.Where( member => ( ( member is IFieldSymbol || ( member is IPropertySymbol && IsAutoProperty( (IPropertySymbol)member ) ) )
					&& !member.IsStatic
					&& member.DeclaredAccessibility == Accessibility.Public 
					&& member.CanBeReferencedByName ) // We don't want any backing fields here.
					)
				.Select( m => m )
				.ToList(  );
		}

		private				List<FieldInfo>			GetPublicFieldAndPropertyInfos ( INamedTypeSymbol type )
		{
			return GetPublicFieldAndPropertySymbols( type )
				.Select( symbol => new FieldInfo( 
					( symbol is IFieldSymbol )
						? ((IFieldSymbol)symbol).Type.ToDisplayString(  )
						: ((IPropertySymbol)symbol).Type.ToDisplayString(  ),
					symbol.Name ) )
				 .ToList(  );
		}

		private static		Boolean					IsAutoProperty			( IPropertySymbol member )
        {
            var ret = member.SetMethod != null
				&& member.GetMethod != null
				&& !member.GetMethod.DeclaringSyntaxReferences
					.First()
					.GetSyntax()
					.DescendantNodes()
					.Any(x => x is MethodDeclarationSyntax)
				&& !member.SetMethod.DeclaringSyntaxReferences
					.First()
					.GetSyntax()
					.DescendantNodes()
					.Any(x => x is MethodDeclarationSyntax);
			return ret;
        }

		private				void					ProvideEventComp		( MainEntity ent )
		{
			var type			= ent.iNamedTypeSymbol.Value;
			var eventInfos		= type.GetAttributes(  )
				.Where( attr => attr.AttributeClass.ToString(  ) == typeof( EventAttribute ).FullName )
				.Select( attr => new EventInfo(
					eventTarget			: (EventTarget)attr.ConstructorArguments[0].Value,
					eventType			: (EventType)attr.ConstructorArguments[1].Value,
					priority			: (Int32)attr.ConstructorArguments[2].Value ) )
				.ToList(  );

			if ( eventInfos.Count <= 0 )
			{
				return;
			}

			ent.AddEventComp( eventInfos );
			CodeGeneratorExtentions.ProvideEventCompNewEnts( _contexts, ent );
		}
	}
}