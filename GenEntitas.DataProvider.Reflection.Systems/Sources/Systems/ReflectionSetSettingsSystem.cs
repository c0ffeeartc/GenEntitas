using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Runtime.InteropServices;
using Entitas;
using Ent = GenEntitas.SettingsEntity;

namespace GenEntitas
{
	[Export(typeof(ISystem))]
	[Guid("065BF37D-6843-4BAB-8154-2B1C1A3D8968")]
	public class ReflectionSetSettingsSystem : ReactiveSystem<Ent>
	{
		public				ReflectionSetSettingsSystem	( Contexts contexts ) : base( contexts.settings )
		{
			_contexts			= contexts;
		}

		public				ReflectionSetSettingsSystem	(  ) : this( Contexts.sharedInstance )
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

			_contexts.settings.ReplaceReflectionAssemblyPaths( d.ContainsKey( nameof( ReflectionAssemblyPaths ) )
				? d[nameof( ReflectionAssemblyPaths )]
				: new List<string>(  ) );
		}
	}
}