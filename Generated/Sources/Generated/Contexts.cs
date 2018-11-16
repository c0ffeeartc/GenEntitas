namespace GenEntitas {
public partial class Contexts : Entitas.IContexts {

    public static Contexts sharedInstance {
        get {
            if (_sharedInstance == null) {
                _sharedInstance = new Contexts();
            }

            return _sharedInstance;
        }
        set { _sharedInstance = value; }
    }

    static Contexts _sharedInstance;

    public MainContext main { get; set; }
    public SettingsContext settings { get; set; }

    public Entitas.IContext[] allContexts { get { return new Entitas.IContext [] { main, settings }; } }

    public Contexts() {
        main = new MainContext();
        settings = new SettingsContext();

        var postConstructors = System.Linq.Enumerable.Where(
            GetType().GetMethods(),
            method => System.Attribute.IsDefined(method, typeof(Entitas.CodeGeneration.Attributes.PostConstructorAttribute))
        );

        foreach (var postConstructor in postConstructors) {
            postConstructor.Invoke(this, null);
        }
    }

    public void Reset() {
        var contexts = allContexts;
        for (int i = 0; i < contexts.Length; i++) {
            contexts[i].Reset();
        }
    }
}

}

namespace GenEntitas {
public partial class Contexts {

#if (!ENTITAS_DISABLE_VISUAL_DEBUGGING && UNITY_EDITOR)

    [Entitas.CodeGeneration.Attributes.PostConstructor]
    public void InitializeContexObservers() {
        try {
            CreateContextObserver(main);
            CreateContextObserver(settings);
        } catch(System.Exception) {
        }
    }

    public void CreateContextObserver(Entitas.IContext context) {
        if (UnityEngine.Application.isPlaying) {
            var observer = new Entitas.VisualDebugging.Unity.ContextObserver(context);
            UnityEngine.Object.DontDestroyOnLoad(observer.gameObject);
        }
    }

#endif
}

}

namespace GenEntitas {
public partial class Contexts {

    public const string Comp = "Comp";
    public const string INamedTypeSymbol = "INamedTypeSymbol";
    public const string TypeComp = "TypeComp";

    [Entitas.CodeGeneration.Attributes.PostConstructor]
    public void InitializeEntityIndices() {
        main.AddEntityIndex(new Entitas.PrimaryEntityIndex<MainEntity, string>(
            Comp,
            main.GetGroup(MainMatcher.Comp),
            (e, c) => ((GenEntitas.Comp)c).FullTypeName));

        main.AddEntityIndex(new Entitas.PrimaryEntityIndex<MainEntity, Microsoft.CodeAnalysis.INamedTypeSymbol>(
            INamedTypeSymbol,
            main.GetGroup(MainMatcher.INamedTypeSymbol),
            (e, c) => ((GenEntitas.INamedTypeSymbolComponent)c).Value));

        main.AddEntityIndex(new Entitas.EntityIndex<MainEntity, System.Type>(
            TypeComp,
            main.GetGroup(MainMatcher.TypeComp),
            (e, c) => ((GenEntitas.TypeComp)c).Value));
    }
}

public static class ContextsExtensions {

    public static MainEntity GetEntityWithComp(this MainContext context, string FullTypeName) {
        return ((Entitas.PrimaryEntityIndex<MainEntity, string>)context.GetEntityIndex(Contexts.Comp)).GetEntity(FullTypeName);
    }

    public static MainEntity GetEntityWithINamedTypeSymbol(this MainContext context, Microsoft.CodeAnalysis.INamedTypeSymbol Value) {
        return ((Entitas.PrimaryEntityIndex<MainEntity, Microsoft.CodeAnalysis.INamedTypeSymbol>)context.GetEntityIndex(Contexts.INamedTypeSymbol)).GetEntity(Value);
    }

    public static System.Collections.Generic.HashSet<MainEntity> GetEntitiesWithTypeComp(this MainContext context, System.Type Value) {
        return ((Entitas.EntityIndex<MainEntity, System.Type>)context.GetEntityIndex(Contexts.TypeComp)).GetEntities(Value);
    }
}
}
