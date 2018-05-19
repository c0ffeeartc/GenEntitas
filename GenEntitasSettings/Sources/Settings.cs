using System;

namespace GenEntitas
{
	public partial class Settings
	{
		public					Settings				(  ) : this
			( ignoreNamespaces:		false
			, generatePath:				""
			, dllPaths:					""
			, generatedNamespace:		""
			, dryRun:				false )
		{
		}

		public					Settings				( bool ignoreNamespaces, string generatePath, string dllPaths, string generatedNamespace, bool dryRun )
		{
			IgnoreNamespaces			= ignoreNamespaces;
			GeneratePath				= generatePath;
			DllPaths					= dllPaths;
			GeneratedNamespace			= generatedNamespace;
			DryRun						= dryRun;
		}

		public					Boolean					IgnoreNamespaces;
		public					String					GeneratePath;
		public					String					DllPaths;
		public					String					GeneratedNamespace;
		public					Boolean					DryRun;
	}
}