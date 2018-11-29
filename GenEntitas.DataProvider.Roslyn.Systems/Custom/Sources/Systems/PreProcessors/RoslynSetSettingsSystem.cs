using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Runtime.InteropServices;
using Entitas;
using Ent = GenEntitas.SettingsEntity;

namespace GenEntitas
{
	[Export(typeof(ISystem))]
	[Guid("C893C745-BED3-48E6-8800-401EA88B8CE0")]
	public class RoslynSetSettingsSystem : ReactiveSystem<Ent>
	{
		public				RoslynSetSettingsSystem	( Contexts contexts ) : base( contexts.settings )
		{
			_contexts			= contexts;
		}

		public				RoslynSetSettingsSystem	(  ) : this( Contexts.sharedInstance )
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

			_contexts.settings.ReplaceRoslynPathToSolution( d.ContainsKey( nameof( RoslynPathToSolution ) )
				? d[nameof( RoslynPathToSolution )].FirstOrDefault(  )
				: "" );
		}
	}
}