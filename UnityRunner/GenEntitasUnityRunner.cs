using UnityEditor;
using UnityEngine;

namespace GenEntitas
{
public class GenEntitasUnityRunner : MonoBehaviour
{

	[UnityEditor.MenuItem( "Tools/GenEntitas/Generate #&g" )]
	private static void Run(  )
	{
		var runner = new Runner();
		runner.Init(  );

		var contexts = runner.Contexts;
		FillContexts( contexts );

		runner.Systems.Initialize(  );
		runner.Systems.Execute(  );
		runner.Systems.Cleanup(  );
		runner.Systems.TearDown(  );

		AssetDatabase.Refresh(  );
		Debug.Log( "Done!" );
	}

	private static void FillContexts( Contexts contexts )
	{
		contexts.settings.isConsoleWriteLineGeneratedPaths		= true;
		contexts.settings.ReplaceSettingsPath( "./GenEntitasUnity.settings" );
	}
}
}
