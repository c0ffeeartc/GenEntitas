namespace GenEntitas {
public partial class SettingsContext {

    public SettingsEntity csprojPathEntity { get { return GetGroup(SettingsMatcher.CsprojPath).GetSingleEntity(); } }
    public GenEntitas.CsprojPath csprojPath { get { return csprojPathEntity.csprojPath; } }
    public bool hasCsprojPath { get { return csprojPathEntity != null; } }

    public SettingsEntity SetCsprojPath(string newValue) {
        if (hasCsprojPath) {
            throw new Entitas.EntitasException("Could not set CsprojPath!\n" + this + " already has an entity with GenEntitas.CsprojPath!",
                "You should check if the context already has a csprojPathEntity before setting it or use context.ReplaceCsprojPath().");
        }
        var entity = CreateEntity();
        entity.AddCsprojPath(newValue);
        return entity;
    }

    public void ReplaceCsprojPath(string newValue) {
        var entity = csprojPathEntity;
        if (entity == null) {
            entity = SetCsprojPath(newValue);
        } else {
            entity.ReplaceCsprojPath(newValue);
        }
    }

    public void RemoveCsprojPath() {
        csprojPathEntity.Destroy();
    }
}

}

namespace GenEntitas {
public partial class SettingsEntity {

    public GenEntitas.CsprojPath csprojPath { get { return (GenEntitas.CsprojPath)GetComponent(SettingsComponentsLookup.CsprojPath); } }
    public bool hasCsprojPath { get { return HasComponent(SettingsComponentsLookup.CsprojPath); } }

    public void AddCsprojPath(string newValue) {
        var index = SettingsComponentsLookup.CsprojPath;
        var component = CreateComponent<GenEntitas.CsprojPath>(index);
        component.Value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceCsprojPath(string newValue) {
        var index = SettingsComponentsLookup.CsprojPath;
        var component = CreateComponent<GenEntitas.CsprojPath>(index);
        component.Value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveCsprojPath() {
        RemoveComponent(SettingsComponentsLookup.CsprojPath);
    }
}

}

namespace GenEntitas {
public sealed partial class SettingsMatcher {

    static Entitas.IMatcher<SettingsEntity> _matcherCsprojPath;

    public static Entitas.IMatcher<SettingsEntity> CsprojPath {
        get {
            if (_matcherCsprojPath == null) {
                var matcher = (Entitas.Matcher<SettingsEntity>)Entitas.Matcher<SettingsEntity>.AllOf(SettingsComponentsLookup.CsprojPath);
                matcher.componentNames = SettingsComponentsLookup.componentNames;
                _matcherCsprojPath = matcher;
            }

            return _matcherCsprojPath;
        }
    }
}

}
