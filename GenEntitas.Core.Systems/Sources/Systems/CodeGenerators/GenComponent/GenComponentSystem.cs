using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DesperateDevs.Utils;
using Entitas;
using Ent = GenEntitas.MainEntity;

namespace GenEntitas
{
	public class GenComponentSystem : ReactiveSystem<Ent>
	{
		public				GenComponentSystem		( Contexts contexts ) : base( contexts.main )
		{
			_contexts			= contexts;
		}

		private				Contexts				_contexts;
		private const		String					COMPONENT_TEMPLATE		=
@"[Entitas.CodeGeneration.Attributes.DontGenerate(false)]
public partial class ${FullComponentName} : Entitas.IComponent {
${memberList}
}
";

		protected override	ICollector<Ent>			GetTrigger				( IContext<Ent> context )
		{
			return context.CreateCollector( MainMatcher.AllOf( MainMatcher.Comp ).NoneOf( MainMatcher.DontGenerateComp, MainMatcher.AlreadyImplementedComp, MainMatcher.EventListenerComp ) );
		}

		protected override	Boolean					Filter					( Ent entity )
		{
			return entity.hasComp && !entity.isDontGenerateComp && !entity.isAlreadyImplementedComp;
		}

		protected override	void					Execute					( List<Ent> entities )
		{
			for ( var i = 0; i < entities.Count; i++ )
			{
				var ent				= entities[i];

				var contents		= COMPONENT_TEMPLATE
					.Replace("${FullComponentName}", ent.comp.Name );

				contents = contents
					.Replace(
						"${memberList}",
						ent.hasPublicFieldsComp
							? GenerateMemberAssignmentList( ent.publicFieldsComp.Values )
							: ""
					);

				var filePath		= "Components" + Path.DirectorySeparatorChar + ent.comp.Name.AddComponentSuffix(  ) + ".cs";
				var generatedBy		= GetType(  ).FullName;
				var fileEnt			= _contexts.main.CreateEntity(  );
				fileEnt.AddGeneratedFileComp( filePath, contents.WrapInNamespace( _contexts ), generatedBy );
			}
		}

		private				String				GenerateMemberAssignmentList( List<FieldInfo> memberData )
		{
			return String.Join( "\n", memberData
				.Select( info => $"    public {info.TypeName} {info.FieldName};" )
				.ToArray(  )
			);
        }
	}
}