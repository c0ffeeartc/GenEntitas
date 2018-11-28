using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Runtime.InteropServices;
using DesperateDevs.Utils;
using Entitas;
using Entitas.CodeGeneration.Attributes;
using Ent = GenEntitas.MainEntity;

namespace GenEntitas
{
	[Export(typeof(IExecuteSystem))]
	[Guid("852FFEBA-D466-4505-B239-7C33D14C0446")]
	public class ReflectionToCompsSystem : ReactiveSystem<Ent>
	{
		public				ReflectionToCompsSystem	( Contexts contexts ) : base( contexts.main )
		{
			_contexts			= contexts;
		}

		public				ReflectionToCompsSystem	(  ) : this( Contexts.sharedInstance )
		{
		}

		public static	Dictionary<Type,List<String>> TypeToContextNames;
		private				Contexts				_contexts;

		protected override	ICollector<Ent>			GetTrigger				( IContext<Ent> context )
		{
			return context.CreateCollector( MainMatcher.ReflectionComponentTypes );
		}

		protected override	Boolean					Filter					( Ent entity )
		{
			return entity.hasReflectionComponentTypes;
		}

		protected override	void					Execute					( List<Ent> entities )
		{
			var types			= _contexts.main.reflectionComponentTypes.Values;

			TypeToContextNames	= new Dictionary<Type, List<String>>(  );

			foreach ( var t in types )
			{
				var ent = _contexts.main.CreateEntity();
				ent.AddTypeComp( t );

				ProvideDontGenerate( ent );
				ProvideComp( ent );
				ProvideContextNamesComp( ent );
				ProvideEventComp( ent );
				ProvideUniqueComp( ent );
				ProvidePublicFieldsComp( ent );
				ProvideUniquePrefix( ent );
				ProvideGenCompEntApiInterface_ForSingleContextAttr( ent );

				TypeToContextNames[t]	= ent.contextNamesComp.Values;
			}
		}

		private				void					ProvideComp				( MainEntity ent )
		{
			var t		= ent.typeComp.Value;
			if ( t.ImplementsInterface<IComponent>(  )
				&& !Attribute.GetCustomAttributes(t).OfType<DontGenerateAttribute>().Any() )
			{
				ent.AddComp( t.Name, t.ToCompilableString(  ) );
				ent.isAlreadyImplementedComp	= true;
			}
			else
			{
				ent.AddNonIComp( t.Name, t.ToCompilableString() );
			}
		}

		private				void					ProvideContextNamesComp	( MainEntity ent )
		{
			var t				= ent.typeComp.Value;
			var contextNames	= Attribute
				.GetCustomAttributes(t)
				.OfType<Entitas.CodeGeneration.Attributes.ContextAttribute>()
				.Select(attr => attr.contextName)
				.ToList();

			if ( contextNames.Count == 0 )
			{
				ent.AddContextNamesComp( new List<string>{ "Undefined" } );
				return;
			}

			ent.AddContextNamesComp( contextNames );
		}

		private				void					ProvideEventComp		( MainEntity ent )
		 {
			var type			= ent.typeComp.Value;
			var eventInfos		= Attribute.GetCustomAttributes(type)
				.OfType<EventAttribute>()
				.Select(attr => new EventInfo(attr.eventTarget, attr.eventType, attr.priority))
				.ToList();

			if ( eventInfos.Count <= 0 )
			{
				return;
			}

			ent.AddEventComp( eventInfos );
			CodeGeneratorExtentions.ProvideEventCompNewEnts( _contexts, ent );
		}

		private				void					ProvideUniqueComp		( Ent ent )
		{
			ent.isUniqueComp	= Attribute
				.GetCustomAttributes(ent.typeComp.Value)
				.OfType<UniqueAttribute>()
				.Any();
		}

		private				void					ProvidePublicFieldsComp	( Ent ent )
		{
			var memberData		= ent.typeComp.Value.GetPublicMemberInfos()
				.Select(info => new FieldInfo(info.type.ToCompilableString(), info.name))
				.ToList();

			if ( memberData.Count == 0 )
			{
				return;
			}

			ent.AddPublicFieldsComp( memberData );
		}

		private				void					ProvideUniquePrefix		( Ent ent )
		{
			var attr		= Attribute.GetCustomAttributes(ent.typeComp.Value)
				.OfType<FlagPrefixAttribute>()
				.SingleOrDefault();

			ent.AddUniquePrefixComp( attr == null ? "is" : attr.prefix );
		}

		private				void					ProvideDontGenerate		( Ent ent )
		{
			var dontGenerate		= Attribute
				.GetCustomAttributes(ent.typeComp.Value)
				.OfType<DontGenerateAttribute>()
				.Any();
			ent.isDontGenerateComp		= dontGenerate;
		}

		private				void					ProvideGenCompEntApiInterface_ForSingleContextAttr( Ent ent )
		{
			var value		= Attribute
				.GetCustomAttributes(ent.typeComp.Value)
				.OfType<GenCompEntApiInterface_ForSingleContextAttribute>()
				.Any();
			ent.isGenCompEntApiInterface_ForSingleContext = value;
		}
	}
}