namespace GenEntitas {
public partial class SettingsContext {

    public SettingsEntity settingsParseInputEntity { get { return GetGroup(SettingsMatcher.SettingsParseInput).GetSingleEntity(); } }
    public GenEntitas.SettingsParseInput settingsParseInput { get { return settingsParseInputEntity.settingsParseInput; } }
    public bool hasSettingsParseInput { get { return settingsParseInputEntity != null; } }

    public SettingsEntity SetSettingsParseInput(string newValue) {
        if (hasSettingsParseInput) {
            throw new Entitas.EntitasException("Could not set SettingsParseInput!\n" + this + " already has an entity with GenEntitas.SettingsParseInput!",
                "You should check if the context already has a settingsParseInputEntity before setting it or use context.ReplaceSettingsParseInput().");
        }
        var entity = CreateEntity();
        entity.AddSettingsParseInput(newValue);
        return entity;
    }

    public void ReplaceSettingsParseInput(string newValue) {
        var entity = settingsParseInputEntity;
        if (entity == null) {
            entity = SetSettingsParseInput(newValue);
        } else {
            entity.ReplaceSettingsParseInput(newValue);
        }
    }

    public void RemoveSettingsParseInput() {
        settingsParseInputEntity.Destroy();
    }
}

}

namespace GenEntitas {
public partial class SettingsEntity {

    public GenEntitas.SettingsParseInput settingsParseInput { get { return (GenEntitas.SettingsParseInput)GetComponent(SettingsComponentsLookup.SettingsParseInput); } }
    public bool hasSettingsParseInput { get { return HasComponent(SettingsComponentsLookup.SettingsParseInput); } }

    public void AddSettingsParseInput(string newValue) {
        var index = SettingsComponentsLookup.SettingsParseInput;
        var component = CreateComponent<GenEntitas.SettingsParseInput>(index);
        component.Value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceSettingsParseInput(string newValue) {
        var index = SettingsComponentsLookup.SettingsParseInput;
        var component = CreateComponent<GenEntitas.SettingsParseInput>(index);
        component.Value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveSettingsParseInput() {
        RemoveComponent(SettingsComponentsLookup.SettingsParseInput);
    }
}

}

namespace GenEntitas {
public sealed partial class SettingsMatcher {

    static Entitas.IMatcher<SettingsEntity> _matcherSettingsParseInput;

    public static Entitas.IMatcher<SettingsEntity> SettingsParseInput {
        get {
            if (_matcherSettingsParseInput == null) {
                var matcher = (Entitas.Matcher<SettingsEntity>)Entitas.Matcher<SettingsEntity>.AllOf(SettingsComponentsLookup.SettingsParseInput);
                matcher.componentNames = SettingsComponentsLookup.componentNames;
                _matcherSettingsParseInput = matcher;
            }

            return _matcherSettingsParseInput;
        }
    }
}

}
