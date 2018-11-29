using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Runtime.InteropServices;
using Entitas;
using Ent = GenEntitas.MainEntity;

namespace GenEntitas
{
	[Export(typeof(ISystem))]
	[Guid("46A7731A-6A36-49E4-8A8C-542705BE50E2")]
	public class GenContextAttributeSystem : ReactiveSystem<Ent>
	{
		public			GenContextAttributeSystem	( Contexts contexts ) : base( contexts.main )
		{
			_contexts			= contexts;
		}

		public			GenContextAttributeSystem	(  ) : this( Contexts.sharedInstance )
		{
		}

		private				Contexts				_contexts;
		private const		String					TEMPLATE				=
@"public sealed class ${ContextName}Attribute : Entitas.CodeGeneration.Attributes.ContextAttribute {

    public ${ContextName}Attribute() : base(""${ContextName}"") {
    }
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
				var filePath		= contextName + Path.DirectorySeparatorChar + contextName + "Attribute.cs";
				var contents		= TEMPLATE.Replace( contextName );
				var generatedBy		= GetType(  ).FullName;
				var fileEnt			= _contexts.main.CreateEntity(  );
				fileEnt.AddGeneratedFileComp( filePath, contents.WrapInNamespace( _contexts ), generatedBy );
			}
		}
	}
}