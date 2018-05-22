using System;
using System.Collections.Generic;
using Entitas;
using GenEntitasLang;
using Sprache;
using Ent = GenEntitas.MainEntity;

namespace GenEntitas
{
	public class ProvideGenEntitasLangToCompsSystem : ReactiveSystem<Ent>
	{
		public				ProvideGenEntitasLangToCompsSystem				( Contexts contexts ) : base( contexts.main )
		{
			_contexts			= contexts;
		}

		private				Contexts				_contexts;

		protected override	ICollector<Ent>			GetTrigger				( IContext<Ent> context )
		{
			return context.CreateCollector( MainMatcher.GenEntitasLangInputString );
		}

		protected override	Boolean					Filter					( Ent entity )
		{
			return entity.hasGenEntitasLangInputString;
		}

		protected override	void					Execute					( List<Ent> entities )
		{
			var parsers = new GenEntitasLangParser( _contexts );
			foreach ( var ent in entities )
			{
				var str = ent.genEntitasLangInputString.Value;
				parsers.Root.Parse( str );
			}

			var group_			= _contexts.main.GetGroup( MainMatcher.ParsedByGenEntitasLang );
			foreach ( var ent in group_.GetEntities(  ) )
			{
				ProvideUniquePrefix( ent );
			}
		}

		private				void					ProvideUniquePrefix		( Ent ent )
		{
			if ( ent.hasUniquePrefixComp )
			{
				return;
			}
			ent.AddUniquePrefixComp( "is" );
		}
	}
}