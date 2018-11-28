namespace GenEntitas {
public partial class MainContext {

    public MainEntity importedSystemsEntity { get { return GetGroup(MainMatcher.ImportedSystems).GetSingleEntity(); } }
    public GenEntitas.ImportedSystemsComponent importedSystems { get { return importedSystemsEntity.importedSystems; } }
    public bool hasImportedSystems { get { return importedSystemsEntity != null; } }

    public MainEntity SetImportedSystems(System.Collections.Generic.List<Entitas.IExecuteSystem> newValues) {
        if (hasImportedSystems) {
            throw new Entitas.EntitasException("Could not set ImportedSystems!\n" + this + " already has an entity with GenEntitas.ImportedSystemsComponent!",
                "You should check if the context already has a importedSystemsEntity before setting it or use context.ReplaceImportedSystems().");
        }
        var entity = CreateEntity();
        entity.AddImportedSystems(newValues);
        return entity;
    }

    public void ReplaceImportedSystems(System.Collections.Generic.List<Entitas.IExecuteSystem> newValues) {
        var entity = importedSystemsEntity;
        if (entity == null) {
            entity = SetImportedSystems(newValues);
        } else {
            entity.ReplaceImportedSystems(newValues);
        }
    }

    public void RemoveImportedSystems() {
        importedSystemsEntity.Destroy();
    }
}

}

namespace GenEntitas {
public partial class MainEntity {

    public GenEntitas.ImportedSystemsComponent importedSystems { get { return (GenEntitas.ImportedSystemsComponent)GetComponent(MainComponentsLookup.ImportedSystems); } }
    public bool hasImportedSystems { get { return HasComponent(MainComponentsLookup.ImportedSystems); } }

    public void AddImportedSystems(System.Collections.Generic.List<Entitas.IExecuteSystem> newValues) {
        var index = MainComponentsLookup.ImportedSystems;
        var component = (GenEntitas.ImportedSystemsComponent)CreateComponent(index, typeof(GenEntitas.ImportedSystemsComponent));
        component.Values = newValues;
        AddComponent(index, component);
    }

    public void ReplaceImportedSystems(System.Collections.Generic.List<Entitas.IExecuteSystem> newValues) {
        var index = MainComponentsLookup.ImportedSystems;
        var component = (GenEntitas.ImportedSystemsComponent)CreateComponent(index, typeof(GenEntitas.ImportedSystemsComponent));
        component.Values = newValues;
        ReplaceComponent(index, component);
    }

    public void RemoveImportedSystems() {
        RemoveComponent(MainComponentsLookup.ImportedSystems);
    }
}

}

namespace GenEntitas {
public sealed partial class MainMatcher {

    static Entitas.IMatcher<MainEntity> _matcherImportedSystems;

    public static Entitas.IMatcher<MainEntity> ImportedSystems {
        get {
            if (_matcherImportedSystems == null) {
                var matcher = (Entitas.Matcher<MainEntity>)Entitas.Matcher<MainEntity>.AllOf(MainComponentsLookup.ImportedSystems);
                matcher.componentNames = MainComponentsLookup.componentNames;
                _matcherImportedSystems = matcher;
            }

            return _matcherImportedSystems;
        }
    }
}

}
