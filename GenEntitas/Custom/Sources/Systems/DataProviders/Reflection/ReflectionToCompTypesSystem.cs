using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DesperateDevs.Utils;
using Entitas;
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
			var types				= new List<Type>();

			foreach ( var path in assemblyPaths )
			{
				var assembly		= Assembly.LoadFrom( path );
				var assemblyTypes	= assembly
					.GetTypes(  )
					.Where(type => !type.IsAbstract)
					.Where(type => type.ImplementsInterface<IComponent>(  ) )
					.ToArray(  );
				types.AddRange( assemblyTypes );
			}

			_contexts.main.SetReflectionComponentTypes( types );
		}
	}
}
