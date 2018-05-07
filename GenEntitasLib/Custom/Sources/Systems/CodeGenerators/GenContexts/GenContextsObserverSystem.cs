using System;
using System.Collections.Generic;
using System.Linq;
using DesperateDevs.Utils;
using Entitas;
using Ent = MainEntity;

namespace GenEntitas.Sources
{
	public class GenContextsObserverSystem : ReactiveSystem<Ent>
	{
		public				GenContextsObserverSystem( Contexts contexts ) : base( contexts.main )
		{
			_contexts			= contexts;
		}

		private				Contexts				_contexts;
		private const		String					CONTEXTS_TEMPLATE		=
@"public partial class Contexts {

#if (!ENTITAS_DISABLE_VISUAL_DEBUGGING && UNITY_EDITOR)

    [Entitas.CodeGeneration.Attributes.PostConstructor]
    public void InitializeContexObservers() {
        try {
${contextObservers}
        } catch(System.Exception) {
        }
    }

    public void CreateContextObserver(Entitas.IContext context) {
        if (UnityEngine.Application.isPlaying) {
            var observer = new Entitas.VisualDebugging.Unity.ContextObserver(context);
            UnityEngine.Object.DontDestroyOnLoad(observer.gameObject);
        }
    }

#endif
}
";
		private const		String					CONTEXT_OBSERVER_TEMPLATE = @"            CreateContextObserver(${contextName});";

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
			var contextNames	= new List<String>(  );
			for ( var i = 0; i < entities.Count; i++ )
			{
				var ent = entities[i];
				contextNames.Add( ent.contextComp.Name );
			}
			contextNames.Sort( ( a, b ) => String.Compare( a, b, StringComparison.Ordinal ) );
			var fileEnt			= _contexts.main.CreateEntity(  );
			var contents		= Generate( contextNames.ToArray(  ) );
			fileEnt.AddGeneratedFileComp( "Contexts.cs", contents, GetType(  ).FullName );
		}

		private				String					Generate				( String[] contextNames )
		{
			var contextObservers = string.Join("\n", contextNames
				.Select(contextName => CONTEXT_OBSERVER_TEMPLATE
					.Replace("${contextName}", contextName.LowercaseFirst())
				).ToArray());

			return CONTEXTS_TEMPLATE
				.Replace("${contextObservers}", contextObservers);
		}
	}
}