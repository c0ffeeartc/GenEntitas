﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using DesperateDevs.Utils;
using Entitas;
using Entitas.CodeGeneration.Attributes;
using Ent = SettingsEntity;

namespace GenEntitas
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
			var compTypes			= new HashSet<Type>();
			var loadableTypes		= new HashSet<Type>();

			foreach ( var path in assemblyPaths )
			{
				var fileInfo		= new FileInfo( path );
				if ( !fileInfo.Exists )
				{
					Console.WriteLine( "No such file: " + fileInfo.FullName );
					continue;
				}
				var assembly		= Assembly.LoadFrom( path );
				var types			= GetLoadableTypes( assembly );
				loadableTypes.UnionWith( types );

				var dataFromComponents = types
				.Where(type => type.ImplementsInterface<IComponent>())
				.Where(type => !type.IsAbstract)
				.ToArray();

				compTypes.UnionWith( dataFromComponents );

				var dataFromNonComponents = types
				.Where(type => !type.ImplementsInterface<IComponent>())
				.Where(type => !type.IsGenericType)
				.Where(type => GetContextNames(type).Length > 0)
				.ToArray();

				compTypes.UnionWith( dataFromNonComponents );

			}

			_contexts.main.SetReflectionComponentTypes( compTypes.ToList(  ) );
			_contexts.main.SetReflectionLoadableTypes( loadableTypes.ToList(  ) );
		}

		private				String[]				GetContextNames			(Type type)
		{
			return Attribute
				.GetCustomAttributes(type)
				.OfType<ContextAttribute>()
				.Select(attr => attr.contextName)
				.ToArray();
		}

		public IEnumerable<Type> GetLoadableTypes( Assembly assembly )
		{
			if (assembly == null) throw new ArgumentNullException(nameof(assembly));
			try
			{
				return assembly.GetTypes();
			}
			catch (ReflectionTypeLoadException e)
			{
				return e.Types.Where( t => t != null ).ToList();
			}
		}
	}
}
