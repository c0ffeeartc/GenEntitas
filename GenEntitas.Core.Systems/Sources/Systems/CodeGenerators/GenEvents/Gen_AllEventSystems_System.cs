using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Entitas;
using Ent = GenEntitas.MainEntity;

namespace GenEntitas
{
	[Export(typeof(ISystem))]
	[Guid("28160892-AD0E-4080-BC2F-2D356926CD06")]
	public class Gen_AllEventSystems_System : ReactiveSystem<Ent>
	{
		public				Gen_AllEventSystems_System	( Contexts contexts ) : base( contexts.main )
		{
			_contexts			= contexts;
		}

		public		Gen_AllEventSystems_System	(  ) : this( Contexts.sharedInstance )
		{
		}

		private				Contexts				_contexts;
		private const		String					TEMPLATE				=
@"public sealed class AllEventSystems : Feature {

    public AllEventSystems(Contexts contexts) {
${systemsList}
    }
}
";

		private const		String					SYSTEM_ADD_TEMPLATE		= @"        Add(new ${Event}EventSystem(contexts)); // priority: ${priority}";

		protected override	ICollector<Ent>			GetTrigger				( IContext<Ent> context )
		{
			return context.CreateCollector( MainMatcher.AllOf( MainMatcher.Comp, MainMatcher.EventComp ) );
		}

		protected override	Boolean					Filter					( Ent entity )
		{
			return entity.hasComp && entity.hasEventComp;
		}

		protected override	void					Execute					( List<Ent> eventEnts )
		{
			var orderedEventData = eventEnts
				.SelectMany(ent => ent.eventComp.Values.Select(eventData => new DataTuple { Ent = ent, EventInfo = eventData }).ToArray())
				.OrderBy(tuple => tuple.EventInfo.Priority)
				.ThenBy(tuple => tuple.Ent.ComponentName( _contexts ))
				.ToArray();

			{
				var filePath		= "Events" + Path.DirectorySeparatorChar + "AllEventSystems.cs";
				var contents		= TEMPLATE
					.Replace("${systemsList}", GenerateSystemList( orderedEventData ) )
					;

				var generatedBy		= GetType().FullName;

				var fileEnt			= _contexts.main.CreateEntity(  );
				fileEnt.AddGeneratedFileComp( filePath, contents.WrapInNamespace( _contexts ), generatedBy );
			}
		}

		private				String					GenerateSystemList		( DataTuple[] data )
		{
			return string.Join("\n", data
				.SelectMany( tuple => GenerateSystemListForData( tuple ) )
				.ToArray());
		}

		private				String[]			GenerateSystemListForData	( DataTuple data )
		{
			return data.Ent.contextNamesComp.Values
				.Select(ctxName => GenerateAddSystem(ctxName, data))
				.ToArray();
		}

		private				String					GenerateAddSystem		(string contextName, DataTuple data )
		{
			return SYSTEM_ADD_TEMPLATE
				.Replace( _contexts, data.Ent, contextName, data.EventInfo)
				.Replace("${priority}", data.EventInfo.Priority.ToString())
				.Replace("${Event}", data.Ent.Event( _contexts, contextName, data.EventInfo));
		}

		struct DataTuple
		{
			public MainEntity Ent;
			public EventInfo EventInfo;
		}
	}
}