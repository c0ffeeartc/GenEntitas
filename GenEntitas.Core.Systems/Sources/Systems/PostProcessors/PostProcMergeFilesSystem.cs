using System;
using System.Collections.Generic;
using Entitas;
using Ent = GenEntitas.MainEntity;

namespace GenEntitas
{
	public class PostProcMergeFilesSystem : ReactiveSystem<Ent>
	{
		public				PostProcMergeFilesSystem ( Contexts contexts ) : base( contexts.main )
		{
		}

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
			var filePathToEnt = new Dictionary<String, Ent>(  );
			foreach ( var ent in entities )
			{
				var filePath = ent.generatedFileComp.FilePath;
				if ( filePathToEnt.ContainsKey( filePath ) )
				{
					var prevEnt			= filePathToEnt[filePath];
					filePathToEnt[filePath].ReplaceGeneratedFileComp( 
						filePath,
						prevEnt.generatedFileComp.Contents + "\n" + ent.generatedFileComp.Contents,
						prevEnt.generatedFileComp.GeneratedBy + ", " + ent.generatedFileComp.GeneratedBy
					 );
					 ent.isDestroy		= true;
					continue;
				}
				filePathToEnt[filePath] = ent;
			}
		}
	}
}
