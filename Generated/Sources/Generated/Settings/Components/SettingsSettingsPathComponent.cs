namespace GenEntitas {
public partial class SettingsContext {

    public SettingsEntity settingsPathEntity { get { return GetGroup(SettingsMatcher.SettingsPath).GetSingleEntity(); } }
    public GenEntitas.SettingsPath settingsPath { get { return settingsPathEntity.settingsPath; } }
    public bool hasSettingsPath { get { return settingsPathEntity != null; } }

    public SettingsEntity SetSettingsPath(string newValue) {
        if (hasSettingsPath) {
            throw new Entitas.EntitasException("Could not set SettingsPath!\n" + this + " already has an entity with GenEntitas.SettingsPath!",
                "You should check if the context already has a settingsPathEntity before setting it or use context.ReplaceSettingsPath().");
        }
        var entity = CreateEntity();
        entity.AddSettingsPath(newValue);
        return entity;
    }

    public void ReplaceSettingsPath(string newValue) {
        var entity = settingsPathEntity;
        if (entity == null) {
            entity = SetSettingsPath(newValue);
        } else {
            entity.ReplaceSettingsPath(newValue);
        }
    }

    public void RemoveSettingsPath() {
        settingsPathEntity.Destroy();
    }
}

}

namespace GenEntitas {
public partial class SettingsEntity {

    public GenEntitas.SettingsPath settingsPath { get { return (GenEntitas.SettingsPath)GetComponent(SettingsComponentsLookup.SettingsPath); } }
    public bool hasSettingsPath { get { return HasComponent(SettingsComponentsLookup.SettingsPath); } }

    public void AddSettingsPath(string newValue) {
        var index = SettingsComponentsLookup.SettingsPath;
        var component = (GenEntitas.SettingsPath)CreateComponent(index, typeof(GenEntitas.SettingsPath));
        component.Value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceSettingsPath(string newValue) {
        var index = SettingsComponentsLookup.SettingsPath;
        var component = (GenEntitas.SettingsPath)CreateComponent(index, typeof(GenEntitas.SettingsPath));
        component.Value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveSettingsPath() {
        RemoveComponent(SettingsComponentsLookup.SettingsPath);
    }
}

}

namespace GenEntitas {
public sealed partial class SettingsMatcher {

    static Entitas.IMatcher<SettingsEntity> _matcherSettingsPath;

    public static Entitas.IMatcher<SettingsEntity> SettingsPath {
        get {
            if (_matcherSettingsPath == null) {
                var matcher = (Entitas.Matcher<SettingsEntity>)Entitas.Matcher<SettingsEntity>.AllOf(SettingsComponentsLookup.SettingsPath);
                matcher.componentNames = SettingsComponentsLookup.componentNames;
                _matcherSettingsPath = matcher;
            }

            return _matcherSettingsPath;
        }
    }
}

}
