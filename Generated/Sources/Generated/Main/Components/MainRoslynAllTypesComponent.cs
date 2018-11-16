namespace GenEntitas {
public partial class MainContext {

    public MainEntity roslynAllTypesEntity { get { return GetGroup(MainMatcher.RoslynAllTypes).GetSingleEntity(); } }
    public GenEntitas.RoslynAllTypes roslynAllTypes { get { return roslynAllTypesEntity.roslynAllTypes; } }
    public bool hasRoslynAllTypes { get { return roslynAllTypesEntity != null; } }

    public MainEntity SetRoslynAllTypes(System.Collections.Generic.List<Microsoft.CodeAnalysis.INamedTypeSymbol> newValues) {
        if (hasRoslynAllTypes) {
            throw new Entitas.EntitasException("Could not set RoslynAllTypes!\n" + this + " already has an entity with GenEntitas.RoslynAllTypes!",
                "You should check if the context already has a roslynAllTypesEntity before setting it or use context.ReplaceRoslynAllTypes().");
        }
        var entity = CreateEntity();
        entity.AddRoslynAllTypes(newValues);
        return entity;
    }

    public void ReplaceRoslynAllTypes(System.Collections.Generic.List<Microsoft.CodeAnalysis.INamedTypeSymbol> newValues) {
        var entity = roslynAllTypesEntity;
        if (entity == null) {
            entity = SetRoslynAllTypes(newValues);
        } else {
            entity.ReplaceRoslynAllTypes(newValues);
        }
    }

    public void RemoveRoslynAllTypes() {
        roslynAllTypesEntity.Destroy();
    }
}

}

namespace GenEntitas {
public partial class MainEntity {

    public GenEntitas.RoslynAllTypes roslynAllTypes { get { return (GenEntitas.RoslynAllTypes)GetComponent(MainComponentsLookup.RoslynAllTypes); } }
    public bool hasRoslynAllTypes { get { return HasComponent(MainComponentsLookup.RoslynAllTypes); } }

    public void AddRoslynAllTypes(System.Collections.Generic.List<Microsoft.CodeAnalysis.INamedTypeSymbol> newValues) {
        var index = MainComponentsLookup.RoslynAllTypes;
        var component = (GenEntitas.RoslynAllTypes)CreateComponent(index, typeof(GenEntitas.RoslynAllTypes));
        component.Values = newValues;
        AddComponent(index, component);
    }

    public void ReplaceRoslynAllTypes(System.Collections.Generic.List<Microsoft.CodeAnalysis.INamedTypeSymbol> newValues) {
        var index = MainComponentsLookup.RoslynAllTypes;
        var component = (GenEntitas.RoslynAllTypes)CreateComponent(index, typeof(GenEntitas.RoslynAllTypes));
        component.Values = newValues;
        ReplaceComponent(index, component);
    }

    public void RemoveRoslynAllTypes() {
        RemoveComponent(MainComponentsLookup.RoslynAllTypes);
    }
}

}

namespace GenEntitas {
public sealed partial class MainMatcher {

    static Entitas.IMatcher<MainEntity> _matcherRoslynAllTypes;

    public static Entitas.IMatcher<MainEntity> RoslynAllTypes {
        get {
            if (_matcherRoslynAllTypes == null) {
                var matcher = (Entitas.Matcher<MainEntity>)Entitas.Matcher<MainEntity>.AllOf(MainComponentsLookup.RoslynAllTypes);
                matcher.componentNames = MainComponentsLookup.componentNames;
                _matcherRoslynAllTypes = matcher;
            }

            return _matcherRoslynAllTypes;
        }
    }
}

}
