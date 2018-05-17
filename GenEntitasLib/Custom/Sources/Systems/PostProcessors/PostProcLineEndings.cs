using System;
using System.Collections.Generic;
using Entitas;
using Ent = MainEntity;

namespace GenEntitas
{
	public class PostProcLineEndings : ReactiveSystem<Ent>
	{
		public				PostProcLineEndings		( Contexts contexts ) : base( contexts.main )
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