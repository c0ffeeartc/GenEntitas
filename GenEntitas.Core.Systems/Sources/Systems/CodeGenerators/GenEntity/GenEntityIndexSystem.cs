using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Runtime.InteropServices;
using DesperateDevs.Utils;
using Entitas;
using Entitas.CodeGeneration.Plugins;
using Ent = GenEntitas.MainEntity;

namespace GenEntitas
{
	[Export(typeof(ISystem))]
	[Guid("541377C4-BC1F-4B1B-B9E6-5B6A0F73B6B4")]
	public class GenEntityIndexSystem : IExecuteSystem
	{
		public				GenEntityIndexSystem	( Contexts contexts )
		{
			_contexts			= contexts;
		}

		public				GenEntityIndexSystem	(  ) : this( Contexts.sharedInstance )
		{
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

		public				void					Execute					(  )
		{
			var entities = _contexts.main.GetGroup( MainMatcher.AllOf( MainMatcher.Comp, MainMatcher.PublicFieldsComp ) ).GetEntities(  );
			
			var entityIndexData = new List<EntityIndexData>(  );
			foreach ( var ent in entities )
			{
				foreach ( var field in ent.publicFieldsComp.Values )
				{
					if ( field.EntityIndexInfo == null )
					{
						continue;
					}

					entityIndexData.Add( field.EntityIndexInfo.EntityIndexData );
				}
			}

			var entsCustomIndex		= _contexts.main.GetGroup( MainMatcher.CustomEntityIndexComp ).GetEntities(  );
			foreach ( var ent in entsCustomIndex )
			{
				entityIndexData.Add( ent.customEntityIndexComp.EntityIndexData );
			}

			if ( entityIndexData.Count == 0 )
			{
				return;
			}

			entityIndexData.Sort( (a, b) => a.GetEntityIndexName(  ).CompareTo( b.GetEntityIndexName(  ) ) );
			generateEntityIndices( entityIndexData.ToArray(  ) );
		}

		void generateEntityIndices(EntityIndexData[] data) {

			var indexConstants = string.Join("\n", data
				.Select(d => INDEX_CONSTANTS_TEMPLATE
					.Replace("${IndexName}", d.GetHasMultiple()
						? d.GetEntityIndexName() + d.GetMemberName().UppercaseFirst()
						: d.GetEntityIndexName()))
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
				fileEnt.AddGeneratedFileComp( "Contexts.cs", fileContent.WrapInNamespace( _contexts ), GetType().FullName );
		}

		string generateAddMethods(EntityIndexData data) {
			return string.Join("\n", data.GetContextNames()
				.Aggregate(new List<string>(), (addMethods, contextName) => {
					addMethods.Add(generateAddMethod(data, contextName));
					return addMethods;
				}).ToArray());
		}

		string generateAddMethod(EntityIndexData data, string contextName) {
			return data.IsCustom()
				? generateCustomMethods(data)
				: generateMethods(data, contextName);
		}

		string generateCustomMethods(EntityIndexData data) {
			return ADD_CUSTOM_INDEX_TEMPLATE
				.Replace("${contextName}", data.GetContextNames()[0].LowercaseFirst())
				.Replace("${IndexType}", data.GetEntityIndexType());
		}

		string generateMethods(EntityIndexData data, string contextName) {
			return ADD_INDEX_TEMPLATE
				.Replace("${contextName}", contextName.LowercaseFirst())
				.Replace("${ContextName}", contextName)
				.Replace("${IndexName}", data.GetHasMultiple()
					? data.GetEntityIndexName() + data.GetMemberName().UppercaseFirst()
					: data.GetEntityIndexName())
				.Replace("${Matcher}", data.GetEntityIndexName())
				.Replace("${IndexType}", data.GetEntityIndexType())
				.Replace("${KeyType}", data.GetKeyType())
				.Replace("${ComponentType}", data.GetComponentType())
				.Replace("${MemberName}", data.GetMemberName())
				.Replace("${componentName}", data.GetComponentType().ToComponentName(_contexts.settings.isIgnoreNamespaces).LowercaseFirst());
		}

		string generateGetMethods(EntityIndexData data) {
			return string.Join("\n\n", data.GetContextNames()
				.Aggregate(new List<string>(), (getMethods, contextName) => {
					getMethods.Add(generateGetMethod(data, contextName));
					return getMethods;
				}).ToArray());
		}

		string generateGetMethod(EntityIndexData data, string contextName) {
			var template = "";
			if (data.GetEntityIndexType() == "Entitas.EntityIndex") {
				template = GET_INDEX_TEMPLATE;
			} else if (data.GetEntityIndexType() == "Entitas.PrimaryEntityIndex") {
				template = GET_PRIMARY_INDEX_TEMPLATE;
			} else {
				return getCustomMethods(data);
			}

			return template
				.Replace("${ContextName}", contextName)
				.Replace("${IndexName}", data.GetHasMultiple()
					? data.GetEntityIndexName() + data.GetMemberName().UppercaseFirst()
					: data.GetEntityIndexName())
				.Replace("${IndexType}", data.GetEntityIndexType())
				.Replace("${KeyType}", data.GetKeyType())
				.Replace("${MemberName}", data.GetMemberName());
		}

		string getCustomMethods(EntityIndexData data) {
			return string.Join("\n", data.GetCustomMethods()
				.Select(m => CUSTOM_METHOD_TEMPLATE
					.Replace("${ReturnType}", m.returnType)
					.Replace("${MethodName}", m.methodName)
					.Replace("${ContextName}", data.GetContextNames()[0])
					.Replace("${methodArgs}", string.Join(", ", m.parameters.Select(p => p.type + " " + p.name).ToArray()))
					.Replace("${IndexType}", data.GetEntityIndexType())
					.Replace("${IndexName}", data.GetHasMultiple()
						? data.GetEntityIndexName() + data.GetMemberName().UppercaseFirst()
						: data.GetEntityIndexName())
					.Replace("${args}", string.Join(", ", m.parameters.Select(p => p.name).ToArray()))).ToArray());
		}
	}
}
