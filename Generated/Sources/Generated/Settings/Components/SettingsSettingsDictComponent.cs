namespace GenEntitas {
public partial class SettingsContext {

    public SettingsEntity settingsDictEntity { get { return GetGroup(SettingsMatcher.SettingsDict).GetSingleEntity(); } }
    public GenEntitas.SettingsDictComponent settingsDict { get { return settingsDictEntity.settingsDict; } }
    public bool hasSettingsDict { get { return settingsDictEntity != null; } }

    public SettingsEntity SetSettingsDict(System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>> newDict) {
        if (hasSettingsDict) {
            throw new Entitas.EntitasException("Could not set SettingsDict!\n" + this + " already has an entity with GenEntitas.SettingsDictComponent!",
                "You should check if the context already has a settingsDictEntity before setting it or use context.ReplaceSettingsDict().");
        }
        var entity = CreateEntity();
        entity.AddSettingsDict(newDict);
        return entity;
    }

    public void ReplaceSettingsDict(System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>> newDict) {
        var entity = settingsDictEntity;
        if (entity == null) {
            entity = SetSettingsDict(newDict);
        } else {
            entity.ReplaceSettingsDict(newDict);
        }
    }

    public void RemoveSettingsDict() {
        settingsDictEntity.Destroy();
    }
}

}

namespace GenEntitas {
public partial class SettingsEntity {

    public GenEntitas.SettingsDictComponent settingsDict { get { return (GenEntitas.SettingsDictComponent)GetComponent(SettingsComponentsLookup.SettingsDict); } }
    public bool hasSettingsDict { get { return HasComponent(SettingsComponentsLookup.SettingsDict); } }

    public void AddSettingsDict(System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>> newDict) {
        var index = SettingsComponentsLookup.SettingsDict;
        var component = (GenEntitas.SettingsDictComponent)CreateComponent(index, typeof(GenEntitas.SettingsDictComponent));
        component.Dict = newDict;
        AddComponent(index, component);
    }

    public void ReplaceSettingsDict(System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>> newDict) {
        var index = SettingsComponentsLookup.SettingsDict;
        var component = (GenEntitas.SettingsDictComponent)CreateComponent(index, typeof(GenEntitas.SettingsDictComponent));
        component.Dict = newDict;
        ReplaceComponent(index, component);
    }

    public void RemoveSettingsDict() {
        RemoveComponent(SettingsComponentsLookup.SettingsDict);
    }
}

}

namespace GenEntitas {
public sealed partial class SettingsMatcher {

    static Entitas.IMatcher<SettingsEntity> _matcherSettingsDict;

    public static Entitas.IMatcher<SettingsEntity> SettingsDict {
        get {
            if (_matcherSettingsDict == null) {
                var matcher = (Entitas.Matcher<SettingsEntity>)Entitas.Matcher<SettingsEntity>.AllOf(SettingsComponentsLookup.SettingsDict);
                matcher.componentNames = SettingsComponentsLookup.componentNames;
                _matcherSettingsDict = matcher;
            }

            return _matcherSettingsDict;
        }
    }
}

}
