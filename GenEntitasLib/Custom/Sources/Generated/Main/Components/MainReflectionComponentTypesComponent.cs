public partial class MainContext {

    public MainEntity reflectionComponentTypesEntity { get { return GetGroup(MainMatcher.ReflectionComponentTypes).GetSingleEntity(); } }
    public GenEntitas.ReflectionComponentTypes reflectionComponentTypes { get { return reflectionComponentTypesEntity.reflectionComponentTypes; } }
    public bool hasReflectionComponentTypes { get { return reflectionComponentTypesEntity != null; } }

    public MainEntity SetReflectionComponentTypes(System.Collections.Generic.List<System.Type> newValues) {
        if (hasReflectionComponentTypes) {
            throw new Entitas.EntitasException("Could not set ReflectionComponentTypes!\n" + this + " already has an entity with GenEntitas.ReflectionComponentTypes!",
                "You should check if the context already has a reflectionComponentTypesEntity before setting it or use context.ReplaceReflectionComponentTypes().");
        }
        var entity = CreateEntity();
        entity.AddReflectionComponentTypes(newValues);
        return entity;
    }

    public void ReplaceReflectionComponentTypes(System.Collections.Generic.List<System.Type> newValues) {
        var entity = reflectionComponentTypesEntity;
        if (entity == null) {
            entity = SetReflectionComponentTypes(newValues);
        } else {
            entity.ReplaceReflectionComponentTypes(newValues);
        }
    }

    public void RemoveReflectionComponentTypes() {
        reflectionComponentTypesEntity.Destroy();
    }
}

public partial class MainEntity {

    public GenEntitas.ReflectionComponentTypes reflectionComponentTypes { get { return (GenEntitas.ReflectionComponentTypes)GetComponent(MainComponentsLookup.ReflectionComponentTypes); } }
    public bool hasReflectionComponentTypes { get { return HasComponent(MainComponentsLookup.ReflectionComponentTypes); } }

    public void AddReflectionComponentTypes(System.Collections.Generic.List<System.Type> newValues) {
        var index = MainComponentsLookup.ReflectionComponentTypes;
        var component = CreateComponent<GenEntitas.ReflectionComponentTypes>(index);
        component.Values = newValues;
        AddComponent(index, component);
    }

    public void ReplaceReflectionComponentTypes(System.Collections.Generic.List<System.Type> newValues) {
        var index = MainComponentsLookup.ReflectionComponentTypes;
        var component = CreateComponent<GenEntitas.ReflectionComponentTypes>(index);
        component.Values = newValues;
        ReplaceComponent(index, component);
    }

    public void RemoveReflectionComponentTypes() {
        RemoveComponent(MainComponentsLookup.ReflectionComponentTypes);
    }
}

public sealed partial class MainMatcher {

    static Entitas.IMatcher<MainEntity> _matcherReflectionComponentTypes;

    public static Entitas.IMatcher<MainEntity> ReflectionComponentTypes {
        get {
            if (_matcherReflectionComponentTypes == null) {
                var matcher = (Entitas.Matcher<MainEntity>)Entitas.Matcher<MainEntity>.AllOf(MainComponentsLookup.ReflectionComponentTypes);
                matcher.componentNames = MainComponentsLookup.componentNames;
                _matcherReflectionComponentTypes = matcher;
            }

            return _matcherReflectionComponentTypes;
        }
    }
}
