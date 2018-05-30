using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Entitas;
using Entitas.CodeGeneration.Plugins;
using Ent = GenEntitas.MainEntity;

namespace GenEntitas
{
	public class GenContextEventSystemsSystem : ReactiveSystem<Ent>
	{
		public				GenContextEventSystemsSystem	( Contexts contexts ) : base( contexts.main )
		{
			_contexts			= contexts;
		}

		private				Contexts				_contexts;
		private const		String					TEMPLATE				=
@"public sealed class ${ContextName}EventSystems : Feature {

    public ${ContextName}EventSystems(Contexts contexts) {
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

		protected override	void					Execute					( List<Ent> entities )
		{
			var contextEnts = new Dictionary<String, List<MainEntity>>(  );
			foreach ( var ent in entities )
			{
				foreach ( var contextName in ent.contextNamesComp.Values )
				{
					if ( !contextEnts.ContainsKey( contextName ) )
					{
						contextEnts[contextName] = new List<MainEntity>( );
					}
					contextEnts[contextName].Add( ent );
				}
			}

			var contextNameToDataTuples = new Dictionary<string, List<DataTuple>>();
			foreach (var contextName in contextEnts.Keys.ToArray())
			{
				var orderedEventData = contextEnts[contextName]
					.SelectMany(ent => ent.eventComp.Values.Select(eventData => new DataTuple { Ent = ent, EventInfo = eventData }).ToArray())
					.OrderBy(tuple => tuple.EventInfo.Priority)
					.ThenBy(tuple => tuple.Ent.ComponentName( _contexts ))
					.ToList();

				contextNameToDataTuples.Add(contextName, orderedEventData);
			}

			foreach ( var contextName in contextNameToDataTuples.Keys )
			{
				var dataTuples		= contextNameToDataTuples[contextName].ToArray();

				var filePath		= "Events" + Path.DirectorySeparatorChar + contextName + "EventSystems.cs";
				var contents		= TEMPLATE
					.Replace("${systemsList}", GenerateSystemList(contextName, dataTuples))
					.Replace(contextName);

				var generatedBy		= GetType().FullName;

				var fileEnt			= _contexts.main.CreateEntity(  );
				fileEnt.AddGeneratedFileComp( filePath, contents.WrapInNamespace( _contexts ), generatedBy );
			}
		}

		private				String					GenerateSystemList		( string contextName, DataTuple[] data )
		{
			return string.Join("\n", data
				.SelectMany(tuple => GenerateSystemListForData(contextName, tuple))
				.ToArray());
		}

		private				String[]			GenerateSystemListForData	(string contextName, DataTuple data)
		{
			return data.Ent.contextNamesComp.Values
				.Where(ctxName => ctxName == contextName)
				.Select(ctxName => GenerateAddSystem(ctxName, data))
				.ToArray();
		}

		private				String					GenerateAddSystem		(string contextName, DataTuple data)
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