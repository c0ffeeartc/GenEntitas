using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Runtime.InteropServices;
using Entitas;
using Ent = GenEntitas.MainEntity;

namespace GenEntitas
{
	[Export(typeof(IExecuteSystem))]
	[Guid("DAFB3479-BCDD-4CFB-BA35-5D072FC9FD34")]
	public class PostProcMergeFilesSystem : ReactiveSystem<Ent>
	{
		public				PostProcMergeFilesSystem ( Contexts contexts ) : base( contexts.main )
		{
		}

		public				PostProcMergeFilesSystem(  ) : this( Contexts.sharedInstance )
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
