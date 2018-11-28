using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Entitas;
using Ent = GenEntitas.SettingsEntity;

namespace GenEntitas
{
	public class ExecuteImportedSystemsSystem : ReactiveSystem<Ent>
	{
		public				ExecuteImportedSystemsSystem		( Contexts contexts ) : base( contexts.settings )
		{
			_contexts				= contexts;
		}

		private				Contexts				_contexts;

		protected override	ICollector<Ent>			GetTrigger				( IContext<Ent> context )
		{
			return context.CreateCollector( SettingsMatcher.SystemGuids );
		}

		protected override	Boolean					Filter					( Ent entity )
		{
			return entity.hasSystemGuids;
		}

		protected override	void					Execute					( List<Ent> entities )
		{
			if ( _contexts.settings.systemGuids.Values.Count == 0 )
			{
				throw new Exception( "No GUIDs provided in setting `SystemGuids`" );
			}

			var systems				= GetRunSystems(  );
			foreach ( var system in systems )
			{
				Console.WriteLine( system.GetType(  ).Name );
				system.Execute(  );
			}
		}

		private				List<IExecuteSystem>	GetRunSystems			(  )
		{
			var systems				= new List<IExecuteSystem>(  );

			var importedSystems		= _contexts.main.importedSystems.Values;
			var importedGuids		= new List<Guid>(  );
			for ( var i = 0; i < importedSystems.Count; i++ )
			{
				var s				= importedSystems[i];
				var guidAttr		= (GuidAttribute)s.GetType(  ).GetCustomAttributes( typeof( GuidAttribute ), false ).FirstOrDefault(  );
				if ( guidAttr == null )
				{
					throw new Exception( "No GuidAttribute in system: " + s.GetType(  ).Name );
				}

				var guid			= new Guid( guidAttr.Value );
				importedGuids.Add( guid );
			}

			ThrowOnDuplicateGuid( importedSystems, importedGuids );

			var guids				= _contexts.settings.systemGuids.Values;
			for ( var i = 0; i < guids.Count; i++ )
			{
				var guid			= guids[i];
				var systemI			= importedGuids.FindIndex( g => g == guid );
				if ( systemI < 0 )
				{
					throw new Exception( "Not found system with GUID: " + guid );
				}
				systems.Add( importedSystems[systemI] );
			}

			return systems;
		}

		private				void					ThrowOnDuplicateGuid	( List<IExecuteSystem> systems, List<Guid> guids )
		{
			var guidSet				= new HashSet<Guid>(  );
			for ( var i = 0; i < guids.Count; i++ )
			{
				var guid			= guids[i];
				if ( !guidSet.Contains( guid ) )
				{
					guidSet.Add( guid );
					continue;
				}
				var sb				= new StringBuilder(  );
				for ( var j = 0; j < systems.Count; j++ )
				{
					if ( guid != guids[j] )
					{
						continue;
					}
					sb.Append( systems[j].GetType(  ).Name );
					sb.Append( "\n" );
				}
				throw new Exception( "Duplicate GUID found " + guid + " in systems: \n" + sb );
			}
		}
	}
}