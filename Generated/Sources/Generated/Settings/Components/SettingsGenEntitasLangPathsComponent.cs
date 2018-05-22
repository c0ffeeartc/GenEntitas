namespace GenEntitas {
public partial class SettingsEntity {

    public GenEntitas.GenEntitasLangPaths genEntitasLangPaths { get { return (GenEntitas.GenEntitasLangPaths)GetComponent(SettingsComponentsLookup.GenEntitasLangPaths); } }
    public bool hasGenEntitasLangPaths { get { return HasComponent(SettingsComponentsLookup.GenEntitasLangPaths); } }

    public void AddGenEntitasLangPaths(string newValues) {
        var index = SettingsComponentsLookup.GenEntitasLangPaths;
        var component = CreateComponent<GenEntitas.GenEntitasLangPaths>(index);
        component.Values = newValues;
        AddComponent(index, component);
    }

    public void ReplaceGenEntitasLangPaths(string newValues) {
        var index = SettingsComponentsLookup.GenEntitasLangPaths;
        var component = CreateComponent<GenEntitas.GenEntitasLangPaths>(index);
        component.Values = newValues;
        ReplaceComponent(index, component);
    }

    public void RemoveGenEntitasLangPaths() {
        RemoveComponent(SettingsComponentsLookup.GenEntitasLangPaths);
    }
}

}

namespace GenEntitas {
public sealed partial class SettingsMatcher {

    static Entitas.IMatcher<SettingsEntity> _matcherGenEntitasLangPaths;

    public static Entitas.IMatcher<SettingsEntity> GenEntitasLangPaths {
        get {
            if (_matcherGenEntitasLangPaths == null) {
                var matcher = (Entitas.Matcher<SettingsEntity>)Entitas.Matcher<SettingsEntity>.AllOf(SettingsComponentsLookup.GenEntitasLangPaths);
                matcher.componentNames = SettingsComponentsLookup.componentNames;
                _matcherGenEntitasLangPaths = matcher;
            }

            return _matcherGenEntitasLangPaths;
        }
    }
}

}
