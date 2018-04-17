using System;
using System.Collections.Generic;
using Entitas;
using Ent = MainEntity;

namespace GenEntitas.Sources
{
	public class GenFeatureClassSystem : ReactiveSystem<Ent>
	{
		public				GenFeatureClassSystem	( Contexts contexts ) : base( contexts.main )
		{
			_contexts			= contexts;
		}

		private				Contexts				_contexts;
        private const		String					FEATURE_TEMPLATE		=
@"#if (!ENTITAS_DISABLE_VISUAL_DEBUGGING && UNITY_EDITOR)

public class Feature : Entitas.VisualDebugging.Unity.DebugSystems {

    public Feature(string name) : base(name) {
    }

    public Feature() : base(true) {
        var typeName = DesperateDevs.Utils.SerializationTypeExtension.ToCompilableString(GetType());
        var shortType = DesperateDevs.Utils.SerializationTypeExtension.ShortTypeName(typeName);
        var readableType = DesperateDevs.Utils.StringExtension.ToSpacedCamelCase(shortType);

        initialize(readableType);
    }
}

#elif (!ENTITAS_DISABLE_DEEP_PROFILING && DEVELOPMENT_BUILD)

public class Feature : Entitas.Systems {

    System.Collections.Generic.List<string> _initializeSystemNames;
    System.Collections.Generic.List<string> _executeSystemNames;
    System.Collections.Generic.List<string> _cleanupSystemNames;
    System.Collections.Generic.List<string> _tearDownSystemNames;

    public Feature(string name) : this() {
    }

    public Feature() {
        _initializeSystemNames = new System.Collections.Generic.List<string>();
        _executeSystemNames = new System.Collections.Generic.List<string>();
        _cleanupSystemNames = new System.Collections.Generic.List<string>();
        _tearDownSystemNames = new System.Collections.Generic.List<string>();
    }

    public override Entitas.Systems Add(Entitas.ISystem system) {
        var systemName = system.GetType().FullName;

        if (system is Entitas.IInitializeSystem) {
            _initializeSystemNames.Add(systemName);
        }

        if (system is Entitas.IExecuteSystem) {
            _executeSystemNames.Add(systemName);
        }

        if (system is Entitas.ICleanupSystem) {
            _cleanupSystemNames.Add(systemName);
        }

        if (system is Entitas.ITearDownSystem) {
            _tearDownSystemNames.Add(systemName);
        }

        return base.Add(system);
    }

    public override void Initialize() {
        for (int i = 0; i < _initializeSystems.Count; i++) {
            UnityEngine.Profiling.Profiler.BeginSample(_initializeSystemNames[i]);
            _initializeSystems[i].Initialize();
            UnityEngine.Profiling.Profiler.EndSample();
        }
    }

    public override void Execute() {
        for (int i = 0; i < _executeSystems.Count; i++) {
            UnityEngine.Profiling.Profiler.BeginSample(_executeSystemNames[i]);
            _executeSystems[i].Execute();
            UnityEngine.Profiling.Profiler.EndSample();
        }
    }

    public override void Cleanup() {
        for (int i = 0; i < _cleanupSystems.Count; i++) {
            UnityEngine.Profiling.Profiler.BeginSample(_cleanupSystemNames[i]);
            _cleanupSystems[i].Cleanup();
            UnityEngine.Profiling.Profiler.EndSample();
        }
    }

    public override void TearDown() {
        for (int i = 0; i < _tearDownSystems.Count; i++) {
            UnityEngine.Profiling.Profiler.BeginSample(_tearDownSystemNames[i]);
            _tearDownSystems[i].TearDown();
            UnityEngine.Profiling.Profiler.EndSample();
        }
    }
}

#else

public class Feature : Entitas.Systems {

    public Feature(string name) {
    }

    public Feature() {
    }
}

#endif
";

		// FIXME: wrong trigger
		protected override	ICollector<Ent>			GetTrigger				( IContext<Ent> context )
		{
			return context.CreateCollector( MainMatcher.ContextComp );
		}

		protected override	Boolean					Filter					( Ent entity )
		{
			return entity.hasContextComp;
		}

		// FIXME: enable/disable in settings
		protected override	void					Execute					( List<Ent> entities )
		{
			var ent				= _contexts.main.CreateEntity(  );
			ent.AddGeneratedFileComp( "Feature.cs", FEATURE_TEMPLATE, GetType(  ).FullName );
		}
	}
}
