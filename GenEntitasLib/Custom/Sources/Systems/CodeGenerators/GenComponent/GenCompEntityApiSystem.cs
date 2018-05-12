﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DesperateDevs.Utils;
using Entitas;
using Entitas.CodeGeneration.Plugins;
using Ent = MainEntity;

namespace GenEntitas.Sources
{
	public class GenCompEntityApiSystem : ReactiveSystem<Ent>
	{
		public				GenCompEntityApiSystem	( Contexts contexts ) : base( contexts.main )
		{
			_contexts = contexts;
		}

		private				Contexts				_contexts;

        private const		String					STANDARD_TEMPLATE		=
@"public partial class ${EntityType} {

    public ${ComponentType} ${componentName} { get { return (${ComponentType})GetComponent(${Index}); } }
    public bool has${ComponentName} { get { return HasComponent(${Index}); } }

    public void Add${ComponentName}(${newMethodParameters}) {
        var index = ${Index};
        var component = CreateComponent<${ComponentType}>(index);
${memberAssignmentList}
        AddComponent(index, component);
    }

    public void Replace${ComponentName}(${newMethodParameters}) {
        var index = ${Index};
        var component = CreateComponent<${ComponentType}>(index);
${memberAssignmentList}
        ReplaceComponent(index, component);
    }

    public void Remove${ComponentName}() {
        RemoveComponent(${Index});
    }
}
";

		private const		String					FLAG_TEMPLATE			=
            @"public partial class ${EntityType} {

    static readonly ${ComponentType} ${componentName}Component = new ${ComponentType}();

    public bool ${prefixedComponentName} {
        get { return HasComponent(${Index}); }
        set {
            if (value != ${prefixedComponentName}) {
                var index = ${Index};
                if (value) {
                    var componentPool = GetComponentPool(index);
                    var component = componentPool.Count > 0
                            ? componentPool.Pop()
                            : ${componentName}Component;

                    AddComponent(index, component);
                } else {
                    RemoveComponent(index);
                }
            }
        }
    }
}
";

		protected override	ICollector<Ent>			GetTrigger				( IContext<Ent> context )
		{
			return context.CreateCollector( MainMatcher.AllOf( MainMatcher.Comp ).NoneOf( MainMatcher.DontGenerateComp ) );
		}

		protected override	Boolean					Filter					( Ent entity )
		{
			return entity.hasComp && !entity.isDontGenerateComp;
		}

		protected override	void					Execute					( List<Ent> entities )
		{
			foreach ( var ent in entities )
			{
				var contextNames = ent.contextNamesComp.Values;
				foreach ( var contextName in contextNames )
				{
					var template		= ent.hasPublicFieldsComp ? STANDARD_TEMPLATE : FLAG_TEMPLATE;

					var filePath		= contextName + Path.DirectorySeparatorChar + "Components" + Path.DirectorySeparatorChar + contextName + ent.comp.Name.AddComponentSuffix(  ) + ".cs";

					var contents = template
						.Replace( ent, contextName );

					if ( ent.hasPublicFieldsComp )
					{
						contents = contents
							.Replace( "${memberAssignmentList}", GenerateMemberAssignmentList( ent.publicFieldsComp.Values.ToArray(  ) ) );
					}

					var generatedBy		= GetType().FullName;

					var fileEnt			= _contexts.main.CreateEntity(  );
					fileEnt.AddGeneratedFileComp( filePath, contents, generatedBy );
				}
			}
		}

		private				String				GenerateMemberAssignmentList( FieldInfo[] memberData )
		{
			return String.Join("\n", memberData
				.Select(info => "        component." + info.FieldName + " = new" + info.FieldName.UppercaseFirst() + ";")
				.ToArray()
			);
        }
	}
}