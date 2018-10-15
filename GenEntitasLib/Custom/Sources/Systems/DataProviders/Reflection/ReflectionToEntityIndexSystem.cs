using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DesperateDevs.Utils;
using Entitas;
using Entitas.CodeGeneration.Attributes;
using Entitas.CodeGeneration.Plugins;
using Ent = GenEntitas.MainEntity;

namespace GenEntitas
{
	public class ReflectionToEntityIndexSystem : ReactiveSystem<Ent>
	{
		public				ReflectionToEntityIndexSystem	( Contexts contexts ) : base( contexts.main )
		{
			_contexts			= contexts;
		}

		private				Contexts				_contexts;

		protected override	ICollector<Ent>			GetTrigger				( IContext<Ent> context )
		{
			return context.CreateCollector( MainMatcher.ReflectionLoadableTypes );
		}

		protected override	Boolean					Filter					( Ent entity )
		{
			return entity.hasReflectionLoadableTypes;
		}

		protected override	void					Execute					( List<Ent> entities )
		{
			var types				= _contexts.main.reflectionLoadableTypes.Values;

			var typeTodEntIndexData = new Dictionary<String, List<EntityIndexData>>(  );
			var entityIndexDatas	= GetData( types );
			foreach ( var data in entityIndexDatas )
			{
				var key				= data.GetComponentType(  );
				if ( !typeTodEntIndexData.ContainsKey( key ) )
				{
					typeTodEntIndexData[key] = new List<EntityIndexData>(  );
				}
				typeTodEntIndexData[key].Add( data );
			}

			var compEnts				= _contexts.main.GetGroup( MainMatcher.AllOf( MainMatcher.Comp, MainMatcher.PublicFieldsComp ) ).GetEntities(  );
			foreach( var ent in compEnts )
			{
				var compTypeName = ent.comp.FullTypeName;
				if ( !typeTodEntIndexData.ContainsKey( compTypeName ) )
				{
					continue;
				}

				var entIndexDatas		= typeTodEntIndexData[ent.typeComp.Value.ToCompilableString(  )];
				var publicFields		= ent.publicFieldsComp.Values;

				foreach ( var entityIndexData in entIndexDatas )
				{
				foreach ( var field in publicFields )
				{
					if ( entityIndexData.GetMemberName(  ) != field.FieldName )
					{
						continue;
					}
					var entIndexInfo		= new EntityIndexInfo(  );
					field.EntityIndexInfo	= entIndexInfo;

					entIndexInfo.EntityIndexData	= entityIndexData;
					entIndexInfo.Type				= entityIndexData.GetEntityIndexType(  );
					entIndexInfo.IsCustom			= entityIndexData.IsCustom(  );
					entIndexInfo.CustomMethods		= entIndexInfo.IsCustom ? entityIndexData.GetCustomMethods(  ) : null;
					entIndexInfo.Name				= entityIndexData.GetEntityIndexName(  );
					entIndexInfo.ContextNames		= entityIndexData.GetContextNames(  );
					entIndexInfo.ComponentType		= entityIndexData.GetComponentType(  );
					entIndexInfo.MemberType			= entityIndexData.GetKeyType(  );
					entIndexInfo.MemberName			= entityIndexData.GetMemberName(  );
					entIndexInfo.HasMultple			= entityIndexData.GetHasMultiple(  );
				}
				}

				ent.ReplacePublicFieldsComp( publicFields );
			}

			var customEntityIndexDatas = GetCustomData( types );
			foreach ( var data in customEntityIndexDatas )
			{
				var ent			= _contexts.main.CreateEntity(  );
				ent.AddCustomEntityIndexComp( data );
			}
		}

		private				EntityIndexData[]		GetCustomData			( List<Type> types )
		{
			var customEntityIndexData = types
				.Where(type => !type.IsAbstract)
				.Where(type => Attribute.IsDefined(type, typeof(CustomEntityIndexAttribute)))
				.Select(createCustomEntityIndexData);
			return customEntityIndexData.ToArray(  );
		}

		private				EntityIndexData[]		GetData					( List<Type> types )
		{
			var entityIndexData = types
				.Where(type => !type.IsAbstract)
				.Where(type => type.ImplementsInterface<IComponent>())
				.ToDictionary(
					type => type,
					type => type.GetPublicMemberInfos())
				.Where(kv => kv.Value.Any(info => info.attributes.Any(attr => attr.attribute is AbstractEntityIndexAttribute)))
				.SelectMany(kv => createEntityIndexData(kv.Key, kv.Value));

			return entityIndexData.ToArray();
		}

		private				EntityIndexData[]		createEntityIndexData	(Type type, List<PublicMemberInfo> infos)
		{
			var hasMultiple = infos.Count(i => i.attributes.Count(attr => attr.attribute is AbstractEntityIndexAttribute) == 1) > 1;
			return infos
				.Where(i => i.attributes.Count(attr => attr.attribute is AbstractEntityIndexAttribute) == 1)
				.Select(info => {
					var data = new EntityIndexData();
					var attribute = (AbstractEntityIndexAttribute)info.attributes.Single(attr => attr.attribute is AbstractEntityIndexAttribute).attribute;

					data.SetEntityIndexType(getEntityIndexType(attribute));
					data.IsCustom(false);
					data.SetEntityIndexName(type.ToCompilableString().ToComponentName(_contexts.settings.isIgnoreNamespaces));
					data.SetKeyType(info.type.ToCompilableString());
					data.SetComponentType(type.ToCompilableString());
					data.SetMemberName(info.name);
					data.SetHasMultiple(hasMultiple);
					data.SetContextNames(ReflectionToCompsSystem.TypeToContextNames[type].ToArray(  ));

					return data;
				}).ToArray();
		}

		private				EntityIndexData			createCustomEntityIndexData(Type type)
		{
			var data = new EntityIndexData();

			var attribute = (CustomEntityIndexAttribute)type.GetCustomAttributes(typeof(CustomEntityIndexAttribute), false)[0];

			data.SetEntityIndexType(type.ToCompilableString());
			data.IsCustom(true);
			data.SetEntityIndexName(type.ToCompilableString().RemoveDots());
			data.SetHasMultiple(false);
			data.SetContextNames(new[] { attribute.contextType.ToCompilableString().ShortTypeName().RemoveContextSuffix() });

			var getMethods = type
				.GetMethods(BindingFlags.Public | BindingFlags.Instance)
				.Where(method => Attribute.IsDefined(method, typeof(EntityIndexGetMethodAttribute)))
				.Select(method => new MethodData(
					method.ReturnType.ToCompilableString(),
					method.Name,
					method.GetParameters()
						.Select(p => new MemberData(p.ParameterType.ToCompilableString(), p.Name))
						.ToArray()
				)).ToArray();

			data.SetCustomMethods( getMethods );

			return data;
		}

		private				String					getEntityIndexType		( AbstractEntityIndexAttribute attribute )
		{
			switch (attribute.entityIndexType)
			{
				case EntityIndexType.EntityIndex:
					return "Entitas.EntityIndex";
				case EntityIndexType.PrimaryEntityIndex:
					return "Entitas.PrimaryEntityIndex";
				default:
					throw new Exception("Unhandled EntityIndexType: " + attribute.entityIndexType);
			}
		}
	}
}