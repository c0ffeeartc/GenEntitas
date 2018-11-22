using System;
using System.Collections.Generic;
using System.IO;
using Entitas;
using Entitas.CodeGeneration.Plugins;
using Ent = GenEntitas.MainEntity;

namespace GenEntitas
{
	public class GenCompMatcherApiSystem : ReactiveSystem<Ent>
	{
		public				GenCompMatcherApiSystem	( Contexts contexts ) : base( contexts.main )
		{
			_contexts = contexts;
		}

		private				Contexts				_contexts;

		private const		String					TEMPLATE		=
@"public sealed partial class ${MatcherType} {

    static Entitas.IMatcher<${EntityType}> _matcher${ComponentName};

    public static Entitas.IMatcher<${EntityType}> ${ComponentName} {
        get {
            if (_matcher${ComponentName} == null) {
                var matcher = (Entitas.Matcher<${EntityType}>)Entitas.Matcher<${EntityType}>.AllOf(${Index});
                matcher.componentNames = ${componentNames};
                _matcher${ComponentName} = matcher;
            }

            return _matcher${ComponentName};
        }
    }
}
";

		protected override	ICollector<Ent>			GetTrigger				( IContext<Ent> context )
		{
			return context.CreateCollector( MainMatcher.AllOf( MainMatcher.Comp ).NoneOf( MainMatcher.DontGenerateComp ) );
		}

		protected override	Boolean					Filter					( Ent entity )
		{
			return entity.hasComp && !entity.isDontGenerateComp;
		}

		protected override	void					Execute					( List<Ent> entities )
		{
			foreach ( var ent in entities )
			{
				var contextNames = ent.contextNamesComp.Values;
				foreach ( var contextName in contextNames )
				{
					var filePath		= contextName + Path.DirectorySeparatorChar + "Components" + Path.DirectorySeparatorChar + contextName + ent.comp.Name.AddComponentSuffix(  ) + ".cs";
					var contents		= TEMPLATE
						.Replace("${componentNames}", contextName + CodeGeneratorExtentions.LOOKUP + ".componentNames")
						.Replace( _contexts, ent, contextName );
					var generatedBy		= GetType().FullName;

					var fileEnt			= _contexts.main.CreateEntity(  );
					fileEnt.AddGeneratedFileComp( filePath, contents.WrapInNamespace( _contexts ), generatedBy );
				}
			}
		}
	}
}
