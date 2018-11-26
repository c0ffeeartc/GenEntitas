using System;
using System.Collections.Generic;
using System.IO;
using Entitas;
using Sprache;
using Ent = GenEntitas.SettingsEntity;

namespace GenEntitas
{
	public class SettingsParseStrSystem : ReactiveSystem<Ent>
	{
		public				SettingsParseStrSystem	( Contexts contexts ) : base( contexts.settings )
		{
			_contexts			= contexts;
		}

		private				Contexts				_contexts;

		protected override	ICollector<Ent>			GetTrigger				( IContext<Ent> context )
		{
			return context.CreateCollector( SettingsMatcher.SettingsParseInput );
		}

		protected override	Boolean					Filter					( Ent entity )
		{
			return entity.hasSettingsParseInput;
		}

		protected override	void					Execute					( List<Ent> entities )
		{
			var ent = entities[0];
			
			var parser = new SettingsGrammar( _contexts );
			parser.ParseWithComments( ent.settingsParseInput.Value );

			if ( !Directory.Exists( _contexts.settings.generatePath.Value ) )
			{
				throw new DirectoryNotFoundException( $"Generate path does not exist: '{_contexts.settings.generatePath.Value}'" );
			}
		}
	}
}