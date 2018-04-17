using System;
using System.Collections.Generic;
using Entitas;
using Ent = MainEntity;

namespace GenEntitas.Sources
{
	public class GenComponentFlagSystem : ReactiveSystem<Ent>
	{
		public				GenComponentFlagSystem	( Contexts contexts ) : base( contexts.main )
		{
		}

		protected override	ICollector<Ent>			GetTrigger				( IContext<Ent> context )
		{
			return context.CreateCollector( MainMatcher.AllOf( MainMatcher.Comp ).NoneOf( MainMatcher.PublicFieldsComp ) );
		}

		protected override	Boolean					Filter					( Ent entity )
		{
			return entity.hasComp && !entity.hasPublicFieldsComp;
		}

		protected override	void					Execute					( List<Ent> entities )
		{
		}
	}
}
