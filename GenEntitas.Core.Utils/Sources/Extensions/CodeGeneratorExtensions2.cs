using System;
using System.Collections.Generic;
using System.Linq;
using DesperateDevs.Utils;
using Entitas;
using Entitas.CodeGeneration.Attributes;
using Entitas.CodeGeneration.Plugins;

namespace GenEntitas {

    public static class CodeGeneratorExtentions2 {

        public const string LOOKUP = "ComponentsLookup";

        public static string ComponentName( this MainEntity ent, Contexts contexts ) {
            return ent.comp.FullTypeName.ToComponentName( contexts.settings.isIgnoreNamespaces );
        }

        public static string ComponentNameWithContext(this MainEntity ent, string contextName) {
            return contextName + ent.comp.Name;
        }

		public static string WrapInNamespace( this string s, Contexts contexts )
		{
			var value	= contexts.settings.hasGeneratedNamespace
				? contexts.settings.generatedNamespace.Value
				: null;
			return String.IsNullOrEmpty( value ) ? s : $"namespace {value} {{\n{s}\n}}\n";
		}

		public static string Replace( this string template, Contexts contexts, MainEntity ent, string contextName )
		{
			if ( ent.hasComp )
			{
				var componentName = ent.ComponentName( contexts );
				template = template
					.Replace(contextName)
					.Replace("${ComponentType}", ent.comp.FullTypeName)
					.Replace("${ComponentName}", componentName)
					.Replace("${validComponentName}", componentName.LowercaseFirst())
					.Replace("${componentName}", componentName.LowercaseFirst())
					.Replace("${Index}", contextName + LOOKUP + "." + componentName)
					.Replace("${prefixedComponentName}", ent.PrefixedComponentName(contexts));
			}

			if ( ent.hasPublicFieldsComp )
			{
				template = template
					.Replace("${newMethodParameters}", GetMethodParameters(ent.publicFieldsComp.Values, true))
					.Replace("${methodParameters}", GetMethodParameters(ent.publicFieldsComp.Values, false))
					.Replace("${newMethodArgs}", GetMethodArgs(ent.publicFieldsComp.Values, true))
					.Replace("${methodArgs}", GetMethodArgs(ent.publicFieldsComp.Values, false));
			}
			return template;
		}

        public static string Replace( this string template, Contexts contexts, MainEntity ent, string contextName,
	        EventInfo eventInfo ) {
            var eventListener = ent.EventListener( contexts, contextName, eventInfo);
            var lowerEventListener = ent.hasContextNamesComp && ent.contextNamesComp.Values.Count > 1
                ? contextName.LowercaseFirst() + ent.ComponentName(contexts) + ent.GetEventTypeSuffix(eventInfo) + "Listener"
                : ent.ComponentName(contexts).LowercaseFirst() + ent.GetEventTypeSuffix(eventInfo) + "Listener";

            return template
                .Replace( contexts, ent, contextName)
                .Replace("${EventListenerComponent}", eventListener.AddComponentSuffix())
                .Replace("${Event}", ent.Event( contexts, contextName, eventInfo))
                .Replace("${EventListener}", eventListener)
                .Replace("${eventListener}", lowerEventListener)
                .Replace("${EventType}", ent.GetEventTypeSuffix(eventInfo));
        }

        public static string PrefixedComponentName(this MainEntity ent, Contexts contexts ) {
        	var uniquePrefix = ent.hasUniquePrefixComp ? ent.uniquePrefixComp.Value : "";
            return uniquePrefix.LowercaseFirst() + ent.ComponentName( contexts );
        }

		public static string Event( this MainEntity ent, Contexts contexts, string contextName, EventInfo eventInfo ) {
			var optionalContextName = ent.hasContextNamesComp && ent.contextNamesComp.Values.Count > 1 ? contextName : string.Empty;
			return optionalContextName + ent.ComponentName( contexts ) + ent.GetEventTypeSuffix(eventInfo);
		}

		public static string EventListener( this MainEntity ent, Contexts contexts, string contextName, EventInfo eventInfo ) {
			return ent.Event( contexts, contextName, eventInfo) + "Listener";
		}

        public static string GetEventMethodArgs(this MainEntity ent, EventInfo eventInfo, string args) {
            if (!ent.hasPublicFieldsComp) {
                return string.Empty;
            }

            return eventInfo.EventType == EventType.Removed
                ? string.Empty
                : args;
        }

		public static string GetEventTypeSuffix(this MainEntity ent, EventInfo eventInfo) {
			return eventInfo.EventType == EventType.Removed ? "Removed" : string.Empty;
		}

        public static string GetMethodParameters(this List<FieldInfo> memberData, bool newPrefix) {
            return string.Join(", ", memberData
                .Select(info => info.TypeName + (newPrefix ? " new" + info.FieldName.UppercaseFirst() : " " + info.FieldName))
                .ToArray());
        }

        public static string GetMethodArgs(List<FieldInfo> memberData, bool newPrefix) {
            return string.Join(", ", memberData
                .Select(info => (newPrefix ? "new" + info.FieldName.UppercaseFirst() : info.FieldName))
                .ToArray()
            );
        }
    }
}
