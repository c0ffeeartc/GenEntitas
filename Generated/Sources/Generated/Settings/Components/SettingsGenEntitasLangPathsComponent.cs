namespace GenEntitas {
public partial class SettingsContext {

    public SettingsEntity genEntitasLangPathsEntity { get { return GetGroup(SettingsMatcher.GenEntitasLangPaths).GetSingleEntity(); } }
    public GenEntitas.GenEntitasLangPaths genEntitasLangPaths { get { return genEntitasLangPathsEntity.genEntitasLangPaths; } }
    public bool hasGenEntitasLangPaths { get { return genEntitasLangPathsEntity != null; } }

    public SettingsEntity SetGenEntitasLangPaths(System.Collections.Generic.List<string> newValues) {
        if (hasGenEntitasLangPaths) {
            throw new Entitas.EntitasException("Could not set GenEntitasLangPaths!\n" + this + " already has an entity with GenEntitas.GenEntitasLangPaths!",
                "You should check if the context already has a genEntitasLangPathsEntity before setting it or use context.ReplaceGenEntitasLangPaths().");
        }
        var entity = CreateEntity();
        entity.AddGenEntitasLangPaths(newValues);
        return entity;
    }

    public void ReplaceGenEntitasLangPaths(System.Collections.Generic.List<string> newValues) {
        var entity = genEntitasLangPathsEntity;
        if (entity == null) {
            entity = SetGenEntitasLangPaths(newValues);
        } else {
            entity.ReplaceGenEntitasLangPaths(newValues);
        }
    }

    public void RemoveGenEntitasLangPaths() {
        genEntitasLangPathsEntity.Destroy();
    }
}

}

namespace GenEntitas {
public partial class SettingsEntity {

    public GenEntitas.GenEntitasLangPaths genEntitasLangPaths { get { return (GenEntitas.GenEntitasLangPaths)GetComponent(SettingsComponentsLookup.GenEntitasLangPaths); } }
    public bool hasGenEntitasLangPaths { get { return HasComponent(SettingsComponentsLookup.GenEntitasLangPaths); } }

    public void AddGenEntitasLangPaths(System.Collections.Generic.List<string> newValues) {
        var index = SettingsComponentsLookup.GenEntitasLangPaths;
        var component = CreateComponent<GenEntitas.GenEntitasLangPaths>(index);
        component.Values = newValues;
        AddComponent(index, component);
    }

    public void ReplaceGenEntitasLangPaths(System.Collections.Generic.List<string> newValues) {
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
