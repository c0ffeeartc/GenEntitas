using System;
using System.Collections.Generic;
using Entitas;
using Ent = GenEntitas.SettingsEntity;

namespace GenEntitas
{
	public class AssemblyResolveSystem : ReactiveSystem<Ent>
	{
		public				AssemblyResolveSystem	( Contexts contexts ) : base( contexts.settings )
		{
		}

		protected override	ICollector<Ent>			GetTrigger				( IContext<Ent> context )
		{
			return context.CreateCollector( SettingsMatcher.SearchPaths );
		}

		protected override	Boolean					Filter					( Ent entity )
		{
			return entity.hasSearchPaths;
		}

		protected override	void					Execute					( List<Ent> entities )
		{
			var ent = entities[0];
			if ( ent.searchPaths.Value.Count == 0 )
			{
				return;
			}
			new AssemblyResolver( ent.searchPaths.Value.ToArray(  ) );
		}
	}
}