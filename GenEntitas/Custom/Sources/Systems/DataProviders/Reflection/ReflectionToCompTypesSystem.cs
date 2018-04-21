using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DesperateDevs.Utils;
using Entitas;
using Entitas.CodeGeneration.Attributes;
using Ent = SettingsEntity;

namespace GenEntitas.Sources
{
	public class ReflectionToCompTypesSystem : ReactiveSystem<SettingsEntity>
	{
		public				ReflectionToCompTypesSystem	( Contexts contexts ) : base( contexts.settings )
		{
			_contexts			= contexts;
		}

		private				Contexts				_contexts;

		protected override	ICollector<Ent>			GetTrigger				( IContext<Ent> context )
		{
			return context.CreateCollector( SettingsMatcher.ReflectionAssemblyPaths );
		}

		protected override	Boolean					Filter					( Ent entity )
		{
			return entity.hasReflectionAssemblyPaths;
		}

		protected override	void					Execute					( List<Ent> entities )
		{
			var assemblyPaths		= entities[0].reflectionAssemblyPaths.Values;
			var typeSet				= new HashSet<Type>();

			foreach ( var path in assemblyPaths )
			{
				var assembly		= Assembly.LoadFrom( path );
				var types			= assembly.GetTypes();

			var dataFromComponents = types
				.Where(type => type.ImplementsInterface<IComponent>())
				.Where(type => !type.IsAbstract)
				.ToArray();

				typeSet.UnionWith( dataFromComponents );

			var dataFromNonComponents = types
				.Where(type => !type.ImplementsInterface<IComponent>())
				.Where(type => !type.IsGenericType)
				.Where(type => GetContextNames(type).Length > 0)
				.ToArray();

				typeSet.UnionWith( dataFromNonComponents );

			}

			_contexts.main.SetReflectionComponentTypes( typeSet.ToList(  ) );
		}

		private				String[]				GetContextNames			(Type type)
		{
			return Attribute
				.GetCustomAttributes(type)
				.OfType<ContextAttribute>()
				.Select(attr => attr.contextName)
				.ToArray();
		}
	}
}
