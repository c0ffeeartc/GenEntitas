using System;
using System.Collections.Generic;
using Entitas;
using Ent = GenEntitas.MainEntity;

namespace GenEntitas
{
	public class ContextEntsProviderSystem : ReactiveSystem<Ent>
	{
		public				ContextEntsProviderSystem( Contexts contexts ) : base( contexts.main )
		{
			_contexts			= contexts;
		}

		private				Contexts				_contexts;

		protected override	ICollector<Ent>			GetTrigger				( IContext<Ent> context )
		{
			return context.CreateCollector( MainMatcher.AllOf( MainMatcher.ContextNamesComp ).NoneOf( MainMatcher.DontGenerateComp ) );
		}

		protected override	Boolean					Filter					( Ent entity )
		{
			return entity.hasContextNamesComp;
		}

		protected override	void					Execute					( List<Ent> entities )
		{
			var contextNames = new HashSet<String>(  );
			foreach ( var ent in entities )
			{
				contextNames.UnionWith( ent.contextNamesComp.Values );
			}

			foreach ( var name in contextNames )
			{
				var ent			= _contexts.main.CreateEntity(  );
				ent.AddContextComp( name );
			}
		}
	}
}