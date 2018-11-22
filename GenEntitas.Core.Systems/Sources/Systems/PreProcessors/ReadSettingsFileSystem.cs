using System;
using System.Collections.Generic;
using System.IO;
using Entitas;
using Ent = GenEntitas.SettingsEntity;

namespace GenEntitas
{
	public class ReadSettingsFileSystem : ReactiveSystem<Ent>
	{
		public				ReadSettingsFileSystem	( Contexts contexts ) : base( contexts.settings )
		{
			_contexts			= contexts;
		}

		private				Contexts				_contexts;

		protected override	ICollector<Ent>			GetTrigger				( IContext<Ent> context )
		{
			return context.CreateCollector( SettingsMatcher.SettingsPath );
		}

		protected override	Boolean					Filter					( Ent entity )
		{
			return entity.hasSettingsPath;
		}

		protected override	void					Execute					( List<Ent> entities )
		{
			var path			= _contexts.settings.settingsPath.Value;
			if ( !File.Exists( path ) )
			{
				throw new FileNotFoundException( $"Settings file does not exist: '{path}'" );
			}

			var settingsStr		= File.ReadAllText( path );
			_contexts.settings.ReplaceSettingsParseInput( settingsStr );
		}
	}
}