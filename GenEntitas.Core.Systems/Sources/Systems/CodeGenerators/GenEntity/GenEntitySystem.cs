using System;
using System.Collections.Generic;
using System.IO;
using Entitas;
using Ent = GenEntitas.MainEntity;

namespace GenEntitas
{
	public class GenEntitySystem : ReactiveSystem<Ent>
	{
		public				GenEntitySystem			( Contexts contexts ) : base( contexts.main )
		{
			_contexts			= contexts;
		}

		private				Contexts				_contexts;
		private const		String					TEMPLATE				=
@"public sealed partial class ${EntityType} : Entitas.Entity {
}
";

		protected override	ICollector<Ent>			GetTrigger				( IContext<Ent> context )
		{
			return context.CreateCollector( MainMatcher.ContextComp );
		}

		protected override	Boolean					Filter					( Ent entity )
		{
			return entity.hasContextComp;
		}

		protected override	void					Execute					( List<Ent> entities )
		{
			for ( var i = 0; i < entities.Count; i++ )
			{
				var ent				= entities[i];
				var contextName		= ent.contextComp.Name;

				var filePath		= contextName + Path.DirectorySeparatorChar + contextName.AddEntitySuffix(  ) + ".cs";
				var contents		= TEMPLATE.Replace( contextName );
				var generatedBy		= GetType(  ).FullName;

				var fileEnt			= _contexts.main.CreateEntity(  );
				fileEnt.AddGeneratedFileComp( filePath, contents.WrapInNamespace( _contexts ), generatedBy );
			}
		}
	}
}
