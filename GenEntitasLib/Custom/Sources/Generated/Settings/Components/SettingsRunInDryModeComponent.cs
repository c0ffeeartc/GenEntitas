namespace GenEntitas {
public partial class SettingsContext {

    public SettingsEntity runInDryModeEntity { get { return GetGroup(SettingsMatcher.RunInDryMode).GetSingleEntity(); } }

    public bool isRunInDryMode {
        get { return runInDryModeEntity != null; }
        set {
            var entity = runInDryModeEntity;
            if (value != (entity != null)) {
                if (value) {
                    CreateEntity().isRunInDryMode = true;
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

    static readonly GenEntitas.RunInDryMode runInDryModeComponent = new GenEntitas.RunInDryMode();

    public bool isRunInDryMode {
        get { return HasComponent(SettingsComponentsLookup.RunInDryMode); }
        set {
            if (value != isRunInDryMode) {
                var index = SettingsComponentsLookup.RunInDryMode;
                if (value) {
                    var componentPool = GetComponentPool(index);
                    var component = componentPool.Count > 0
                            ? componentPool.Pop()
                            : runInDryModeComponent;

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

    static Entitas.IMatcher<SettingsEntity> _matcherRunInDryMode;

    public static Entitas.IMatcher<SettingsEntity> RunInDryMode {
        get {
            if (_matcherRunInDryMode == null) {
                var matcher = (Entitas.Matcher<SettingsEntity>)Entitas.Matcher<SettingsEntity>.AllOf(SettingsComponentsLookup.RunInDryMode);
                matcher.componentNames = SettingsComponentsLookup.componentNames;
                _matcherRunInDryMode = matcher;
            }

            return _matcherRunInDryMode;
        }
    }
}

}
