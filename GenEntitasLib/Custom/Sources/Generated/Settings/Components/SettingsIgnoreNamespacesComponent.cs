namespace GenEntitas {
public partial class SettingsContext {

    public SettingsEntity ignoreNamespacesEntity { get { return GetGroup(SettingsMatcher.IgnoreNamespaces).GetSingleEntity(); } }

    public bool isIgnoreNamespaces {
        get { return ignoreNamespacesEntity != null; }
        set {
            var entity = ignoreNamespacesEntity;
            if (value != (entity != null)) {
                if (value) {
                    CreateEntity().isIgnoreNamespaces = true;
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

    static readonly GenEntitas.IgnoreNamespaces ignoreNamespacesComponent = new GenEntitas.IgnoreNamespaces();

    public bool isIgnoreNamespaces {
        get { return HasComponent(SettingsComponentsLookup.IgnoreNamespaces); }
        set {
            if (value != isIgnoreNamespaces) {
                var index = SettingsComponentsLookup.IgnoreNamespaces;
                if (value) {
                    var componentPool = GetComponentPool(index);
                    var component = componentPool.Count > 0
                            ? componentPool.Pop()
                            : ignoreNamespacesComponent;

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

    static Entitas.IMatcher<SettingsEntity> _matcherIgnoreNamespaces;

    public static Entitas.IMatcher<SettingsEntity> IgnoreNamespaces {
        get {
            if (_matcherIgnoreNamespaces == null) {
                var matcher = (Entitas.Matcher<SettingsEntity>)Entitas.Matcher<SettingsEntity>.AllOf(SettingsComponentsLookup.IgnoreNamespaces);
                matcher.componentNames = SettingsComponentsLookup.componentNames;
                _matcherIgnoreNamespaces = matcher;
            }

            return _matcherIgnoreNamespaces;
        }
    }
}

}
