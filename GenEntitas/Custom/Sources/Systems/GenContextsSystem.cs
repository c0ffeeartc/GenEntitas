using System;
using System.Collections.Generic;
using Entitas;
using Ent = MainEntity;


namespace GenEntitas.Sources
{
	public class GenContextsSystem : ReactiveSystem<Ent>
	{
		public				GenContextsSystem	( Contexts contexts ) : base( contexts.main )
		{
		}

		protected override	ICollector<Ent>			GetTrigger				( IContext<Ent> context )
		{
			return context.CreateCollector( MainMatcher.Comp );
		}

		protected override	Boolean					Filter					( Ent entity )
		{
			return entity.hasComp;
		}

		protected override	void					Execute					( List<Ent> entities )
		{
		}
	}
}