using System;
using System.Collections.Generic;
using System.IO;
using Entitas;
using Ent = MainEntity;

namespace GenEntitas.Sources
{
	public class GenCompMatcherApiSystem : ReactiveSystem<Ent>
	{
		public				GenCompMatcherApiSystem	( Contexts contexts ) : base( contexts.main )
		{
			_contexts = contexts;
		}

		private				Contexts				_contexts;

		private const		String					STANDARD_TEMPLATE		=
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
					var filePath		= contextName + Path.DirectorySeparatorChar + "Components" + Path.DirectorySeparatorChar + contextName + ent.comp.FileName.AddComponentSuffix(  ) + ".cs";
					var contents		= STANDARD_TEMPLATE.Replace( "${ContextType}", contextName );
					var generatedBy		= GetType().FullName;

					var fileEnt			= _contexts.main.CreateEntity(  );
					fileEnt.AddGeneratedFileComp( filePath, contents, generatedBy );
				}
			}
		}
	}
}
