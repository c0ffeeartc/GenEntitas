namespace GenEntitas {
public partial class SettingsContext {

    public SettingsEntity systemGuidsEntity { get { return GetGroup(SettingsMatcher.SystemGuids).GetSingleEntity(); } }
    public GenEntitas.SystemGuids systemGuids { get { return systemGuidsEntity.systemGuids; } }
    public bool hasSystemGuids { get { return systemGuidsEntity != null; } }

    public SettingsEntity SetSystemGuids(System.Collections.Generic.List<System.Guid> newValues) {
        if (hasSystemGuids) {
            throw new Entitas.EntitasException("Could not set SystemGuids!\n" + this + " already has an entity with GenEntitas.SystemGuids!",
                "You should check if the context already has a systemGuidsEntity before setting it or use context.ReplaceSystemGuids().");
        }
        var entity = CreateEntity();
        entity.AddSystemGuids(newValues);
        return entity;
    }

    public void ReplaceSystemGuids(System.Collections.Generic.List<System.Guid> newValues) {
        var entity = systemGuidsEntity;
        if (entity == null) {
            entity = SetSystemGuids(newValues);
        } else {
            entity.ReplaceSystemGuids(newValues);
        }
    }

    public void RemoveSystemGuids() {
        systemGuidsEntity.Destroy();
    }
}

}

namespace GenEntitas {
public partial class SettingsEntity {

    public GenEntitas.SystemGuids systemGuids { get { return (GenEntitas.SystemGuids)GetComponent(SettingsComponentsLookup.SystemGuids); } }
    public bool hasSystemGuids { get { return HasComponent(SettingsComponentsLookup.SystemGuids); } }

    public void AddSystemGuids(System.Collections.Generic.List<System.Guid> newValues) {
        var index = SettingsComponentsLookup.SystemGuids;
        var component = (GenEntitas.SystemGuids)CreateComponent(index, typeof(GenEntitas.SystemGuids));
        component.Values = newValues;
        AddComponent(index, component);
    }

    public void ReplaceSystemGuids(System.Collections.Generic.List<System.Guid> newValues) {
        var index = SettingsComponentsLookup.SystemGuids;
        var component = (GenEntitas.SystemGuids)CreateComponent(index, typeof(GenEntitas.SystemGuids));
        component.Values = newValues;
        ReplaceComponent(index, component);
    }

    public void RemoveSystemGuids() {
        RemoveComponent(SettingsComponentsLookup.SystemGuids);
    }
}

}

namespace GenEntitas {
public sealed partial class SettingsMatcher {

    static Entitas.IMatcher<SettingsEntity> _matcherSystemGuids;

    public static Entitas.IMatcher<SettingsEntity> SystemGuids {
        get {
            if (_matcherSystemGuids == null) {
                var matcher = (Entitas.Matcher<SettingsEntity>)Entitas.Matcher<SettingsEntity>.AllOf(SettingsComponentsLookup.SystemGuids);
                matcher.componentNames = SettingsComponentsLookup.componentNames;
                _matcherSystemGuids = matcher;
            }

            return _matcherSystemGuids;
        }
    }
}

}
