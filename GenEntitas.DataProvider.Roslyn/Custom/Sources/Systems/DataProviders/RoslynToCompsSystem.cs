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
			ent.isDontGenerateComp		= ent.iNamedTypeSymbol.Value
				.GetAttributes()
				.Any( attr => attr.AttributeClass.ToString(  ) == typeof( DontGenerateAttribute ).FullName );
		}

		private				void					ProvideComp				( MainEntity ent )
		{
			var t						= ent.iNamedTypeSymbol.Value;
			var isComp					= t.AllInterfaces.Any( i => i.ToString(  ) == typeof( IComponent).FullName );
			if ( isComp )
			{
				ent.AddComp( t.Name, t.Name );
				ent.isAlreadyImplementedComp	= true;
			}
			else
			{
				ent.AddNonIComp( t.Name, t.Name );
			}
		}

		private				void					ProvideContextNamesComp	( MainEntity ent )
		{
			var t						= ent.iNamedTypeSymbol.Value;
			List<String> contextNames	= null;
			foreach ( var attr in t.GetAttributes(  ) )
			{
				if ( attr.AttributeClass.ToString(  ) == typeof( ContextAttribute ).FullName )
				{
					if ( contextNames == null )
					{
						contextNames = new List<String>(  );
					}
					contextNames.Add( (String)attr.ConstructorArguments[0].Value );
				}
			}

			if ( contextNames == null )
			{
				ent.AddContextNamesComp( new List<string>{ "Undefined" } );
				return;
			}

			ent.AddContextNamesComp( contextNames );
		}

		private				void					ProvideUniqueComp		( Ent ent )
		{
			ent.isUniqueComp		= ent.iNamedTypeSymbol.Value
				.GetAttributes()
				.Any( attr => attr.AttributeClass.ToString(  ) == typeof( UniqueAttribute ).FullName );
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
			ent.isGenCompEntApiInterface_ForSingleContext = ent.iNamedTypeSymbol.Value
				.GetAttributes(  )
				.Any( attr => attr.AttributeClass.ToString(  ) == typeof( GenCompEntApiInterface_ForSingleContextAttribute ).FullName );
		}

		private				void					ProvidePublicFieldsComp	( Ent ent )
		{
			var type					= ent.iNamedTypeSymbol.Value;
			var memberInfos				= new List<FieldInfo>();
			foreach (var member in type.GetMembers())
			{
				if ( ( member is IFieldSymbol || member is IPropertySymbol && IsAutoProperty( (IPropertySymbol)member ) )
					&& !member.IsStatic
					&& member.DeclaredAccessibility == Accessibility.Public 
					&& member.CanBeReferencedByName) // We don't want any backing fields here.
				{
					var memberType = (member is IFieldSymbol) ? ((IFieldSymbol)member).Type : ((IPropertySymbol)member).Type;
					memberInfos.Add( new FieldInfo( memberType.ToDisplayString(  ), member.Name ) );
				}
			}

			if ( memberInfos.Count == 0 )
			{
				return;
			}

			ent.AddPublicFieldsComp( memberInfos );
		}

		private				Boolean					IsAutoProperty			( IPropertySymbol member )
        {
            var ret = member.SetMethod != null && member.GetMethod != null &&
                !member.GetMethod.DeclaringSyntaxReferences.First()
                .GetSyntax()
                .DescendantNodes()
                .Any(x => x is MethodDeclarationSyntax) &&  
                !member.SetMethod.DeclaringSyntaxReferences.First()
                .GetSyntax()
                .DescendantNodes()
                .Any(x => x is MethodDeclarationSyntax);
            return ret;
        }

		private				void					ProvideEventComp		( MainEntity ent )
		{
			var type			= ent.iNamedTypeSymbol.Value;
			var eventInfos		= new List<EventInfo>(  );

			foreach ( var attr in type.GetAttributes(  ) )
			{
				if ( attr.AttributeClass.ToString(  ) == typeof( EventAttribute ).FullName )
				{
					var eventTarget		= (EventTarget)attr.ConstructorArguments[0].Value;
					var eventType		= (EventType)attr.ConstructorArguments[1].Value;
					var priority		= (Int32)attr.ConstructorArguments[2].Value;
					eventInfos.Add( new EventInfo( eventTarget, eventType, priority ) );
				}
			}

			if ( eventInfos.Count <= 0 )
			{
				return;
			}

			ent.AddEventComp( eventInfos );
			ReflectionToCompsSystem.ProvideEventCompNewEnts( _contexts, ent );
		}
	}
}