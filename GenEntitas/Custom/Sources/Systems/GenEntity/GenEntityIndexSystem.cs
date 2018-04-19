using System;
using System.Collections.Generic;
using System.Linq;
using DesperateDevs.Utils;
using Entitas;
using Entitas.CodeGeneration.Plugins;
using Ent = MainEntity;

namespace GenEntitas.Sources
{
	public class GenEntityIndexSystem : ReactiveSystem<Ent>
	{
		public				GenEntityIndexSystem			( Contexts contexts ) : base( contexts.main )
		{
			_contexts			= contexts;
		}

		private				Contexts				_contexts;
		private const		String					CLASS_TEMPLATE			=
@"public partial class Contexts {

${indexConstants}

    [Entitas.CodeGeneration.Attributes.PostConstructor]
    public void InitializeEntityIndices() {
${addIndices}
    }
}

public static class ContextsExtensions {

${getIndices}
}";

		private const		String					INDEX_CONSTANTS_TEMPLATE = @"    public const string ${IndexName} = ""${IndexName}"";";

		private const		String					ADD_INDEX_TEMPLATE		=
            @"        ${contextName}.AddEntityIndex(new ${IndexType}<${ContextName}Entity, ${KeyType}>(
            ${IndexName},
            ${contextName}.GetGroup(${ContextName}Matcher.${Matcher}),
            (e, c) => ((${ComponentType})c).${MemberName}));";

		private const		String					ADD_CUSTOM_INDEX_TEMPLATE =
            @"        ${contextName}.AddEntityIndex(new ${IndexType}(${contextName}));";

		private const		String					GET_INDEX_TEMPLATE		=
            @"    public static System.Collections.Generic.HashSet<${ContextName}Entity> GetEntitiesWith${IndexName}(this ${ContextName}Context context, ${KeyType} ${MemberName}) {
        return ((${IndexType}<${ContextName}Entity, ${KeyType}>)context.GetEntityIndex(Contexts.${IndexName})).GetEntities(${MemberName});
    }";

		private const		String					GET_PRIMARY_INDEX_TEMPLATE =
            @"    public static ${ContextName}Entity GetEntityWith${IndexName}(this ${ContextName}Context context, ${KeyType} ${MemberName}) {
        return ((${IndexType}<${ContextName}Entity, ${KeyType}>)context.GetEntityIndex(Contexts.${IndexName})).GetEntity(${MemberName});
    }";

		private const		String					CUSTOM_METHOD_TEMPLATE	=
            @"    public static ${ReturnType} ${MethodName}(this ${ContextName}Context context, ${methodArgs}) {
        return ((${IndexType})(context.GetEntityIndex(Contexts.${IndexName}))).${MethodName}(${args});
    }
