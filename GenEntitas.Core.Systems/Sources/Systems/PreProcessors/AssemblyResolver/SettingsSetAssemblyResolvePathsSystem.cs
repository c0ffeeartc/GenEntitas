using System;
using System.Collections.Generic;
using Entitas;
using Ent = GenEntitas.SettingsEntity;

namespace GenEntitas
{
	public class SettingsSetAssemblyResolvePathsSystem : ReactiveSystem<Ent>
	{
		public				SettingsSetAssemblyResolvePathsSystem			( Contexts contexts ) : base( contexts.settings )
		{
			_contexts				= contexts;
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
			var d					= _contexts.settings.settingsDict.Dict;

			_contexts.settings.ReplaceAssemblyResolvePaths( d.ContainsKey( nameof( AssemblyResolvePaths ) )
				? d[nameof( AssemblyResolvePaths )]
				: new List<string>(  ) );
		}
	}
}