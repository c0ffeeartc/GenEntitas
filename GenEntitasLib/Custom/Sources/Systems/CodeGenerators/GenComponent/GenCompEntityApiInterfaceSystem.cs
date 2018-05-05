using System;
using System.Collections.Generic;
using System.IO;
using Entitas;
using Entitas.CodeGeneration.Plugins;
using Ent = MainEntity;

namespace GenEntitas.Sources
{
	public class GenCompEntityApiInterfaceSystem : ReactiveSystem<Ent>
	{
		public				GenCompEntityApiInterfaceSystem	( Contexts contexts ) : base( contexts.main )
		{
			_contexts = contexts;
		}

		private				Contexts				_contexts;

		private const		String					STANDARD_TEMPLATE		=
@"public partial interface I${ComponentName}Entity {

    ${ComponentType} ${componentName} { get; }
    bool has${ComponentName} { get; }

    void Add${ComponentName}(${newMethodParameters});
    void Replace${ComponentName}(${newMethodParameters});
    void Remove${ComponentName}();
}
";

		private const		String					FLAG_TEMPLATE			=
@"public partial interface I${ComponentName}Entity {
    bool ${prefixedComponentName} { get; set; }
}
";

		private const		String					ENTITY_INTERFACE_TEMPLATE = "public partial class ${EntityType} : I${ComponentName}Entity { }\n";

		protected override	ICollector<Ent>			GetTrigger				( IContext<Ent> context )
		{
			return context.CreateCollector( MainMatcher.AllOf( MainMatcher.Comp, MainMatcher.ContextNamesComp ).NoneOf( MainMatcher.DontGenerateComp ) );
		}

		protected override	Boolean					Filter					( Ent entity )
		{
			return entity.hasComp && entity.hasContextNamesComp && !entity.isDontGenerateComp;
		}

		protected override	void					Execute					( List<Ent> entities )
		{
			foreach ( var ent in entities )
			{
				if ( ent.contextNamesComp.Values.Count < 2 )
				{
					continue;
				}

				{
					var template		= ent.hasPublicFieldsComp ? STANDARD_TEMPLATE : FLAG_TEMPLATE;
					var filePath		= "Components" + Path.DirectorySeparatorChar + "Interfaces" + Path.DirectorySeparatorChar + "I" + ent.ComponentName(  ) + "Entity.cs";
					var contents		= template.Replace( ent, String.Empty );
					var generatedBy		= GetType(  ).FullName;

					var fileEnt			= _contexts.main.CreateEntity(  );
					fileEnt.AddGeneratedFileComp( filePath, contents, generatedBy );
				}

				var contextNames = ent.contextNamesComp.Values;
				foreach ( var contextName in contextNames )
				{
					var filePath		= contextName + Path.DirectorySeparatorChar + "Components" + Path.DirectorySeparatorChar + ent.ComponentNameWithContext(contextName).AddComponentSuffix() + ".cs";
					var contents		= ENTITY_INTERFACE_TEMPLATE.Replace(ent, contextName);
					var generatedBy		= GetType(  ).FullName;

					var fileEnt			= _contexts.main.CreateEntity(  );
					fileEnt.AddGeneratedFileComp( filePath, contents, generatedBy );
				}
			}
		}
	}
}
