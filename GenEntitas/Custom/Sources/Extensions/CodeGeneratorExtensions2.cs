using System.Collections.Generic;
using System.Linq;
using DesperateDevs.Utils;
using Entitas.CodeGeneration.Attributes;

namespace Entitas.CodeGeneration.Plugins {

    public static class CodeGeneratorExtentions2 {

        public const string LOOKUP = "ComponentsLookup";

        public static bool ignoreNamespaces;

        public static string ComponentName(this MainEntity ent) {
            return ent.comp.FullTypeName.ToComponentName(ignoreNamespaces);
        }

        public static string ComponentNameWithContext(this MainEntity ent, string contextName) {
            return contextName + ent.comp.Name;
        }

		public static string Replace(this string template, MainEntity ent, string contextName)
		{
			if ( ent.hasComp )
			{
				var componentName = ent.comp.Name;
				template = template
					.Replace(contextName)
					.Replace("${ComponentType}", ent.comp.FullTypeName)
					.Replace("${ComponentName}", componentName)
					.Replace("${componentName}", componentName.LowercaseFirst())
					.Replace("${Index}", contextName + LOOKUP + "." + componentName)
					.Replace("${prefixedComponentName}", ent.PrefixedComponentName());
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

        public static string Replace(this string template, MainEntity ent, string contextName, EventInfo eventInfo) {
            var eventListener = ent.EventListener(contextName, eventInfo);
            var lowerEventListener = ent.hasContextNamesComp
                ? contextName.LowercaseFirst() + ent.comp.Name + ent.GetEventTypeSuffix(eventInfo) + "Listener"
                : ent.comp.Name.LowercaseFirst() + ent.GetEventTypeSuffix(eventInfo) + "Listener";

            return template
                .Replace(ent, contextName)
                .Replace("${EventListenerComponent}", eventListener.AddComponentSuffix())
                .Replace("${Event}", ent.Event(contextName, eventInfo))
                .Replace("${EventListener}", eventListener)
                .Replace("${eventListener}", lowerEventListener)
                .Replace("${EventType}", ent.GetEventTypeSuffix(eventInfo));
        }

        public static string PrefixedComponentName(this MainEntity ent) {
        	var uniquePrefix = ent.hasUniquePrefixComp ? ent.uniquePrefixComp.Value : "";
            return uniquePrefix.LowercaseFirst() + ent.comp.Name;
        }

		public static string Event(this MainEntity ent, string contextName, EventInfo eventInfo) {
			var optionalContextName = ent.hasContextNamesComp ? contextName : string.Empty;
			return optionalContextName + ent.comp.Name + ent.GetEventTypeSuffix(eventInfo);
		}

		public static string EventListener(this MainEntity ent, string contextName, EventInfo eventInfo) {
			return ent.Event(contextName, eventInfo) + "Listener";
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
