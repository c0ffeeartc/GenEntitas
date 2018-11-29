using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Runtime.InteropServices;
using Entitas;
using Ent = GenEntitas.MainEntity;

namespace GenEntitas
{
	[Export(typeof(ISystem))]
	[Guid("695EB79E-E907-448A-886B-5ADA89E9E690")]
	public class ContextEntsProviderSystem : ReactiveSystem<Ent>
	{
		public				ContextEntsProviderSystem( Contexts contexts ) : base( contexts.main )
		{
			_contexts			= contexts;
		}

		public				ContextEntsProviderSystem(  ) : this( Contexts.sharedInstance )
		{
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