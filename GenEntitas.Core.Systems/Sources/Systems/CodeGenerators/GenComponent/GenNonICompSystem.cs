﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Runtime.InteropServices;
using Entitas;
using Ent = GenEntitas.MainEntity;

namespace GenEntitas
{
	[Export(typeof(ISystem))]
	[Guid("3E6C4884-71E8-4593-8D36-64ECAF7DC525")]
	public class GenNonICompSystem : ReactiveSystem<Ent>
	{
		public				GenNonICompSystem		( Contexts contexts ) : base( contexts.main )
		{
			_contexts			= contexts;
		}

		public				GenNonICompSystem		(  ) : this( Contexts.sharedInstance )
		{
		}

		private				Contexts				_contexts;
		private const		String					COMPONENT_TEMPLATE		=
@"[Entitas.CodeGeneration.Attributes.DontGenerate(false)]
public sealed class ${FullComponentName} : Entitas.IComponent {
    public ${Type} value;
}
";

		protected override	ICollector<Ent>			GetTrigger				( IContext<Ent> context )
		{
			return context.CreateCollector( MainMatcher.AllOf( MainMatcher.NonIComp ).NoneOf( MainMatcher.DontGenerateComp ) );
		}

		protected override	Boolean					Filter					( Ent entity )
		{
			return entity.hasNonIComp && !entity.isDontGenerateComp;
		}

		protected override	void					Execute					( List<Ent> entities )
		{
			for ( var i = 0; i < entities.Count; i++ )
			{
				var ent				= entities[i];

				var filePath		= "Components" + Path.DirectorySeparatorChar + ent.nonIComp.FullCompName + ".cs";
				var contents		= Generate( ent.nonIComp.FullCompName, ent.nonIComp.FieldTypeName );
				var generatedBy		= GetType(  ).FullName;

				var fileEnt			= _contexts.main.CreateEntity(  );
				fileEnt.AddGeneratedFileComp( filePath, contents.WrapInNamespace( _contexts ), generatedBy );
			}
		}

		private				String					Generate				( String fullComponentName, String fieldTypeName )
		{
			return COMPONENT_TEMPLATE
				.Replace("${FullComponentName}", fullComponentName)
				.Replace("${Type}", fieldTypeName );
		}
	}
}
