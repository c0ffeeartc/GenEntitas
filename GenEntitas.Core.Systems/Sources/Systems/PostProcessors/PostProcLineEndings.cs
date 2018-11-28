using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Runtime.InteropServices;
using Entitas;
using Ent = GenEntitas.MainEntity;

namespace GenEntitas
{
	[Export(typeof(IExecuteSystem))]
	[Guid("16FCAE59-CEBD-4E32-95B4-A195015FD14F")]
	public class PostProcLineEndings : ReactiveSystem<Ent>
	{
		public				PostProcLineEndings		( Contexts contexts ) : base( contexts.main )
		{
		}

		public				PostProcLineEndings		(  ) : this( Contexts.sharedInstance )
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
			foreach ( var ent in entities )
			{
			
        		ent.generatedFileComp.Contents = ent.generatedFileComp.Contents.Replace("\n", Environment.NewLine);
				ent.ReplaceGeneratedFileComp(
					ent.generatedFileComp.FilePath,
					ent.generatedFileComp.Contents,
					ent.generatedFileComp.GeneratedBy );
			}
		}
	}
}