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
		}
	}
}