using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Entitas;
using Entitas.CodeGeneration.Plugins;
using Ent = GenEntitas.MainEntity;

namespace GenEntitas
{
	[Export(typeof(ISystem))]
	[Guid("E5400EF9-6EB2-4BCA-885D-1315AC7B1C4D")]
	public class GenEventSystemsSystem : ReactiveSystem<Ent>
	{
		public				GenEventSystemsSystem	( Contexts contexts ) : base( contexts.main )
		{
			_contexts			= contexts;
		}

		public				GenEventSystemsSystem	(  ) : this( Contexts.sharedInstance )
		{
		}

		private				Contexts				_contexts;
		private const		String					TEMPLATE				=
@"public sealed class EventSystems : Feature {

    public EventSystems(Contexts contexts) {
${systemsList}
    }
}
";

		private const		String					SYSTEM_ADD_TEMPLATE		= @"        Add(new ${ContextName}EventSystems(contexts));";

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
			var contextNamesWithEvents = new HashSet<String>(  );
			foreach ( var ent in entities )
			{
				contextNamesWithEvents.UnionWith( ent.contextNamesComp.Values );
			}

			var contextNames = contextNamesWithEvents.ToList(  );
			contextNames.Sort(  );

			var generatedBy		= GetType(  ).FullName;
			var filePath		= "Events" + Path.DirectorySeparatorChar + "EventSystems.cs";
			var contents		= TEMPLATE
				.Replace("${systemsList}", GenerateSystemList( contextNames ) );


			var fileEnt			= _contexts.main.CreateEntity(  );
			fileEnt.AddGeneratedFileComp( filePath, contents.WrapInNamespace( _contexts ), generatedBy );
		}

		private				String					GenerateSystemList		( List<String> contextNames )
		{
			return string.Join( "\n", contextNames
				.Select(contextName => GenerateSystemListForData( contextName ) )
				.ToArray(  ) );
		}

		private				String					GenerateSystemListForData( String contextName )
		{
			return SYSTEM_ADD_TEMPLATE
				.Replace( "${ContextName}", contextName );
		}
	}
}