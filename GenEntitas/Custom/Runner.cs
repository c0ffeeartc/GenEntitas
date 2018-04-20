﻿using Entitas;
using GenEntitas.Sources;

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
				.Add( new GenContextsSystem( _contexts ) )
				.Add( new GenContextsObserverSystem( _contexts ) )

				.Add( new GenFeatureClassSystem( _contexts ) )

				.Add( new GenContextAttributeSystem( _contexts ) )
				.Add( new GenContextMatcherSystem( _contexts ) )
				.Add( new GenContextSystem( _contexts ) )

				.Add( new GenCompContextApiSystem( _contexts ) )
				.Add( new GenFlagCompContextApiSystem( _contexts ) )

				.Add( new GenNonICompSystem( _contexts ) )

				.Add( new PostProcAddFileHeaderSystem( _contexts ) )
				.Add( new PostProcCleanTargetDirSystem( _contexts ) )
				.Add( new PostProcWriteToDiskSystem( _contexts ) );
		}
	}
}