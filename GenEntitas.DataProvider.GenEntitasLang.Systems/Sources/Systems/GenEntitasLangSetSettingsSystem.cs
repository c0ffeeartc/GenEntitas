using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Runtime.InteropServices;
using Entitas;
using Ent = GenEntitas.SettingsEntity;

namespace GenEntitas
{
	[Export(typeof(IExecuteSystem))]
	[Guid("19410422-FD8A-4FF0-880F-F620657602E8")]
	public class GenEntitasLangSetSettingsSystem : ReactiveSystem<Ent>
	{
		public				GenEntitasLangSetSettingsSystem					( Contexts contexts ) : base( contexts.settings )
		{
			_contexts			= contexts;
		}

		public				GenEntitasLangSetSettingsSystem					(  ) : this( Contexts.sharedInstance )
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

		protected override	void					Execute					( List<Ent> entities )
		{
			var d					= entities[0].settingsDict.Dict;

			_contexts.settings.ReplaceGenEntitasLangPaths( d.ContainsKey( nameof( GenEntitasLangPaths ) )
				? d[nameof( GenEntitasLangPaths )]
				: new List<string>(  ) );
		}
	}
}