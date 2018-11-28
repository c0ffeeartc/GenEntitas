using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Runtime.InteropServices;
using Entitas;
using Ent = GenEntitas.MainEntity;

namespace GenEntitas
{
	[Export(typeof(IExecuteSystem))]
	[Guid("8283A655-B9BA-4106-ADC8-4245C4CAF059")]
	public class PostProcCleanTargetDirSystem : ReactiveSystem<Ent>
	{
		public				PostProcCleanTargetDirSystem ( Contexts contexts ) : base( contexts.main )
		{
			_contexts			= contexts;
		}

		public		PostProcCleanTargetDirSystem	(  ) : this( Contexts.sharedInstance )
		{
		}

		private				Contexts				_contexts;

		protected override	ICollector<Ent>			GetTrigger				( IContext<Ent> context )
		{
			return context.CreateCollector( MainMatcher.GeneratedFileComp );
		}

		protected override	Boolean					Filter					( Ent entity )
		{
			return entity.hasGeneratedFileComp;
		}

		protected override	void					Execute					( List<Ent> entities )
		{
			if ( _contexts.settings.isRunInDryMode )
			{
				return;
			}

			var generatePath		= Path.Combine( _contexts.settings.generatePath.Value, "Generated" );
			var dirInfo				= new DirectoryInfo( generatePath );
			if ( !dirInfo.Exists )
			{
				return;
			}
			foreach ( var file in dirInfo.GetFiles( ) )
			{
				file.Delete(  );
			}
			foreach ( var dir in dirInfo.GetDirectories( ) )
			{
				dir.Delete( true );
			}
		}
	}
}
