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
	[Guid("EAE3F8D4-5E36-46BB-89B0-ECF8C691BB18")]
	public class GenContextSystem : ReactiveSystem<Ent>
	{
		public				GenContextSystem		( Contexts contexts ) : base( contexts.main )
		{
			_contexts			= contexts;
		}

		public				GenContextSystem		(  ) : this( Contexts.sharedInstance )
		{
		}

		private				Contexts				_contexts;
		private const		String					TEMPLATE				=
            @"public sealed partial class ${ContextType} : Entitas.Context<${EntityType}> {

    public ${ContextType}()
        : base(
            ${Lookup}.TotalComponents,
            0,
            new Entitas.ContextInfo(
                ""${ContextName}"",
                ${Lookup}.componentNames,
                ${Lookup}.componentTypes
            ),
            (entity) =>

#if (ENTITAS_FAST_AND_UNSAFE)
                new Entitas.UnsafeAERC(),
#else
                new Entitas.SafeAERC(entity),
#endif
            () => new ${EntityType}()
        ) {
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

				var filePath		= contextName + Path.DirectorySeparatorChar + contextName.AddContextSuffix(  ) + ".cs";
				var contents		= TEMPLATE.Replace( contextName );
				var generatedBy		= GetType(  ).FullName;

				var fileEnt			= _contexts.main.CreateEntity(  );
				fileEnt.AddGeneratedFileComp( filePath, contents.WrapInNamespace( _contexts ), generatedBy );
			}
		}
	}
}
