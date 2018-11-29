using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Entitas;
using Ent = GenEntitas.MainEntity;

namespace GenEntitas
{
	// Replaces PostProcCleanTargetDirSystem, PostProcWriteToDiskSystem
	[Export(typeof(ISystem))]
	[Guid("3EB8A0DE-D615-4263-AE20-5FD966814030")]
	public class PostProcApplyDiffToDiskSystem : ReactiveSystem<Ent>
	{
		public				PostProcApplyDiffToDiskSystem					( Contexts contexts ) : base( contexts.main )
		{
			_contexts			= contexts;
		}

		public				PostProcApplyDiffToDiskSystem					(  ) : this( Contexts.sharedInstance )
		{
		}

		private				Contexts				_contexts;
		private				String					_generatePath;
		private				Boolean					_isDryRun;

		protected override	ICollector<Ent>			GetTrigger				( IContext<Ent> context )
		{
			return context.CreateCollector( MainMatcher.AllOf( MainMatcher.GeneratedFileComp ).NoneOf( MainMatcher.Destroy ) );
		}

		protected override	Boolean					Filter					( Ent entity )
		{
			return entity.hasGeneratedFileComp && !entity.isDestroy;
		}

		protected override	void					Execute					( List<Ent> ents )
		{
			_generatePath			= Path.Combine( _contexts.settings.generatePath.Value, "Generated" );
			_isDryRun				= _contexts.settings.isRunInDryMode;
			var stringBuilder		= new StringBuilder(  );

			DeleteNonGenFiles( ents, stringBuilder );

			foreach ( var ent in ents )
			{
				WriteFile( ent, stringBuilder );
			}

			if ( _contexts.settings.isLogGeneratedPaths )
			{
				var s				= stringBuilder.ToString(  );
				if ( String.IsNullOrEmpty( s ) )
				{
					Log( "No changes found since previous run\n" );
				}
				else
				{
					Log( s );
				}
			}
		}

		private				void					Log						( String s )
		{
#if UNITY_EDITOR
			UnityEngine.Debug.Log( s );
#else
			Console.Write( s );
#endif
		}

		private				void					CreateDirIfNeeded		( String dirPath )
		{
			if ( !_isDryRun
				&& !String.IsNullOrEmpty( dirPath )
				&& !Directory.Exists( dirPath ) )
			{
				Directory.CreateDirectory( dirPath );
			}
		}

		private				void					Delete					( String path )
		{
			if ( _isDryRun )
			{
				return;
			}
			File.Delete( path );
		}

		private				void					Write					( String path, String contents )
		{
			if ( _isDryRun )
			{
				return;
			}
			File.WriteAllText( path, contents );
		}

		private				void					DeleteNonGenFiles		( List<Ent> ents, StringBuilder sb )
		{
			var dirInfo					= new DirectoryInfo( _generatePath );
			CreateDirIfNeeded( _generatePath );

			var curFiles				= dirInfo.Exists
				? dirInfo.GetFiles( "*.cs", SearchOption.AllDirectories ).ToList(  )
				: new List<FileInfo>(  );

			var entGenPaths				= ents.Select( ent=> Path.Combine( dirInfo.FullName, ent.generatedFileComp.FilePath ) ).ToList(  );

			for ( var i = 0; i < curFiles.Count; i++ )
			{
				var f					= curFiles[i];
				var path				= f.FullName;
				if ( entGenPaths.Contains( path ) )
				{
					continue;
				}
				Delete( path );

				if ( _contexts.settings.isLogGeneratedPaths )
				{
					sb.Append( " - " );
					sb.Append( path );
					sb.Append( "\n" );
				}
			}
		}

		private				void					WriteFile				( Ent ent, StringBuilder sb )
		{
			var targetPath				= Path.Combine( _generatePath, ent.generatedFileComp.FilePath );
			var dirPath					= Path.GetDirectoryName( targetPath );
			CreateDirIfNeeded( dirPath );

			var contents				= ent.generatedFileComp.Contents;
			var writeState				= WriteFileState.Undefined;
			if ( !File.Exists( targetPath ) )
			{
				writeState				= WriteFileState.Create;
				Write( targetPath, contents );
			}
			else if ( String.Compare( File.ReadAllText( targetPath ), contents, StringComparison.Ordinal ) != 0 )
			{
				writeState				= WriteFileState.Change;
				Write( targetPath, contents );
			}
			else
			{
				writeState				= WriteFileState.Keep;
			}

			if ( _contexts.settings.isLogGeneratedPaths
				&& writeState != WriteFileState.Keep )
			{
				switch ( writeState )
				{
					case WriteFileState.Create:
							sb.Append( " + " );
							break;
					case WriteFileState.Change:
							sb.Append( " * " );
							break;
					case WriteFileState.Keep:
							sb.Append( "   " );
							break;
					default:
						throw new NotImplementedException(  );
				}
				sb.Append( targetPath );
				sb.Append( " - " );
				sb.Append( ent.generatedFileComp.GeneratedBy );
				sb.Append( "\n" );
			}
		}

		private enum WriteFileState
		{
			Undefined,
			Create,
			Change,
			Keep,
		}
	}
}
