using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Entitas;

namespace GenEntitas
{
	[Export(typeof(ISystem))]
	[Guid("62A85490-CA0F-4D5B-AB5E-EC8FF7D04979")]
	public class GenCompLookupDictsSystem : ReactiveSystem<MainEntity>
	{
		public			GenCompLookupDictsSystem	( Contexts contexts ) : base( contexts.main )
		{
			_contexts = contexts;
		}

		public			GenCompLookupDictsSystem	(  ) : this( Contexts.sharedInstance )
		{
		}

		private				Contexts				_contexts;

		private const		String					TEMPLATE				=
@"public static partial class ${Lookup}
{
    public static readonly System.Collections.Generic.Dictionary<System.Type,int> TypeToI = new System.Collections.Generic.Dictionary<System.Type,int>()
    {
${kTypeVIndexList}
    };
}
";

		private const		String					K_TYPE_V_INDEX			= @"        { typeof(${ComponentType}), ${Index} },";

		protected override	ICollector<MainEntity>			GetTrigger				( IContext<MainEntity> context )
		{
			return context.CreateCollector( MainMatcher.AllOf( MainMatcher.Comp ).NoneOf( MainMatcher.DontGenerateComp ) );
		}

		protected override	Boolean					Filter					( MainEntity entity )
		{
			return entity.hasComp && !entity.isDontGenerateComp;
		}

		protected override	void					Execute					( List<MainEntity> entities )
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
					.Select((ent, index) => K_TYPE_V_INDEX
						.Replace("${ComponentType}", ent.comp.FullTypeName )
						.Replace("${Index}", index.ToString())).ToArray());

				var contextName			= kv.Key;
				var filePath		= contextName + Path.DirectorySeparatorChar + contextName + "ComponentsLookupDicts.cs";
				var generatedBy		= GetType().FullName;

				var contents = TEMPLATE
					.Replace("${Lookup}", contextName + CodeGeneratorExtentions.LOOKUP)
					.Replace("${kTypeVIndexList}", componentConstantsList);

				var fileEnt			= _contexts.main.CreateEntity(  );
				fileEnt.AddGeneratedFileComp( filePath, contents.WrapInNamespace( _contexts ), generatedBy );
			}
		}
	}
}