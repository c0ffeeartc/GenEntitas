using Entitas;

namespace GenEntitas
{
	internal class Runner
	{
		private				Contexts				_contexts;
		private				Systems					_bootstrapSystems;
		private				Systems					_systems;
		public				Contexts				Contexts				=> _contexts;
		public				Systems					BootstrapSystems		=> _bootstrapSystems;
		public				Systems					Systems					=> _systems;

		public				void					Init					(  )
		{
			_contexts			= Contexts.sharedInstance;

			_contexts.Services	= new Services(  )
			{
				Settings		= new SettingsGrammar( _contexts ),
			};

			_systems			= new Systems(  );

			_bootstrapSystems	= new Systems(  )
				.Add( new SettingsReadFileSystem( _contexts ) )
				.Add( new SettingsParseSettingsDictSystem( _contexts ) )
				.Add( new SettingsSetSystemGuidsSystem( _contexts ) )
				.Add( new ImportSystemsSystem( _contexts ) )
				.Add( new SetSystemsOrderedSystem( _contexts, _systems ) )
				.Add( new SettingsSetCoreSettingsSystem( _contexts ) )
				.Add( new AssemblyResolveSystem( _contexts ) )
				.Add( new DestroySystem( _contexts ) );
				;
		}
	}
}