using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Entitas;
using Ent = GenEntitas.MainEntity;

namespace GenEntitas
{
	[Export(typeof(ISystem))]
	[Guid("67E5AA92-AFDA-4EE3-AA24-2589129AE8F0")]
	public class GenCompLookupSystem : ReactiveSystem<Ent>
	{
		public				GenCompLookupSystem	( Contexts contexts ) : base( contexts.main )
		{
			_contexts = contexts;
		}

		public				GenCompLookupSystem		(  ) : this( Contexts.sharedInstance )
		{
		}

		private				Contexts				_contexts;

		private const		String					TEMPLATE				=
@"public static partial class ${Lookup} {

${componentConstantsList}

${totalComponentsConstant}

    public static readonly string[] componentNames = {
${componentNamesList}
    };

    public static readonly System.Type[] componentTypes = {
${componentTypesList}
    };
}
";

		private const		String				COMPONENT_CONSTANT_TEMPLATE = @"    public const int ${ComponentName} = ${Index};";
		private const		String		TOTAL_COMPONENTS_CONSTANT_TEMPLATE	= @"    public const int TotalComponents = ${totalComponents};";
		private const		String					COMPONENT_NAME_TEMPLATE = @"        ""${ComponentName}""";
		private const		String					COMPONENT_TYPE_TEMPLATE = @"        typeof(${ComponentType})";

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
			var contextEnts = new Dictionary<String, List<MainEntity>>(  );
			foreach ( var ent in entities )
			{
				foreach ( var contextName in ent.contextNamesComp.Values )
				{
					if ( !contextEnts.ContainsKey( contextName ) )
					{
						contextEnts[contextName] = new List<MainEntity>( );
					}
					contextEnts[contextName].Add( ent );
				}
			}

			foreach (var contextName in contextEnts.Keys.ToArray())
			{
				contextEnts[contextName] = contextEnts[contextName]
					.OrderBy( ent => ent.comp.FullTypeName)
					.ToList();
			}


			foreach ( var kv in contextEnts )
			{
				var ents = kv.Value;

				var componentConstantsList = string.Join("\n", ents.ToArray()
					.Select((ent, index) => COMPONENT_CONSTANT_TEMPLATE
						.Replace("${ComponentName}", ent.ComponentName( _contexts ) )
						.Replace("${Index}", index.ToString())).ToArray());

				var totalComponentsConstant = TOTAL_COMPONENTS_CONSTANT_TEMPLATE
					.Replace("${totalComponents}", ents.Count.ToString());

				var componentNamesList = string.Join(",\n", ents
					.Select(ent => COMPONENT_NAME_TEMPLATE
						.Replace("${ComponentName}", ent.ComponentName( _contexts ))
					).ToArray());

				var componentTypesList = string.Join(",\n", ents.ToArray()
					.Select(ent => COMPONENT_TYPE_TEMPLATE
						.Replace("${ComponentType}", ent.comp.FullTypeName)
					).ToArray());

				var contextName			= kv.Key;
					var filePath		= contextName + Path.DirectorySeparatorChar + contextName + "ComponentsLookup.cs";
					var generatedBy		= GetType().FullName;

				var contents = TEMPLATE
					.Replace("${Lookup}", contextName + CodeGeneratorExtentions.LOOKUP)
					.Replace("${componentConstantsList}", componentConstantsList)
					.Replace("${totalComponentsConstant}", totalComponentsConstant)
					.Replace("${componentNamesList}", componentNamesList)
					.Replace("${componentTypesList}", componentTypesList);

				var fileEnt			= _contexts.main.CreateEntity(  );
				fileEnt.AddGeneratedFileComp( filePath, contents.WrapInNamespace( _contexts ), generatedBy );
			}
		}
	}
}