";

		protected override	ICollector<Ent>			GetTrigger				( IContext<Ent> context )
		{
			return context.CreateCollector( MainMatcher.AllOf( MainMatcher.EntityIndexComp, MainMatcher.PublicFieldsComp, MainMatcher.ContextNamesComp ) );
		}

		protected override	Boolean					Filter					( Ent entity )
		{
			return entity.hasEntityIndexComp && entity.hasPublicFieldsComp && entity.hasContextNamesComp;
		}

		protected override	void					Execute					( List<Ent> entities )
		{
			generateEntityIndices( entities.ToArray(  ) );
		}

		void generateEntityIndices(Ent[] data) {

			var indexConstants = string.Join("\n", data
				.Select(ent => INDEX_CONSTANTS_TEMPLATE
					.Replace("${IndexName}", ent.entityIndexComp.HasMultiple(  )
						? ent.entityIndexComp.GetEntityIndexName() + ent.entityIndexComp.GetMemberName().UppercaseFirst()
						: ent.entityIndexComp.GetEntityIndexName()))
				.ToArray());

			var addIndices = string.Join("\n\n", data
				.Select(generateAddMethods)
				.ToArray());

			var getIndices = string.Join("\n\n", data
				.Select(generateGetMethods)
				.ToArray());

			var fileContent = CLASS_TEMPLATE
				.Replace("${indexConstants}", indexConstants)
				.Replace("${addIndices}", addIndices)
				.Replace("${getIndices}", getIndices);

				var fileEnt			= _contexts.main.CreateEntity(  );
				fileEnt.AddGeneratedFileComp( "EntityIndex.cs", fileContent, GetType().FullName );
		}

		string generateAddMethods(Ent ent) {
			return string.Join("\n", ent.contextNamesComp.Values
				.Aggregate(new List<string>(), (addMethods, contextName) => {
					addMethods.Add(generateAddMethod(ent, contextName));
					return addMethods;
				}).ToArray());
		}

		string generateAddMethod(Ent ent, string contextName) {
			return ent.entityIndexComp.IsCustom()
				? generateCustomMethods(ent)
				: generateMethods(ent, contextName);
		}

		string generateCustomMethods(Ent ent) {
			return ADD_CUSTOM_INDEX_TEMPLATE
				.Replace("${contextName}", ent.contextNamesComp.Values[0].LowercaseFirst())
				.Replace("${IndexType}", ent.entityIndexComp.GetEntityIndexType());
		}

		string generateMethods(Ent ent, string contextName) {
			return ADD_INDEX_TEMPLATE
				.Replace("${contextName}", contextName.LowercaseFirst())
				.Replace("${ContextName}", contextName)
				.Replace("${IndexName}", ent.entityIndexComp.HasMultiple()
					? ent.entityIndexComp.GetEntityIndexName() + ent.entityIndexComp.GetMemberName().UppercaseFirst()
					: ent.entityIndexComp.GetEntityIndexName())
				.Replace("${Matcher}", ent.entityIndexComp.GetEntityIndexName())
				.Replace("${IndexType}", ent.entityIndexComp.GetEntityIndexType())
				.Replace("${KeyType}", ent.entityIndexComp.GetKeyType())
				.Replace("${ComponentType}", ent.entityIndexComp.GetComponentType())
				.Replace("${MemberName}", ent.entityIndexComp.GetMemberName())
				.Replace("${componentName}", ent.entityIndexComp.GetComponentType().ToComponentName(_contexts.settings.isIgnoreNamespaces).LowercaseFirst());
		}

		string generateGetMethods(Ent ent) {
			return string.Join("\n\n", ent.contextNamesComp.Values
				.Aggregate(new List<string>(), (getMethods, contextName) => {
					getMethods.Add(generateGetMethod(ent, contextName));
					return getMethods;
				}).ToArray());
		}

		string generateGetMethod(Ent ent, string contextName) {
			var template = "";
			if (ent.entityIndexComp.GetEntityIndexType() == "Entitas.EntityIndex") {
				template = GET_INDEX_TEMPLATE;
			} else if (ent.entityIndexComp.GetEntityIndexType() == "Entitas.PrimaryEntityIndex") {
				template = GET_PRIMARY_INDEX_TEMPLATE;
			} else {
				return getCustomMethods(ent);
			}

			return template
				.Replace("${ContextName}", contextName)
				.Replace("${IndexName}", ent.entityIndexComp.HasMultiple()
					? ent.entityIndexComp.GetEntityIndexName() + ent.entityIndexComp.GetMemberName().UppercaseFirst()
					: ent.entityIndexComp.GetEntityIndexName())
				.Replace("${IndexType}", ent.entityIndexComp.GetEntityIndexType())
				.Replace("${KeyType}", ent.entityIndexComp.GetKeyType())
				.Replace("${MemberName}", ent.entityIndexComp.GetMemberName());
		}

		string getCustomMethods(Ent ent) {
			return string.Join("\n", ent.entityIndexComp.GetCustomMethods()
				.Select(m => CUSTOM_METHOD_TEMPLATE
					.Replace("${ReturnType}", m.returnType)
					.Replace("${MethodName}", m.methodName)
					.Replace("${ContextName}", ent.contextNamesComp.Values[0])
					.Replace("${methodArgs}", string.Join(", ", m.parameters.Select(p => p.type + " " + p.name).ToArray()))
					.Replace("${IndexType}", ent.entityIndexComp.GetEntityIndexType())
					.Replace("${IndexName}", ent.entityIndexComp.HasMultiple()
						? ent.entityIndexComp.GetEntityIndexName() + ent.entityIndexComp.GetMemberName().UppercaseFirst()
						: ent.entityIndexComp.GetEntityIndexName())
					.Replace("${args}", string.Join(", ", m.parameters.Select(p => p.name).ToArray()))).ToArray());
		}
	}
    public static class EntityIndexDataExtension2 {
		public static		String					GetEntityIndexName		( this EntityIndexComp entityIndexComp )
		{
			return "FIXME";
		}
		public static		String					GetEntityIndexType		( this EntityIndexComp entityIndexComp )
		{
			return "FIXME";
		}
		public static		String					GetMemberName			( this EntityIndexComp entityIndexComp )
		{
			return "FIXME";
		}
		public static		Boolean					IsCustom				( this EntityIndexComp entityIndexComp )
		{
			return false;  // FIXME
		}
		public static		String					GetComponentType		( this EntityIndexComp entityIndexComp )
		{
			return "FIXME";
		}
		public static		String					GetKeyType				( this EntityIndexComp entityIndexComp )
		{
			return "FIXME";
		}
		public static		MethodData[]			GetCustomMethods		( this EntityIndexComp entityIndexComp )
		{
			return new []{new MethodData( "int", "FIXME", new []{ new MemberData("int", "FIXME") } )};
		}
		public static		Boolean					HasMultiple				( this EntityIndexComp entityIndexComp )
		{
			return entityIndexComp.Values.Count > 1;  // FIXME
		}
    }
}
