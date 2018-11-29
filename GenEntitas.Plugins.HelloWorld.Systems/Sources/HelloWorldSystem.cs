using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Runtime.InteropServices;
using Entitas;
using Ent = GenEntitas.SettingsEntity;

namespace GenEntitas
{
	[Export(typeof(ISystem))]
	[Guid("598BE062-3115-495A-9451-F924856BB2D2")]
	public class HelloWorldSystem : ReactiveSystem<Ent>
	{
		public				HelloWorldSystem	( Contexts contexts ) : base( contexts.settings )
		{
			_contexts			= contexts;
		}

		public				HelloWorldSystem	(  ) : this( Contexts.sharedInstance )
		{
		}

		private				Contexts				_contexts;

		protected override	ICollector<Ent>			GetTrigger				( IContext<Ent> context )
		{
			return context.CreateCollector( SettingsMatcher.SettingsDict );
		}

		protected override	Boolean					Filter					( Ent entity )
		{
			return entity.hasSettingsDict;
		}

		protected override	void					Execute					( List<Ent> ents )
		{
			var dict				= _contexts.settings.settingsDict.Dict;
			var key					= "HelloWorld";
			Console.WriteLine( ">> Hello world system" );

			if( !dict.ContainsKey( key ) )
			{
				return;
			}

			foreach ( var s in dict[key] )
			{
				Console.WriteLine( s );
			}
		}
	}
}