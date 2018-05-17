public partial class MainContext {

    public MainEntity reflectionLoadableTypesEntity { get { return GetGroup(MainMatcher.ReflectionLoadableTypes).GetSingleEntity(); } }
    public GenEntitas.ReflectionLoadableTypes reflectionLoadableTypes { get { return reflectionLoadableTypesEntity.reflectionLoadableTypes; } }
    public bool hasReflectionLoadableTypes { get { return reflectionLoadableTypesEntity != null; } }

    public MainEntity SetReflectionLoadableTypes(System.Collections.Generic.List<System.Type> newValues) {
        if (hasReflectionLoadableTypes) {
            throw new Entitas.EntitasException("Could not set ReflectionLoadableTypes!\n" + this + " already has an entity with GenEntitas.ReflectionLoadableTypes!",
                "You should check if the context already has a reflectionLoadableTypesEntity before setting it or use context.ReplaceReflectionLoadableTypes().");
        }
        var entity = CreateEntity();
        entity.AddReflectionLoadableTypes(newValues);
        return entity;
    }

    public void ReplaceReflectionLoadableTypes(System.Collections.Generic.List<System.Type> newValues) {
        var entity = reflectionLoadableTypesEntity;
        if (entity == null) {
            entity = SetReflectionLoadableTypes(newValues);
        } else {
            entity.ReplaceReflectionLoadableTypes(newValues);
        }
    }

    public void RemoveReflectionLoadableTypes() {
        reflectionLoadableTypesEntity.Destroy();
    }
}

public partial class MainEntity {

    public GenEntitas.ReflectionLoadableTypes reflectionLoadableTypes { get { return (GenEntitas.ReflectionLoadableTypes)GetComponent(MainComponentsLookup.ReflectionLoadableTypes); } }
    public bool hasReflectionLoadableTypes { get { return HasComponent(MainComponentsLookup.ReflectionLoadableTypes); } }

    public void AddReflectionLoadableTypes(System.Collections.Generic.List<System.Type> newValues) {
        var index = MainComponentsLookup.ReflectionLoadableTypes;
        var component = CreateComponent<GenEntitas.ReflectionLoadableTypes>(index);
        component.Values = newValues;
        AddComponent(index, component);
    }

    public void ReplaceReflectionLoadableTypes(System.Collections.Generic.List<System.Type> newValues) {
        var index = MainComponentsLookup.ReflectionLoadableTypes;
        var component = CreateComponent<GenEntitas.ReflectionLoadableTypes>(index);
        component.Values = newValues;
        ReplaceComponent(index, component);
    }

    public void RemoveReflectionLoadableTypes() {
        RemoveComponent(MainComponentsLookup.ReflectionLoadableTypes);
    }
}

public sealed partial class MainMatcher {

    static Entitas.IMatcher<MainEntity> _matcherReflectionLoadableTypes;

    public static Entitas.IMatcher<MainEntity> ReflectionLoadableTypes {
        get {
            if (_matcherReflectionLoadableTypes == null) {
                var matcher = (Entitas.Matcher<MainEntity>)Entitas.Matcher<MainEntity>.AllOf(MainComponentsLookup.ReflectionLoadableTypes);
                matcher.componentNames = MainComponentsLookup.componentNames;
                _matcherReflectionLoadableTypes = matcher;
            }

            return _matcherReflectionLoadableTypes;
        }
    }
}
