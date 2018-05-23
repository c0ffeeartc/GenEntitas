﻿using System;
using System.Collections.Generic;
using System.IO;
using Entitas;
using Ent = GenEntitas.SettingsEntity;

namespace GenEntitas
{
	public class ReadGenEntitasLangFilesSystem : ReactiveSystem<Ent>
	{
		public				ReadGenEntitasLangFilesSystem	( Contexts contexts ) : base( contexts.settings )
		{
			_contexts			= contexts;
		}

		private				Contexts				_contexts;

		protected override	ICollector<Ent>			GetTrigger				( IContext<Ent> context )
		{
			return context.CreateCollector( SettingsMatcher.SettingsPath );
		}

		protected override	Boolean					Filter					( Ent entity )
		{
			return entity.hasSettingsPath;
		}

		protected override	void					Execute					( List<Ent> entities )
		{
			var paths			= _contexts.settings.genEntitasLangPaths.Values;
			foreach ( var path in paths )
			{
				if ( String.IsNullOrEmpty( path ) )
				{
					continue;
				}

				if ( !File.Exists( path ) )
				{
					throw new FileNotFoundException( $"GenEntitasLang file does not exist: '{path}'" );
				}

				var str		= File.ReadAllText( path );

				var ent		= _contexts.main.CreateEntity(  );
				ent.AddGenEntitasLangInputString( str );
			}
		}
	}
}