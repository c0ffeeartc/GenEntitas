using System;
using System.Collections.Generic;
using System.IO;
using Entitas;
using Ent = MainEntity;

namespace GenEntitas.Sources
{
	public class GenCompLookupSystem : ReactiveSystem<Ent>
	{
		public				GenCompLookupSystem	( Contexts contexts ) : base( contexts.main )
		{
			_contexts = contexts;
		}

		private				Contexts				_contexts;

		private const		String					STANDARD_TEMPLATE		=
@"public static class ${Lookup} {

${componentConstantsList}

${totalComponentsConstant}

    public static readonly string[] componentNames = {
${componentNamesList}
    };

    public static readonly System.Type[] componentTypes = {
${componentTypesList}
    };
}
";

		private const		String				COMPONENT_CONSTANT_TEMPLATE = @"    public const int ${ComponentName} = ${Index};";
		private const		String		TOTAL_COMPONENTS_CONSTANT_TEMPLATE	= @"    public const int TotalComponents = ${totalComponents};";
		private const		String					COMPONENT_NAME_TEMPLATE = @"        ""${ComponentName}""";
		private const		String					COMPONENT_TYPE_TEMPLATE = @"        typeof(${ComponentType})";

		protected override	ICollector<Ent>			GetTrigger				( IContext<Ent> context )
		{
			return context.CreateCollector( MainMatcher.AllOf( MainMatcher.Comp, MainMatcher.PublicFieldsComp ).NoneOf( MainMatcher.DontGenerateComp ) );
		}

		protected override	Boolean					Filter					( Ent entity )
		{
			return entity.hasComp && entity.hasPublicFieldsComp && !entity.isDontGenerateComp;
		}

		protected override	void					Execute					( List<Ent> entities )
		{
			foreach ( var ent in entities )
			{
				var contextNames = ent.contextNamesComp.Values;
				foreach ( var contextName in contextNames )
				{
					var filePath		= contextName + Path.DirectorySeparatorChar + "Components" + Path.DirectorySeparatorChar + contextName + ent.comp.Name.AddComponentSuffix(  ) + ".cs";
					var contents		= STANDARD_TEMPLATE.Replace( "${ContextType}", contextName );
					var generatedBy		= GetType().FullName;

					var fileEnt			= _contexts.main.CreateEntity(  );
					fileEnt.AddGeneratedFileComp( filePath, contents, generatedBy );
				}
			}
		}
	}
}