using DesperateDevs.Utils;
using Entitas;

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
    }
}
