using System;
using System.Collections.Generic;
using System.Linq;
using DesperateDevs.Utils;
using Entitas;
using Entitas.CodeGeneration.Attributes;
using Entitas.CodeGeneration.Plugins;
using Ent = GenEntitas.MainEntity;

namespace GenEntitas
{
	public class ReflectionToCompsSystem : ReactiveSystem<Ent>
	{
		public				ReflectionToCompsSystem	( Contexts contexts ) : base( contexts.main )
		{
			_contexts			= contexts;
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
			ProvideEventCompNewEnts( ent );
		}

		private				void					ProvideEventCompNewEnts		( MainEntity ent )
		{
			foreach ( var contextName in ent.contextNamesComp.Values )
			{
				var eventComp					= _contexts.main.CreateEntity(  );

				foreach ( var eventInfo in ent.eventComp.Values )
				{
					var componentName				= ent.comp.FullTypeName.ToComponentName( _contexts.settings.isIgnoreNamespaces );
					var optionalContextName			= ent.contextNamesComp.Values.Count > 1 ? contextName : string.Empty;
					var eventTypeSuffix				= ent.GetEventTypeSuffix( eventInfo );
					var listenerComponentName		= optionalContextName + componentName + eventTypeSuffix + "Listener";
					var eventCompFullTypeName		= listenerComponentName.AddComponentSuffix();

					eventComp.AddComp( listenerComponentName, eventCompFullTypeName );
					eventComp.AddContextNamesComp( new List<String>{ contextName } );
					eventComp.AddPublicFieldsComp( new List<FieldInfo>
						{
							new FieldInfo( "System.Collections.Generic.List<I" + listenerComponentName + ">", "value" )
						} );
				}
			}
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
				.OfType<UniquePrefixAttribute>()
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
	}
}