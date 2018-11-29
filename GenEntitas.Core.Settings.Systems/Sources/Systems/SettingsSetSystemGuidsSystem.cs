using System;
using System.Collections.Generic;
using Entitas;

namespace GenEntitas
{
	public class SettingsSetSystemGuidsSystem : ReactiveSystem<SettingsEntity>
	{
		public				SettingsSetSystemGuidsSystem	( Contexts contexts ) : base( contexts.settings )
		{
			_contexts			= contexts;
		}

		private				Contexts				_contexts;

		protected override	ICollector<SettingsEntity>			GetTrigger				( IContext<SettingsEntity> context )
		{
			return context.CreateCollector( SettingsMatcher.SettingsDict );
		}

		protected override	Boolean					Filter					( SettingsEntity entity )
		{
			return entity.hasSettingsDict;
		}

		protected override	void					Execute					( List<SettingsEntity> entities )
		{
			var ent					= entities[0];

			var d					= ent.settingsDict.Dict;

			var guids				= new List<Guid>(  );
			if ( d.ContainsKey( nameof( SystemGuids ) ) )
			{
				foreach ( var str in d[ nameof ( SystemGuids )])
				{
					var guid	= new Guid( str );
					guids.Add( guid );
				}
			}
			_contexts.settings.ReplaceSystemGuids( guids ); 
		}
	}
}