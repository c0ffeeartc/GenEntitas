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
				.Add( new ReadSettingsFileSystem( _contexts ) )
				.Add( new ParseSettingsStrSystem( _contexts ) )
				.Add( new AssemblyResolveSystem( _contexts ) )

				.Add( new ReflectionToTypesSystem( _contexts ) )
				.Add( new ReflectionToCompsSystem( _contexts ) )
				.Add( new ReflectionToEntityIndexSystem( _contexts ) )

				.Add( new ReadGenEntitasLangFilesSystem( _contexts ) )
				.Add( new ProvideGenEntitasLangToCompsSystem( _contexts ) )

				.Add( new ContextEntsProviderSystem( _contexts ) )

				.Add( new GenContextsSystem( _contexts ) )
				.Add( new GenContextsObserverSystem( _contexts ) )

				.Add( new GenFeatureClassSystem( _contexts ) )

//				.Add( new GenContextAttributeSystem( _contexts ) )
				.Add( new GenContextMatcherSystem( _contexts ) )
				.Add( new GenContextSystem( _contexts ) )

				.Add( new GenEntitySystem( _contexts ) )
				.Add( new GenEntityIndexSystem( _contexts ) )

				.Add( new GenComponentSystem( _contexts ) )
				.Add( new GenCompLookupSystem( _contexts ) )
				.Add( new GenCompContextApiSystem( _contexts ) )
				.Add( new GenCompEntityApiSystem( _contexts ) )
				.Add( new GenCompEntityApiInterfaceSystem( _contexts ) )
				.Add( new GenCompMatcherApiSystem( _contexts ) )

				.Add( new GenNonICompSystem( _contexts ) )

				.Add( new GenEventEntityApiSystem( _contexts ) )
				.Add( new GenEventListenerCompSystem( _contexts ) )
				.Add( new GenEventListenerInterfaceSystem( _contexts ) )
				.Add( new GenEventSystemSystem( _contexts ) )
				.Add( new GenContextEventSystemsSystem( _contexts ) )
				.Add( new GenEventSystemsSystem( _contexts ) )

				//.Add( new PostProcAddFileHeaderSystem( _contexts ) )
				.Add( new PostProcMergeFilesSystem( _contexts ) )
				.Add( new PostProcLineEndings( _contexts ) )
//				.Add( new PostProcCleanTargetDirSystem( _contexts ) )
//				.Add( new PostProcWriteToDiskSystem( _contexts ) )
				.Add( new PostProcApplyDiffToDiskSystem( _contexts ) )
				.Add( new PostProcWriteGenPathsToCsprojSystem( _contexts ) )

				.Add( new DestroySystem( _contexts ) )
				;
		}
	}
}