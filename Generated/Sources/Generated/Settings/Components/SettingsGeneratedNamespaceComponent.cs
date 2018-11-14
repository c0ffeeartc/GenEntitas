namespace GenEntitas {
public partial class SettingsContext {

    public SettingsEntity generatedNamespaceEntity { get { return GetGroup(SettingsMatcher.GeneratedNamespace).GetSingleEntity(); } }
    public GenEntitas.GeneratedNamespace generatedNamespace { get { return generatedNamespaceEntity.generatedNamespace; } }
    public bool hasGeneratedNamespace { get { return generatedNamespaceEntity != null; } }

    public SettingsEntity SetGeneratedNamespace(string newValue) {
        if (hasGeneratedNamespace) {
            throw new Entitas.EntitasException("Could not set GeneratedNamespace!\n" + this + " already has an entity with GenEntitas.GeneratedNamespace!",
                "You should check if the context already has a generatedNamespaceEntity before setting it or use context.ReplaceGeneratedNamespace().");
        }
        var entity = CreateEntity();
        entity.AddGeneratedNamespace(newValue);
        return entity;
    }

    public void ReplaceGeneratedNamespace(string newValue) {
        var entity = generatedNamespaceEntity;
        if (entity == null) {
            entity = SetGeneratedNamespace(newValue);
        } else {
            entity.ReplaceGeneratedNamespace(newValue);
        }
    }

    public void RemoveGeneratedNamespace() {
        generatedNamespaceEntity.Destroy();
    }
}

}

namespace GenEntitas {
public partial class SettingsEntity {

    public GenEntitas.GeneratedNamespace generatedNamespace { get { return (GenEntitas.GeneratedNamespace)GetComponent(SettingsComponentsLookup.GeneratedNamespace); } }
    public bool hasGeneratedNamespace { get { return HasComponent(SettingsComponentsLookup.GeneratedNamespace); } }

    public void AddGeneratedNamespace(string newValue) {
        var index = SettingsComponentsLookup.GeneratedNamespace;
        var component = (GenEntitas.GeneratedNamespace)CreateComponent(index, typeof(GenEntitas.GeneratedNamespace));
        component.Value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceGeneratedNamespace(string newValue) {
        var index = SettingsComponentsLookup.GeneratedNamespace;
        var component = (GenEntitas.GeneratedNamespace)CreateComponent(index, typeof(GenEntitas.GeneratedNamespace));
        component.Value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveGeneratedNamespace() {
        RemoveComponent(SettingsComponentsLookup.GeneratedNamespace);
    }
}

}

namespace GenEntitas {
public sealed partial class SettingsMatcher {

    static Entitas.IMatcher<SettingsEntity> _matcherGeneratedNamespace;

    public static Entitas.IMatcher<SettingsEntity> GeneratedNamespace {
        get {
            if (_matcherGeneratedNamespace == null) {
                var matcher = (Entitas.Matcher<SettingsEntity>)Entitas.Matcher<SettingsEntity>.AllOf(SettingsComponentsLookup.GeneratedNamespace);
                matcher.componentNames = SettingsComponentsLookup.componentNames;
                _matcherGeneratedNamespace = matcher;
            }

            return _matcherGeneratedNamespace;
        }
    }
}

}
