using Entitas;

namespace GenEntitas
{
	internal class Runner
	{
		private				Contexts				_contexts;
		private				Systems					_systems;
		public				Contexts				Contexts				=> _contexts;
		public				Systems					Systems					=> _systems;

		public				void					Init					(  )
		{
			_contexts			= Contexts.sharedInstance;

			_systems			= new Systems(  )
				.Add( new SettingsReadFileSystem( _contexts ) )

				.Add( new SystemGuidsSettingsParseStrSystem( _contexts ) )
				.Add( new ImportSystemsSystem( _contexts ) )

				.Add( new SettingsParseStrSystem( _contexts ) )
				.Add( new AssemblyResolveSystem( _contexts ) )
				.Add( new ExecuteImportedSystemsSystem( _contexts ) )

				.Add( new DestroySystem( _contexts ) )
				;
		}
	}
}