public partial class SettingsContext {

    public SettingsEntity reflectionAssemblyPathsEntity { get { return GetGroup(SettingsMatcher.ReflectionAssemblyPaths).GetSingleEntity(); } }
    public ReflectionAssemblyPaths reflectionAssemblyPaths { get { return reflectionAssemblyPathsEntity.reflectionAssemblyPaths; } }
    public bool hasReflectionAssemblyPaths { get { return reflectionAssemblyPathsEntity != null; } }

    public SettingsEntity SetReflectionAssemblyPaths(System.Collections.Generic.List<string> newValues) {
        if (hasReflectionAssemblyPaths) {
            throw new Entitas.EntitasException("Could not set ReflectionAssemblyPaths!\n" + this + " already has an entity with ReflectionAssemblyPaths!",
                "You should check if the context already has a reflectionAssemblyPathsEntity before setting it or use context.ReplaceReflectionAssemblyPaths().");
        }
        var entity = CreateEntity();
        entity.AddReflectionAssemblyPaths(newValues);
        return entity;
    }

    public void ReplaceReflectionAssemblyPaths(System.Collections.Generic.List<string> newValues) {
        var entity = reflectionAssemblyPathsEntity;
        if (entity == null) {
            entity = SetReflectionAssemblyPaths(newValues);
        } else {
            entity.ReplaceReflectionAssemblyPaths(newValues);
        }
    }

    public void RemoveReflectionAssemblyPaths() {
        reflectionAssemblyPathsEntity.Destroy();
    }
}

public partial class SettingsEntity {

    public ReflectionAssemblyPaths reflectionAssemblyPaths { get { return (ReflectionAssemblyPaths)GetComponent(SettingsComponentsLookup.ReflectionAssemblyPaths); } }
    public bool hasReflectionAssemblyPaths { get { return HasComponent(SettingsComponentsLookup.ReflectionAssemblyPaths); } }

    public void AddReflectionAssemblyPaths(System.Collections.Generic.List<string> newValues) {
        var index = SettingsComponentsLookup.ReflectionAssemblyPaths;
        var component = CreateComponent<ReflectionAssemblyPaths>(index);
        component.Values = newValues;
        AddComponent(index, component);
    }

    public void ReplaceReflectionAssemblyPaths(System.Collections.Generic.List<string> newValues) {
        var index = SettingsComponentsLookup.ReflectionAssemblyPaths;
        var component = CreateComponent<ReflectionAssemblyPaths>(index);
        component.Values = newValues;
        ReplaceComponent(index, component);
    }

    public void RemoveReflectionAssemblyPaths() {
        RemoveComponent(SettingsComponentsLookup.ReflectionAssemblyPaths);
    }
}

public sealed partial class SettingsMatcher {

    static Entitas.IMatcher<SettingsEntity> _matcherReflectionAssemblyPaths;

    public static Entitas.IMatcher<SettingsEntity> ReflectionAssemblyPaths {
        get {
            if (_matcherReflectionAssemblyPaths == null) {
                var matcher = (Entitas.Matcher<SettingsEntity>)Entitas.Matcher<SettingsEntity>.AllOf(SettingsComponentsLookup.ReflectionAssemblyPaths);
                matcher.componentNames = SettingsComponentsLookup.componentNames;
                _matcherReflectionAssemblyPaths = matcher;
            }

            return _matcherReflectionAssemblyPaths;
        }
    }
}
