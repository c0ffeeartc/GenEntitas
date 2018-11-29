namespace GenEntitas {
public partial class MainContext {

    public MainEntity systemsImportedEntity { get { return GetGroup(MainMatcher.SystemsImported).GetSingleEntity(); } }
    public GenEntitas.SystemsImportedComponent systemsImported { get { return systemsImportedEntity.systemsImported; } }
    public bool hasSystemsImported { get { return systemsImportedEntity != null; } }

    public MainEntity SetSystemsImported(System.Collections.Generic.List<Entitas.ISystem> newValues) {
        if (hasSystemsImported) {
            throw new Entitas.EntitasException("Could not set SystemsImported!\n" + this + " already has an entity with GenEntitas.SystemsImportedComponent!",
                "You should check if the context already has a systemsImportedEntity before setting it or use context.ReplaceSystemsImported().");
        }
        var entity = CreateEntity();
        entity.AddSystemsImported(newValues);
        return entity;
    }

    public void ReplaceSystemsImported(System.Collections.Generic.List<Entitas.ISystem> newValues) {
        var entity = systemsImportedEntity;
        if (entity == null) {
            entity = SetSystemsImported(newValues);
        } else {
            entity.ReplaceSystemsImported(newValues);
        }
    }

    public void RemoveSystemsImported() {
        systemsImportedEntity.Destroy();
    }
}

}

namespace GenEntitas {
public partial class MainEntity {

    public GenEntitas.SystemsImportedComponent systemsImported { get { return (GenEntitas.SystemsImportedComponent)GetComponent(MainComponentsLookup.SystemsImported); } }
    public bool hasSystemsImported { get { return HasComponent(MainComponentsLookup.SystemsImported); } }

    public void AddSystemsImported(System.Collections.Generic.List<Entitas.ISystem> newValues) {
        var index = MainComponentsLookup.SystemsImported;
        var component = (GenEntitas.SystemsImportedComponent)CreateComponent(index, typeof(GenEntitas.SystemsImportedComponent));
        component.Values = newValues;
        AddComponent(index, component);
    }

    public void ReplaceSystemsImported(System.Collections.Generic.List<Entitas.ISystem> newValues) {
        var index = MainComponentsLookup.SystemsImported;
        var component = (GenEntitas.SystemsImportedComponent)CreateComponent(index, typeof(GenEntitas.SystemsImportedComponent));
        component.Values = newValues;
        ReplaceComponent(index, component);
    }

    public void RemoveSystemsImported() {
        RemoveComponent(MainComponentsLookup.SystemsImported);
    }
}

}

namespace GenEntitas {
public sealed partial class MainMatcher {

    static Entitas.IMatcher<MainEntity> _matcherSystemsImported;

    public static Entitas.IMatcher<MainEntity> SystemsImported {
        get {
            if (_matcherSystemsImported == null) {
                var matcher = (Entitas.Matcher<MainEntity>)Entitas.Matcher<MainEntity>.AllOf(MainComponentsLookup.SystemsImported);
                matcher.componentNames = MainComponentsLookup.componentNames;
                _matcherSystemsImported = matcher;
            }

            return _matcherSystemsImported;
        }
    }
}

}
