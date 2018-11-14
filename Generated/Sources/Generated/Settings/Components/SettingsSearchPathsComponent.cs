namespace GenEntitas {
public partial class SettingsContext {

    public SettingsEntity searchPathsEntity { get { return GetGroup(SettingsMatcher.SearchPaths).GetSingleEntity(); } }
    public GenEntitas.SearchPaths searchPaths { get { return searchPathsEntity.searchPaths; } }
    public bool hasSearchPaths { get { return searchPathsEntity != null; } }

    public SettingsEntity SetSearchPaths(System.Collections.Generic.List<string> newValue) {
        if (hasSearchPaths) {
            throw new Entitas.EntitasException("Could not set SearchPaths!\n" + this + " already has an entity with GenEntitas.SearchPaths!",
                "You should check if the context already has a searchPathsEntity before setting it or use context.ReplaceSearchPaths().");
        }
        var entity = CreateEntity();
        entity.AddSearchPaths(newValue);
        return entity;
    }

    public void ReplaceSearchPaths(System.Collections.Generic.List<string> newValue) {
        var entity = searchPathsEntity;
        if (entity == null) {
            entity = SetSearchPaths(newValue);
        } else {
            entity.ReplaceSearchPaths(newValue);
        }
    }

    public void RemoveSearchPaths() {
        searchPathsEntity.Destroy();
    }
}

}

namespace GenEntitas {
public partial class SettingsEntity {

    public GenEntitas.SearchPaths searchPaths { get { return (GenEntitas.SearchPaths)GetComponent(SettingsComponentsLookup.SearchPaths); } }
    public bool hasSearchPaths { get { return HasComponent(SettingsComponentsLookup.SearchPaths); } }

    public void AddSearchPaths(System.Collections.Generic.List<string> newValue) {
        var index = SettingsComponentsLookup.SearchPaths;
        var component = (GenEntitas.SearchPaths)CreateComponent(index, typeof(GenEntitas.SearchPaths));
        component.Value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceSearchPaths(System.Collections.Generic.List<string> newValue) {
        var index = SettingsComponentsLookup.SearchPaths;
        var component = (GenEntitas.SearchPaths)CreateComponent(index, typeof(GenEntitas.SearchPaths));
        component.Value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveSearchPaths() {
        RemoveComponent(SettingsComponentsLookup.SearchPaths);
    }
}

}

namespace GenEntitas {
public sealed partial class SettingsMatcher {

    static Entitas.IMatcher<SettingsEntity> _matcherSearchPaths;

    public static Entitas.IMatcher<SettingsEntity> SearchPaths {
        get {
            if (_matcherSearchPaths == null) {
                var matcher = (Entitas.Matcher<SettingsEntity>)Entitas.Matcher<SettingsEntity>.AllOf(SettingsComponentsLookup.SearchPaths);
                matcher.componentNames = SettingsComponentsLookup.componentNames;
                _matcherSearchPaths = matcher;
            }

            return _matcherSearchPaths;
        }
    }
}

}
