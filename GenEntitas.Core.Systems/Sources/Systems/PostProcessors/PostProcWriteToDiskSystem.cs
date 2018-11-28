using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Entitas;
using Ent = GenEntitas.MainEntity;

namespace GenEntitas
{
	[Export(typeof(IExecuteSystem))]
	[Guid("B4D95DA8-FB2E-4491-9CEA-02A24A8A6C88")]
	public class PostProcWriteToDiskSystem : ReactiveSystem<Ent>
	{
		public				PostProcWriteToDiskSystem ( Contexts contexts ) : base( contexts.main )
		{
			_contexts			= contexts;
		}

		public				PostProcWriteToDiskSystem(  ) : this( Contexts.sharedInstance )
		{
		}

		private				Contexts				_contexts;

		protected override	ICollector<Ent>			GetTrigger				( IContext<Ent> context )
		{
			return context.CreateCollector( MainMatcher.AllOf( MainMatcher.GeneratedFileComp ).NoneOf( MainMatcher.Destroy ) );
		}

		protected override	Boolean					Filter					( Ent entity )
		{
			return entity.hasGeneratedFileComp && !entity.isDestroy;
		}

		protected override	void					Execute					( List<Ent> entities )
		{
			var generatePath		= Path.Combine( _contexts.settings.generatePath.Value, "Generated" );
			var stringBuilder	= new StringBuilder(  );
			foreach ( var ent in entities )
			{
				var targetPath		= Path.Combine( generatePath, ent.generatedFileComp.FilePath );

				if ( _contexts.settings.isLogGeneratedPaths )
				{
					stringBuilder.Append( targetPath );
					stringBuilder.Append( " - " );
					stringBuilder.Append( ent.generatedFileComp.GeneratedBy );
					stringBuilder.Append( "\n" );
				}

				if ( _contexts.settings.isRunInDryMode )
				{
					continue;
				}

				var dirPath			= Path.GetDirectoryName( targetPath );
				if ( dirPath != null && !Directory.Exists( dirPath ) )
				{
					Directory.CreateDirectory( dirPath );
				}
				File.WriteAllText( targetPath, ent.generatedFileComp.Contents );
			}

			if ( _contexts.settings.isLogGeneratedPaths )
			{
				var s = stringBuilder.ToString(  );
				Console.Write( s );
#if UNITY_EDITOR
				UnityEngine.Debug.Log( s );
#endif
			}
		}
	}
}