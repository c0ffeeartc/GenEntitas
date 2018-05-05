using System.Linq;
using DesperateDevs.Utils;
using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace GenEntitas.Sources {

    public static class CodeGeneratorExtentions {

        public const string LOOKUP = "ComponentsLookup";

        public static bool ignoreNamespaces;

        public static string Replace(this string template, string contextName) {
            return template
                .Replace("${ContextName}", contextName)
                .Replace("${contextName}", contextName.LowercaseFirst())
                .Replace("${ContextType}", contextName.AddContextSuffix())
                .Replace("${EntityType}", contextName.AddEntitySuffix())
                .Replace("${MatcherType}", contextName.AddMatcherSuffix())
                .Replace("${Lookup}", contextName + LOOKUP);
        }
    }
}
