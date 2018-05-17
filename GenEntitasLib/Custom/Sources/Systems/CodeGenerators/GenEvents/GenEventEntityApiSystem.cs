using System;
using System.Collections.Generic;
using System.IO;
using Entitas;
using Entitas.CodeGeneration.Plugins;
using Ent = MainEntity;

namespace GenEntitas
{
	public class GenEventEntityApiSystem : ReactiveSystem<Ent>
	{
		public				GenEventEntityApiSystem	( Contexts contexts ) : base( contexts.main )
		{
			_contexts			= contexts;
		}

		private				Contexts				_contexts;

		protected override	ICollector<Ent>			GetTrigger				( IContext<Ent> context )
		{
			return context.CreateCollector( MainMatcher.AllOf( MainMatcher.Comp, MainMatcher.EventComp ) );
		}

        private const		String					TEMPLATE				=
@"public partial class ${EntityType} {

    public void Add${EventListener}(I${EventListener} value) {
        var listeners = has${EventListener}
            ? ${eventListener}.value
            : new System.Collections.Generic.List<I${EventListener}>();
        listeners.Add(value);
        Replace${EventListener}(listeners);
    }

    public void Remove${EventListener}(I${EventListener} value, bool removeComponentWhenEmpty = true) {
        var listeners = ${eventListener}.value;
        listeners.Remove(value);
        if (removeComponentWhenEmpty && listeners.Count == 0) {
            Remove${EventListener}();
        } else {
            Replace${EventListener}(listeners);
        }
    }
}
";

		protected override	Boolean					Filter					( Ent entity )
		{
			return entity.hasComp && entity.hasEventComp;
		}

		protected override	void					Execute					( List<Ent> entities )
		{
			foreach ( var ent in entities )
			{
				var contextNames = ent.contextNamesComp.Values;
				foreach ( var contextName in contextNames )
				{
					var eventInfos = ent.eventComp.Values;
					foreach ( var eventInfo in eventInfos )
					{
						var filePath		= contextName + Path.DirectorySeparatorChar + "Components" + Path.DirectorySeparatorChar + contextName + ent.EventListener( _contexts, contextName, eventInfo).AddComponentSuffix() + ".cs";

						var contents = TEMPLATE
							.Replace( _contexts, ent, contextName, eventInfo);

						var generatedBy		= GetType().FullName;

						var fileEnt			= _contexts.main.CreateEntity(  );
						fileEnt.AddGeneratedFileComp( filePath, contents, generatedBy );
					}
				}
			}
        }
	}
}