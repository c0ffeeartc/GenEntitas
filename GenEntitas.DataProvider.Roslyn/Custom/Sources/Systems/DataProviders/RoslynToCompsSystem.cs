using System;
using System.Collections.Generic;
using Entitas;
using Ent = GenEntitas.MainEntity;

namespace GenEntitas.DataProvider.Roslyn
{
	public class RoslynToCompsSystem : ReactiveSystem<Ent>
	{
		public				RoslynToCompsSystem	( Contexts contexts ) : base( contexts.main )
		{
			_contexts			= contexts;
		}

		private				Contexts				_contexts;

		protected override	ICollector<Ent>			GetTrigger				( IContext<Ent> context )
		{
			return context.CreateCollector( MainMatcher.RoslynComponentTypes );
		}

		protected override	Boolean					Filter					( Ent ent )
		{
			return ent.hasRoslynComponentTypes;
		}

		protected override	void					Execute					( List<Ent> ents )
		{
			var typeSymbols = ents[0].roslynComponentTypes.Values;
		}
	}
}