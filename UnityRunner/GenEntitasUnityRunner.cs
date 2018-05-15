using System;
using System.IO;
using System.Linq;
using GenEntitas;
using UnityEditor;
using UnityEngine;

	public class Options
	{
		public String DllPaths { get; set; }
		public String GeneratePath { get; set; }
		public Boolean RunInDryMode { get; set; }
	}

public class GenEntitasUnityRunner : MonoBehaviour
{

	[UnityEditor.MenuItem( "Tools/GenEntitas/Generate #&g" )]
	private static void Run(  )
	{
		var options = new Options()
		{
			DllPaths = "Library/ScriptAssemblies/Assembly-CSharp.dll",
			GeneratePath = "Assets/Sources/",
			RunInDryMode = false,
		};
		if ( !Directory.Exists( options.GeneratePath ) )
		{
			Debug.Log( $"Generate path does not exist: '{options.GeneratePath}'" );
			return;
		}

		var runner = new Runner();
		runner.Init(  );

		var contexts = runner.Contexts;
		FillContexts( contexts, options );

		runner.Systems.Initialize(  );
		runner.Systems.Execute(  );
		runner.Systems.Cleanup(  );
		runner.Systems.TearDown(  );

		AssetDatabase.Refresh(  );
		Debug.Log( "Done!" );
	}

	private static void FillContexts( Contexts contexts, Options options )
	{
		contexts.settings.isConsoleWriteLineGeneratedPaths		= true;
		contexts.settings.isRunInDryMode						= options.RunInDryMode;
		contexts.settings.SetGeneratePath( String.IsNullOrEmpty( options.GeneratePath ) ? "./" : options.GeneratePath );
		contexts.settings.SetReflectionAssemblyPaths( options.DllPaths.Split(',').ToList(  ) );
	}
}
