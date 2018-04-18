using System;
using System.Collections.Generic;
using System.IO;
using Entitas;
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
			return context.CreateCollector( MainMatcher.ContextComp );
		}

		protected override	Boolean					Filter					( Ent entity )
		{
			return entity.hasContextComp;
		}

		protected override	void					Execute					( List<Ent> entities )
		{
			for ( var i = 0; i < entities.Count; i++ )
			{
				var ent				= entities[i];
				var contextName		= ent.contextComp.Name;

				var filePath		= contextName + Path.DirectorySeparatorChar + contextName.AddEntitySuffix(  ) + ".cs";
				var contents		= CLASS_TEMPLATE.Replace( contextName );
				var generatedBy		= GetType(  ).FullName;

				var fileEnt			= _contexts.main.CreateEntity(  );
				fileEnt.AddGeneratedFileComp( filePath, contents, generatedBy );
			}
		}
	}
}
