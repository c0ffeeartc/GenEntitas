using System;
using System.Collections.Generic;
using System.IO;
using Entitas;
using Entitas.CodeGeneration.Plugins;
using Ent = MainEntity;

namespace GenEntitas
{
	public class GenEventListenerCompSystem : ReactiveSystem<Ent>
	{
		public			GenEventListenerCompSystem	( Contexts contexts ) : base( contexts.main )
		{
			_contexts			= contexts;
		}

		private				Contexts				_contexts;
		private const		String					TEMPLATE				=
@"[Entitas.CodeGeneration.Attributes.DontGenerate(false)]
public sealed class ${EventListenerComponent} : Entitas.IComponent {
    public System.Collections.Generic.List<I${EventListener}> value;
}
";

		protected override	ICollector<Ent>			GetTrigger				( IContext<Ent> context )
		{
			return context.CreateCollector( MainMatcher.AllOf( MainMatcher.Comp, MainMatcher.EventComp ) );
		}

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
						var filePath		= "Events" + Path.DirectorySeparatorChar + "Components" + Path.DirectorySeparatorChar + ent.EventListener( _contexts, contextName, eventInfo).AddComponentSuffix() + ".cs";
						var contents		= TEMPLATE.Replace( _contexts, ent, contextName, eventInfo);
						var generatedBy		= GetType().FullName;

						var fileEnt			= _contexts.main.CreateEntity(  );
						fileEnt.AddGeneratedFileComp( filePath, contents, generatedBy );
					}
				}
			}
		}
	}
}