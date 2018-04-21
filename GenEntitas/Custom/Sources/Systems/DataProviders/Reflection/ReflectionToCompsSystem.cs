using System;
using System.Collections.Generic;
using System.Linq;
using DesperateDevs.Utils;
using Entitas;
using Entitas.CodeGeneration.Attributes;
using Ent = MainEntity;

namespace GenEntitas.Sources
{
	public class ReflectionToCompsSystem : ReactiveSystem<Ent>
	{
		public				ReflectionToCompsSystem	( Contexts contexts ) : base( contexts.main )
		{
			_contexts			= contexts;
		}

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
			}
		}

		private				void					ProvideComp				( MainEntity ent )
		{
			var t		= ent.typeComp.Value;
			if ( t.ImplementsInterface<IComponent>(  ) )
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
				ent.AddContextNamesComp( new List<string>{ "Main" } );
				return;
			}

			ent.AddContextNamesComp( contextNames );
		}

		private				void					ProvideEventComp		( MainEntity ent )
		 {
			var type			= ent.typeComp.Value;
			var eventInfos		= Attribute.GetCustomAttributes(type)
				.OfType<EventAttribute>()
				.Select(attr => new EventInfo(attr.bindToEntity, attr.eventType, attr.priority))
				.ToList();

			if ( eventInfos.Count <= 0 )
			{
				return;
			}

			ent.AddEventComp( eventInfos );
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

			ent.AddPublicFieldsComp( memberData );
		}

		private				void					ProvideUniquePrefix		( Ent ent )
		{
			var attr		= Attribute.GetCustomAttributes(ent.typeComp.Value)
				.OfType<UniquePrefixAttribute>()
				.SingleOrDefault();

			if ( attr == null )
			{
				return;
			}

			ent.AddUniquePrefixComp( attr.prefix );
		}

		private				void					ProvideDontGenerate		( Ent ent )
		{
			var dontGenerate		= Attribute
				.GetCustomAttributes(ent.typeComp.Value)
				.OfType<DontGenerateAttribute>()
				.Any();
			ent.isDontGenerateComp		= dontGenerate;
		}
	}
}