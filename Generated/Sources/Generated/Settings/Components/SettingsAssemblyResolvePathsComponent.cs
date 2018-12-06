namespace GenEntitas {
public partial class SettingsContext {

    public SettingsEntity assemblyResolvePathsEntity { get { return GetGroup(SettingsMatcher.AssemblyResolvePaths).GetSingleEntity(); } }
    public GenEntitas.AssemblyResolvePaths assemblyResolvePaths { get { return assemblyResolvePathsEntity.assemblyResolvePaths; } }
    public bool hasAssemblyResolvePaths { get { return assemblyResolvePathsEntity != null; } }

    public SettingsEntity SetAssemblyResolvePaths(System.Collections.Generic.List<string> newValue) {
        if (hasAssemblyResolvePaths) {
            throw new Entitas.EntitasException("Could not set AssemblyResolvePaths!\n" + this + " already has an entity with GenEntitas.AssemblyResolvePaths!",
                "You should check if the context already has a assemblyResolvePathsEntity before setting it or use context.ReplaceAssemblyResolvePaths().");
        }
        var entity = CreateEntity();
        entity.AddAssemblyResolvePaths(newValue);
        return entity;
    }

    public void ReplaceAssemblyResolvePaths(System.Collections.Generic.List<string> newValue) {
        var entity = assemblyResolvePathsEntity;
        if (entity == null) {
            entity = SetAssemblyResolvePaths(newValue);
        } else {
            entity.ReplaceAssemblyResolvePaths(newValue);
        }
    }

    public void RemoveAssemblyResolvePaths() {
        assemblyResolvePathsEntity.Destroy();
    }
}

}

namespace GenEntitas {
public partial class SettingsEntity {

    public GenEntitas.AssemblyResolvePaths assemblyResolvePaths { get { return (GenEntitas.AssemblyResolvePaths)GetComponent(SettingsComponentsLookup.AssemblyResolvePaths); } }
    public bool hasAssemblyResolvePaths { get { return HasComponent(SettingsComponentsLookup.AssemblyResolvePaths); } }

    public void AddAssemblyResolvePaths(System.Collections.Generic.List<string> newValue) {
        var index = SettingsComponentsLookup.AssemblyResolvePaths;
        var component = (GenEntitas.AssemblyResolvePaths)CreateComponent(index, typeof(GenEntitas.AssemblyResolvePaths));
        component.Value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceAssemblyResolvePaths(System.Collections.Generic.List<string> newValue) {
        var index = SettingsComponentsLookup.AssemblyResolvePaths;
        var component = (GenEntitas.AssemblyResolvePaths)CreateComponent(index, typeof(GenEntitas.AssemblyResolvePaths));
        component.Value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveAssemblyResolvePaths() {
        RemoveComponent(SettingsComponentsLookup.AssemblyResolvePaths);
    }
}

}

namespace GenEntitas {
public sealed partial class SettingsMatcher {

    static Entitas.IMatcher<SettingsEntity> _matcherAssemblyResolvePaths;

    public static Entitas.IMatcher<SettingsEntity> AssemblyResolvePaths {
        get {
            if (_matcherAssemblyResolvePaths == null) {
                var matcher = (Entitas.Matcher<SettingsEntity>)Entitas.Matcher<SettingsEntity>.AllOf(SettingsComponentsLookup.AssemblyResolvePaths);
                matcher.componentNames = SettingsComponentsLookup.componentNames;
                _matcherAssemblyResolvePaths = matcher;
            }

            return _matcherAssemblyResolvePaths;
        }
    }
}

}
