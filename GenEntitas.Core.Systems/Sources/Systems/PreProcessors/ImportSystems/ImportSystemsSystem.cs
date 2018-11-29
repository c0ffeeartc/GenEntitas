using Entitas;
using Ent = GenEntitas.SettingsEntity;

namespace GenEntitas
{
	public class ImportSystemsSystem : IExecuteSystem
	{
		public				ImportSystemsSystem	( Contexts contexts )
		{
			_contexts			= contexts;
		}

		private				Contexts				_contexts;

		public				void					Execute					(  )
		{
			var import			= new MefImportSystems(  );
			var systems			= import.Import(  );
			_contexts.main.ReplaceImportedSystems( systems );

			// needs retriggering after importing systems from dlls
			_contexts.settings.ReplaceSettingsPath( _contexts.settings.settingsPath.Value );
			_contexts.settings.ReplaceSettingsParseInput( _contexts.settings.settingsParseInput.Value );
			_contexts.settings.ReplaceSettingsDict( _contexts.settings.settingsDict.Dict );
			_contexts.settings.ReplaceSystemGuids( _contexts.settings.systemGuids.Values );
		}
	}
}