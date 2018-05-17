using System;
using System.Collections.Generic;
using System.IO;
using Entitas;
using Entitas.CodeGeneration.Plugins;
using Ent = MainEntity;

namespace GenEntitas
{
	public class GenCompContextApiSystem : ReactiveSystem<Ent>
	{
		public				GenCompContextApiSystem	( Contexts contexts ) : base( contexts.main )
		{
			_contexts = contexts;
		}

		private				Contexts				_contexts;

		private const		String					STANDARD_TEMPLATE		=
@"public partial class ${ContextType} {

    public ${EntityType} ${componentName}Entity { get { return GetGroup(${MatcherType}.${ComponentName}).GetSingleEntity(); } }
    public ${ComponentType} ${componentName} { get { return ${componentName}Entity.${componentName}; } }
    public bool has${ComponentName} { get { return ${componentName}Entity != null; } }

    public ${EntityType} Set${ComponentName}(${newMethodParameters}) {
        if (has${ComponentName}) {
            throw new Entitas.EntitasException(""Could not set ${ComponentName}!\n"" + this + "" already has an entity with ${ComponentType}!"",
                ""You should check if the context already has a ${componentName}Entity before setting it or use context.Replace${ComponentName}()."");
        }
        var entity = CreateEntity();
        entity.Add${ComponentName}(${newMethodArgs});
        return entity;
    }

    public void Replace${ComponentName}(${newMethodParameters}) {
        var entity = ${componentName}Entity;
        if (entity == null) {
            entity = Set${ComponentName}(${newMethodArgs});
        } else {
            entity.Replace${ComponentName}(${newMethodArgs});
        }
    }

    public void Remove${ComponentName}() {
        ${componentName}Entity.Destroy();
    }
}
";

		private const		String					FLAG_TEMPLATE			=
@"public partial class ${ContextType} {

    public ${EntityType} ${componentName}Entity { get { return GetGroup(${MatcherType}.${ComponentName}).GetSingleEntity(); } }

    public bool ${prefixedComponentName} {
        get { return ${componentName}Entity != null; }
        set {
            var entity = ${componentName}Entity;
            if (value != (entity != null)) {
                if (value) {
                    CreateEntity().${prefixedComponentName} = true;
                } else {
                    entity.Destroy();
                }
            }
        }
    }
}
";

		protected override	ICollector<Ent>			GetTrigger				( IContext<Ent> context )
		{
			return context.CreateCollector( MainMatcher.AllOf( MainMatcher.Comp, MainMatcher.UniqueComp ).NoneOf( MainMatcher.DontGenerateComp ) );
		}

		protected override	Boolean					Filter					( Ent entity )
		{
			return entity.isUniqueComp && entity.hasComp && !entity.isDontGenerateComp;
		}

		protected override	void					Execute					( List<Ent> entities )
		{
			foreach ( var ent in entities )
			{
				var contextNames	= ent.contextNamesComp.Values;
				var template		= ent.hasPublicFieldsComp ? STANDARD_TEMPLATE : FLAG_TEMPLATE;
				foreach ( var contextName in contextNames )
				{
					var filePath		= contextName + Path.DirectorySeparatorChar + "Components" + Path.DirectorySeparatorChar + contextName + ent.comp.Name.AddComponentSuffix(  ) + ".cs";
					var contents		= template.Replace( _contexts, ent, contextName );
					var generatedBy		= GetType().FullName;

					var fileEnt			= _contexts.main.CreateEntity(  );
					fileEnt.AddGeneratedFileComp( filePath, contents, generatedBy );
				}
			}
		}
	}
}

