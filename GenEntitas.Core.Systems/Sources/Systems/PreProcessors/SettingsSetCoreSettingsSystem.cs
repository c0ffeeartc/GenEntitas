using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Entitas;
using Ent = GenEntitas.SettingsEntity;

namespace GenEntitas
{
	[Export(typeof(ISystem))]
	[Guid("6F2E73C6-B6D3-42C6-AA29-7A832DA32F3E")]
	public class SettingsSetCoreSettingsSystem : ReactiveSystem<Ent>
	{
		public				SettingsSetCoreSettingsSystem	( Contexts contexts ) : base( contexts.settings )
		{
			_contexts			= contexts;
		}

		public				SettingsSetCoreSettingsSystem	(  ) : this( Contexts.sharedInstance )
		{
		}

		private				Contexts				_contexts;

		protected override	ICollector<Ent>			GetTrigger				( IContext<Ent> context )
		{
			return context.CreateCollector( SettingsMatcher.SettingsDict );
		}

		protected override	Boolean					Filter					( Ent entity )
		{
			return entity.hasSettingsDict;
		}

		protected override	void					Execute					( List<Ent> entities )
		{
			var settings			= _contexts.Services.Settings;
			var d					= _contexts.settings.settingsDict.Dict;
			if ( d.ContainsKey( nameof( LogGeneratedPaths ) ) )
			{
				_contexts.settings.isLogGeneratedPaths = settings.BoolFromStr( d[nameof( LogGeneratedPaths )].FirstOrDefault(  ) );
			}
			else
			{
				_contexts.settings.isLogGeneratedPaths = true;
			}

			if ( d.ContainsKey( nameof( IgnoreNamespaces ) ) )
			{
				_contexts.settings.isIgnoreNamespaces = settings.BoolFromStr( d[nameof( IgnoreNamespaces )].FirstOrDefault(  ) );
			}
			else
			{
				_contexts.settings.isIgnoreNamespaces = false;
			}

			if ( d.ContainsKey( nameof( RunInDryMode ) ) )
			{
				_contexts.settings.isRunInDryMode = settings.BoolFromStr( d[nameof( RunInDryMode )].FirstOrDefault(  ) );
			}
			else
			{
				_contexts.settings.isRunInDryMode = false;
			}

			_contexts.settings.ReplaceGeneratedNamespace( d.ContainsKey( nameof( GeneratedNamespace ) )
				? d[nameof( GeneratedNamespace )].FirstOrDefault(  )
				: "" );

			_contexts.settings.ReplaceGeneratePath( d.ContainsKey( nameof( GeneratePath ) )
				? d[nameof( GeneratePath )].FirstOrDefault(  )
				: "" );

			_contexts.settings.ReplaceWriteGeneratedPathsToCsProj( d.ContainsKey( nameof( WriteGeneratedPathsToCsProj ) )
				? d[nameof( WriteGeneratedPathsToCsProj )].FirstOrDefault(  )
				: "" );

			if ( !Directory.Exists( _contexts.settings.generatePath.Value ) )
			{
				throw new DirectoryNotFoundException( $"Generate path does not exist: '{_contexts.settings.generatePath.Value}'" );
			}
		}
	}
}