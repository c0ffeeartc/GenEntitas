using System;
using System.Collections.Generic;
using DesperateDevs.Utils;
using Entitas;
using Entitas.CodeGeneration.Attributes;
using Entitas.CodeGeneration.Plugins;

namespace GenEntitas {

    public static class CodeGeneratorExtentions {

        public const string LOOKUP = "ComponentsLookup";

        public static string Replace(this string template, string contextName) {
            return template
                .Replace("${ContextName}", contextName)
                .Replace("${contextName}", contextName.LowercaseFirst())
                .Replace("${ContextType}", contextName.AddContextSuffix())
                .Replace("${EntityType}", contextName.AddEntitySuffix())
                .Replace("${MatcherType}", contextName.AddMatcherSuffix())
                .Replace("${Lookup}", contextName + LOOKUP);
        }

		public static		void					ProvideEventCompNewEnts	( Contexts contexts, MainEntity ent )
		{
			foreach ( var contextName in ent.contextNamesComp.Values )
			{
				foreach ( var eventInfo in ent.eventComp.Values )
				{
					var componentName				= ent.comp.FullTypeName.ToComponentName( contexts.settings.isIgnoreNamespaces );
					var optionalContextName			= ent.contextNamesComp.Values.Count > 1 ? contextName : string.Empty;
					var eventTypeSuffix				= ent.GetEventTypeSuffix( eventInfo );
					var theAnySuffix				= eventInfo.EventTarget == EventTarget.Any ? "Any" : "";
					var listenerComponentName		= optionalContextName + theAnySuffix + componentName + eventTypeSuffix + "Listener";
					var eventCompFullTypeName		= listenerComponentName.AddComponentSuffix();

					var eventListenerCompEnt			= contexts.main.CreateEntity(  );
					eventListenerCompEnt.isEventListenerComp	= true;

					eventListenerCompEnt.AddComp( listenerComponentName, eventCompFullTypeName );
					eventListenerCompEnt.AddContextNamesComp( new List<String>{ contextName } );
					eventListenerCompEnt.AddPublicFieldsComp( new List<FieldInfo>
						{
							new FieldInfo( "System.Collections.Generic.List<I" + listenerComponentName + ">", "value" )
						} );
				}
			}
		}
    }
}
