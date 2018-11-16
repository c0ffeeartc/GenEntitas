namespace GenEntitas {
public partial class MainContext {

    public MainEntity roslynComponentTypesEntity { get { return GetGroup(MainMatcher.RoslynComponentTypes).GetSingleEntity(); } }
    public GenEntitas.RoslynComponentTypes roslynComponentTypes { get { return roslynComponentTypesEntity.roslynComponentTypes; } }
    public bool hasRoslynComponentTypes { get { return roslynComponentTypesEntity != null; } }

    public MainEntity SetRoslynComponentTypes(System.Collections.Generic.List<Microsoft.CodeAnalysis.INamedTypeSymbol> newValues) {
        if (hasRoslynComponentTypes) {
            throw new Entitas.EntitasException("Could not set RoslynComponentTypes!\n" + this + " already has an entity with GenEntitas.RoslynComponentTypes!",
                "You should check if the context already has a roslynComponentTypesEntity before setting it or use context.ReplaceRoslynComponentTypes().");
        }
        var entity = CreateEntity();
        entity.AddRoslynComponentTypes(newValues);
        return entity;
    }

    public void ReplaceRoslynComponentTypes(System.Collections.Generic.List<Microsoft.CodeAnalysis.INamedTypeSymbol> newValues) {
        var entity = roslynComponentTypesEntity;
        if (entity == null) {
            entity = SetRoslynComponentTypes(newValues);
        } else {
            entity.ReplaceRoslynComponentTypes(newValues);
        }
    }

    public void RemoveRoslynComponentTypes() {
        roslynComponentTypesEntity.Destroy();
    }
}

}

namespace GenEntitas {
public partial class MainEntity {

    public GenEntitas.RoslynComponentTypes roslynComponentTypes { get { return (GenEntitas.RoslynComponentTypes)GetComponent(MainComponentsLookup.RoslynComponentTypes); } }
    public bool hasRoslynComponentTypes { get { return HasComponent(MainComponentsLookup.RoslynComponentTypes); } }

    public void AddRoslynComponentTypes(System.Collections.Generic.List<Microsoft.CodeAnalysis.INamedTypeSymbol> newValues) {
        var index = MainComponentsLookup.RoslynComponentTypes;
        var component = (GenEntitas.RoslynComponentTypes)CreateComponent(index, typeof(GenEntitas.RoslynComponentTypes));
        component.Values = newValues;
        AddComponent(index, component);
    }

    public void ReplaceRoslynComponentTypes(System.Collections.Generic.List<Microsoft.CodeAnalysis.INamedTypeSymbol> newValues) {
        var index = MainComponentsLookup.RoslynComponentTypes;
        var component = (GenEntitas.RoslynComponentTypes)CreateComponent(index, typeof(GenEntitas.RoslynComponentTypes));
        component.Values = newValues;
        ReplaceComponent(index, component);
    }

    public void RemoveRoslynComponentTypes() {
        RemoveComponent(MainComponentsLookup.RoslynComponentTypes);
    }
}

}

namespace GenEntitas {
public sealed partial class MainMatcher {

    static Entitas.IMatcher<MainEntity> _matcherRoslynComponentTypes;

    public static Entitas.IMatcher<MainEntity> RoslynComponentTypes {
        get {
            if (_matcherRoslynComponentTypes == null) {
                var matcher = (Entitas.Matcher<MainEntity>)Entitas.Matcher<MainEntity>.AllOf(MainComponentsLookup.RoslynComponentTypes);
                matcher.componentNames = MainComponentsLookup.componentNames;
                _matcherRoslynComponentTypes = matcher;
            }

            return _matcherRoslynComponentTypes;
        }
    }
}

}
