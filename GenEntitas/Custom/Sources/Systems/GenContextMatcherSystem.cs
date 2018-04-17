using System;
using System.Collections.Generic;
using System.IO;
using Entitas;
using Ent = MainEntity;

namespace GenEntitas.Sources
{
	public class GenContextMatcherSystem : ReactiveSystem<Ent>
	{
		public				GenContextMatcherSystem	( Contexts contexts ) : base( contexts.main )
		{
			_contexts			= contexts;
		}

		private				Contexts			_contexts;
		private const		String				TEMPLATE					=
@"public sealed partial class ${MatcherType} {

    public static Entitas.IAllOfMatcher<${EntityType}> AllOf(params int[] indices) {
        return Entitas.Matcher<${EntityType}>.AllOf(indices);
    }

    public static Entitas.IAllOfMatcher<${EntityType}> AllOf(params Entitas.IMatcher<${EntityType}>[] matchers) {
          return Entitas.Matcher<${EntityType}>.AllOf(matchers);
    }

    public static Entitas.IAnyOfMatcher<${EntityType}> AnyOf(params int[] indices) {
          return Entitas.Matcher<${EntityType}>.AnyOf(indices);
    }

    public static Entitas.IAnyOfMatcher<${EntityType}> AnyOf(params Entitas.IMatcher<${EntityType}>[] matchers) {
          return Entitas.Matcher<${EntityType}>.AnyOf(matchers);
    }
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
				var filePath		= contextName + Path.DirectorySeparatorChar + contextName.AddMatcherSuffix(  ) + ".cs";
				var contents		= TEMPLATE.Replace( contextName );
				var generatedBy		= GetType(  ).FullName;
				var fileEnt			= _contexts.main.CreateEntity(  );
				fileEnt.AddGeneratedFileComp( filePath, contents, generatedBy );
			}
		}
	}
}