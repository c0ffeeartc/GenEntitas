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
			return context.CreateCollector( SettingsMatcher.AssemblyResolvePaths );
		}

		protected override	Boolean					Filter					( Ent entity )
		{
			return entity.hasAssemblyResolvePaths;
		}

		protected override	void					Execute					( List<Ent> entities )
		{
			var ent = entities[0];
			if ( ent.assemblyResolvePaths.Value.Count == 0 )
			{
				return;
			}
			new AssemblyResolver( ent.assemblyResolvePaths.Value.ToArray(  ) );
		}
	}
}