using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Runtime.InteropServices;
using Entitas;
using GenEntitasLang;
using Ent = GenEntitas.MainEntity;

namespace GenEntitas
{
	[Export(typeof(ISystem))]
	[Guid("8B97F691-DAC8-4AC3-A184-04908579FB82")]
	public class GenEntitasLangToCompsSystem : ReactiveSystem<Ent>
	{
		public				GenEntitasLangToCompsSystem				(  ) : this( Contexts.sharedInstance )
		{
		}

		public				GenEntitasLangToCompsSystem				( Contexts contexts ) : base( contexts.main )
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
				parsers.ParseWithComments( str );
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