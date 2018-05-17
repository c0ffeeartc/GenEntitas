public partial class SettingsContext {

    public SettingsEntity consoleWriteLineGeneratedPathsEntity { get { return GetGroup(SettingsMatcher.ConsoleWriteLineGeneratedPaths).GetSingleEntity(); } }

    public bool isConsoleWriteLineGeneratedPaths {
        get { return consoleWriteLineGeneratedPathsEntity != null; }
        set {
            var entity = consoleWriteLineGeneratedPathsEntity;
            if (value != (entity != null)) {
                if (value) {
                    CreateEntity().isConsoleWriteLineGeneratedPaths = true;
                } else {
                    entity.Destroy();
                }
            }
        }
    }
}

public partial class SettingsEntity {

    static readonly GenEntitas.ConsoleWriteLineGeneratedPaths consoleWriteLineGeneratedPathsComponent = new GenEntitas.ConsoleWriteLineGeneratedPaths();

    public bool isConsoleWriteLineGeneratedPaths {
        get { return HasComponent(SettingsComponentsLookup.ConsoleWriteLineGeneratedPaths); }
        set {
            if (value != isConsoleWriteLineGeneratedPaths) {
                var index = SettingsComponentsLookup.ConsoleWriteLineGeneratedPaths;
                if (value) {
                    var componentPool = GetComponentPool(index);
                    var component = componentPool.Count > 0
                            ? componentPool.Pop()
                            : consoleWriteLineGeneratedPathsComponent;

                    AddComponent(index, component);
                } else {
                    RemoveComponent(index);
                }
            }
        }
    }
}

public sealed partial class SettingsMatcher {

    static Entitas.IMatcher<SettingsEntity> _matcherConsoleWriteLineGeneratedPaths;

    public static Entitas.IMatcher<SettingsEntity> ConsoleWriteLineGeneratedPaths {
        get {
            if (_matcherConsoleWriteLineGeneratedPaths == null) {
                var matcher = (Entitas.Matcher<SettingsEntity>)Entitas.Matcher<SettingsEntity>.AllOf(SettingsComponentsLookup.ConsoleWriteLineGeneratedPaths);
                matcher.componentNames = SettingsComponentsLookup.componentNames;
                _matcherConsoleWriteLineGeneratedPaths = matcher;
            }

            return _matcherConsoleWriteLineGeneratedPaths;
        }
    }
}
