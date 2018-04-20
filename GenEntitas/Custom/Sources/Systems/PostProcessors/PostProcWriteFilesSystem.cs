using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Entitas;
using Ent = MainEntity;

namespace GenEntitas.Sources
{
	public class PostProcWriteFilesSystem : ReactiveSystem<Ent>
	{
		public				PostProcWriteFilesSystem ( Contexts contexts ) : base( contexts.main )
		{
			_contexts			= contexts;
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
			var generatePath		= Path.Combine( _contexts.settings.generatePath.Value, "Generated" );
			foreach ( var ent in entities )
			{
				var targetPath		= Path.Combine( generatePath, ent.generatedFileComp.FilePath );
				var dirPath			= Path.GetDirectoryName( targetPath );
				if ( dirPath != null && !Directory.Exists( dirPath ) )
				{
					Directory.CreateDirectory( dirPath );
				}
				File.WriteAllText( targetPath, ent.generatedFileComp.Contents, Encoding.UTF8 );
				Console.WriteLine( targetPath );
			}
		}
	}
}