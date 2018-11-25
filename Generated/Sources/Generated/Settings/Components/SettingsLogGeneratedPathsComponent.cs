namespace GenEntitas {
public partial class SettingsContext {

    public SettingsEntity logGeneratedPathsEntity { get { return GetGroup(SettingsMatcher.LogGeneratedPaths).GetSingleEntity(); } }

    public bool isLogGeneratedPaths {
        get { return logGeneratedPathsEntity != null; }
        set {
            var entity = logGeneratedPathsEntity;
            if (value != (entity != null)) {
                if (value) {
                    CreateEntity().isLogGeneratedPaths = true;
                } else {
                    entity.Destroy();
                }
            }
        }
    }
}

}

namespace GenEntitas {
public partial class SettingsEntity {

    static readonly GenEntitas.LogGeneratedPaths logGeneratedPathsComponent = new GenEntitas.LogGeneratedPaths();

    public bool isLogGeneratedPaths {
        get { return HasComponent(SettingsComponentsLookup.LogGeneratedPaths); }
        set {
            if (value != isLogGeneratedPaths) {
                var index = SettingsComponentsLookup.LogGeneratedPaths;
                if (value) {
                    var componentPool = GetComponentPool(index);
                    var component = componentPool.Count > 0
                            ? componentPool.Pop()
                            : logGeneratedPathsComponent;

                    AddComponent(index, component);
                } else {
                    RemoveComponent(index);
                }
            }
        }
    }
}

}

namespace GenEntitas {
public sealed partial class SettingsMatcher {

    static Entitas.IMatcher<SettingsEntity> _matcherLogGeneratedPaths;

    public static Entitas.IMatcher<SettingsEntity> LogGeneratedPaths {
        get {
            if (_matcherLogGeneratedPaths == null) {
                var matcher = (Entitas.Matcher<SettingsEntity>)Entitas.Matcher<SettingsEntity>.AllOf(SettingsComponentsLookup.LogGeneratedPaths);
                matcher.componentNames = SettingsComponentsLookup.componentNames;
                _matcherLogGeneratedPaths = matcher;
            }

            return _matcherLogGeneratedPaths;
        }
    }
}

}
