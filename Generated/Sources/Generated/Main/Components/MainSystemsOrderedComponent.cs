namespace GenEntitas {
public partial class MainContext {

    public MainEntity systemsOrderedEntity { get { return GetGroup(MainMatcher.SystemsOrdered).GetSingleEntity(); } }
    public GenEntitas.SystemsOrderedComponent systemsOrdered { get { return systemsOrderedEntity.systemsOrdered; } }
    public bool hasSystemsOrdered { get { return systemsOrderedEntity != null; } }

    public MainEntity SetSystemsOrdered(System.Collections.Generic.List<Entitas.ISystem> newValues) {
        if (hasSystemsOrdered) {
            throw new Entitas.EntitasException("Could not set SystemsOrdered!\n" + this + " already has an entity with GenEntitas.SystemsOrderedComponent!",
                "You should check if the context already has a systemsOrderedEntity before setting it or use context.ReplaceSystemsOrdered().");
        }
        var entity = CreateEntity();
        entity.AddSystemsOrdered(newValues);
        return entity;
    }

    public void ReplaceSystemsOrdered(System.Collections.Generic.List<Entitas.ISystem> newValues) {
        var entity = systemsOrderedEntity;
        if (entity == null) {
            entity = SetSystemsOrdered(newValues);
        } else {
            entity.ReplaceSystemsOrdered(newValues);
        }
    }

    public void RemoveSystemsOrdered() {
        systemsOrderedEntity.Destroy();
    }
}

}

namespace GenEntitas {
public partial class MainEntity {

    public GenEntitas.SystemsOrderedComponent systemsOrdered { get { return (GenEntitas.SystemsOrderedComponent)GetComponent(MainComponentsLookup.SystemsOrdered); } }
    public bool hasSystemsOrdered { get { return HasComponent(MainComponentsLookup.SystemsOrdered); } }

    public void AddSystemsOrdered(System.Collections.Generic.List<Entitas.ISystem> newValues) {
        var index = MainComponentsLookup.SystemsOrdered;
        var component = (GenEntitas.SystemsOrderedComponent)CreateComponent(index, typeof(GenEntitas.SystemsOrderedComponent));
        component.Values = newValues;
        AddComponent(index, component);
    }

    public void ReplaceSystemsOrdered(System.Collections.Generic.List<Entitas.ISystem> newValues) {
        var index = MainComponentsLookup.SystemsOrdered;
        var component = (GenEntitas.SystemsOrderedComponent)CreateComponent(index, typeof(GenEntitas.SystemsOrderedComponent));
        component.Values = newValues;
        ReplaceComponent(index, component);
    }

    public void RemoveSystemsOrdered() {
        RemoveComponent(MainComponentsLookup.SystemsOrdered);
    }
}

}

namespace GenEntitas {
public sealed partial class MainMatcher {

    static Entitas.IMatcher<MainEntity> _matcherSystemsOrdered;

    public static Entitas.IMatcher<MainEntity> SystemsOrdered {
        get {
            if (_matcherSystemsOrdered == null) {
                var matcher = (Entitas.Matcher<MainEntity>)Entitas.Matcher<MainEntity>.AllOf(MainComponentsLookup.SystemsOrdered);
                matcher.componentNames = MainComponentsLookup.componentNames;
                _matcherSystemsOrdered = matcher;
            }

            return _matcherSystemsOrdered;
        }
    }
}

}
