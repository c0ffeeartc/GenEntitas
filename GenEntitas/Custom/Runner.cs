using Entitas;
using GenEntitas.Sources;

namespace GenEntitas
{
	internal class Runner
	{
		private				Contexts				_contexts;
		private				Systems					_systems;
		public				Systems					Systems					=> _systems;

		public				void					Init					(  )
		{
			var main			= Contexts.sharedInstance.main;
			var ent				= main.CreateEntity();

			_systems			= new Systems(  )
				.Add( new GenContextsSystem( _contexts ) )
				.Add( new GenComponentFlagSystem( _contexts ) )
				.Add( new GenComponentSystem( _contexts ) );
		}
	}
}