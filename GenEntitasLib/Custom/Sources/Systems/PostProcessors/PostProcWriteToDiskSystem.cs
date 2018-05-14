using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Entitas;
using Ent = MainEntity;

namespace GenEntitas.Sources
{
	public class PostProcWriteToDiskSystem : ReactiveSystem<Ent>
	{
		public				PostProcWriteToDiskSystem ( Contexts contexts ) : base( contexts.main )
		{
			_contexts			= contexts;
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

				if ( _contexts.settings.isConsoleWriteLineGeneratedPaths )
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

			if ( _contexts.settings.isConsoleWriteLineGeneratedPaths )
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