using System;
using System.Collections.Generic;
using Entitas;

namespace GenEntitas
{
	public class SettingsParseSettingsDictSystem : ReactiveSystem<SettingsEntity>
	{
		public				SettingsParseSettingsDictSystem	( Contexts contexts ) : base( contexts.settings )
		{
			_contexts			= contexts;
		}

		private				Contexts				_contexts;

		protected override	ICollector<SettingsEntity>			GetTrigger				( IContext<SettingsEntity> context )
		{
			return context.CreateCollector( SettingsMatcher.SettingsParseInput );
		}

		protected override	Boolean					Filter					( SettingsEntity entity )
		{
			return entity.hasSettingsParseInput;
		}

		protected override	void					Execute					( List<SettingsEntity> entities )
		{
			var ent					= entities[0];

			var d					= _contexts.Services.Settings.Parse( ent.settingsParseInput.Value );
			_contexts.settings.ReplaceSettingsDict( d );
		}
	}
}