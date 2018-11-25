using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Entitas;
using Ent = GenEntitas.SettingsEntity;

namespace GenEntitas
{
	public class PostProcWriteGenPathsToCsprojSystem : ReactiveSystem<SettingsEntity>
	{
		public				PostProcWriteGenPathsToCsprojSystem ( Contexts contexts ) : base( contexts.settings )
		{
			_contexts			= contexts;
			_generatedGroup		= contexts.main.GetGroup( MainMatcher.AllOf( MainMatcher.GeneratedFileComp ).NoneOf( MainMatcher.Destroy ) );
		}

		private				Contexts				_contexts;
		private				IGroup<MainEntity>		_generatedGroup;
		private				String					_generatePath;

		protected override	ICollector<Ent>			GetTrigger				( IContext<Ent> context )
		{
			return context.CreateCollector( SettingsMatcher.WriteGeneratedPathsToCsProj );
		}

		protected override	Boolean					Filter					( Ent ent )
		{
			return ent.hasWriteGeneratedPathsToCsProj;
		}

		protected override	void					Execute					( List<Ent> entities )
		{
			var path			= entities[0].writeGeneratedPathsToCsProj.Value;
			if ( String.IsNullOrEmpty( path ) )
			{
				return;
			}

			if ( !File.Exists( path ) )
			{
				throw new FileNotFoundException( path );
			}

			_generatePath		= Path.Combine( _contexts.settings.generatePath.Value, "Generated" );

			var contents		= File.ReadAllText( path );
			contents			= RemoveExistingGeneratedEntires( contents );
			contents			= AddGeneratedEntires( contents );

			File.WriteAllText( path, contents );
		}

		private				String					RemoveExistingGeneratedEntires	( String contents )
		{
			var pattern			= "\\s*<Compile Include=\"" + _generatePath.Replace("/", "\\").Replace("\\", "\\\\") + ".* \\/>";
			contents			= Regex.Replace(contents, pattern, string.Empty);
			return Regex.Replace(contents, "\\s*<ItemGroup>\\s*<\\/ItemGroup>", "" );
		}

		private				String					AddGeneratedEntires		( String contents )
		{
			var ents			= _generatedGroup.GetEntities(  );
			if ( ents.Length == 0 )
			{
				return contents;
			}

			var entryTemplate	= "    <Compile Include=\"" + _generatePath.Replace("/", "\\") + "\\{0}\" />";

			var entries			= new List<String>(  );
			foreach ( var ent in ents )
			{
				var entry = String.Format( entryTemplate, ent.generatedFileComp.FilePath.Replace( "/", "\\" ) );
				entries.Add( entry );
			}

			var entriesItemGroup = String.Format("</ItemGroup>\n  <ItemGroup>\n{0}\n  </ItemGroup>", String.Join( "\r\n", entries ) );
			//Console.WriteLine( entriesItemGroup );
			return new Regex("<\\/ItemGroup>").Replace( contents, entriesItemGroup, 1 );
		}
	}
}