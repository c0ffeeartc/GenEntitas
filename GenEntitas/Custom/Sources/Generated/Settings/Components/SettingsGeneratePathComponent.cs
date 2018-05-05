public partial class SettingsContext {

    public SettingsEntity generatePathEntity { get { return GetGroup(SettingsMatcher.GeneratePath).GetSingleEntity(); } }
    public GeneratePath generatePath { get { return generatePathEntity.generatePath; } }
    public bool hasGeneratePath { get { return generatePathEntity != null; } }

    public SettingsEntity SetGeneratePath(string newValue) {
        if (hasGeneratePath) {
            throw new Entitas.EntitasException("Could not set GeneratePath!\n" + this + " already has an entity with GeneratePath!",
                "You should check if the context already has a generatePathEntity before setting it or use context.ReplaceGeneratePath().");
        }
        var entity = CreateEntity();
        entity.AddGeneratePath(newValue);
        return entity;
    }

    public void ReplaceGeneratePath(string newValue) {
        var entity = generatePathEntity;
        if (entity == null) {
            entity = SetGeneratePath(newValue);
        } else {
            entity.ReplaceGeneratePath(newValue);
        }
    }

    public void RemoveGeneratePath() {
        generatePathEntity.Destroy();
    }
}

public partial class SettingsEntity {

    public GeneratePath generatePath { get { return (GeneratePath)GetComponent(SettingsComponentsLookup.GeneratePath); } }
    public bool hasGeneratePath { get { return HasComponent(SettingsComponentsLookup.GeneratePath); } }

    public void AddGeneratePath(string newValue) {
        var index = SettingsComponentsLookup.GeneratePath;
        var component = CreateComponent<GeneratePath>(index);
        component.Value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceGeneratePath(string newValue) {
        var index = SettingsComponentsLookup.GeneratePath;
        var component = CreateComponent<GeneratePath>(index);
        component.Value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveGeneratePath() {
        RemoveComponent(SettingsComponentsLookup.GeneratePath);
    }
}

public sealed partial class SettingsMatcher {

    static Entitas.IMatcher<SettingsEntity> _matcherGeneratePath;

    public static Entitas.IMatcher<SettingsEntity> GeneratePath {
        get {
            if (_matcherGeneratePath == null) {
                var matcher = (Entitas.Matcher<SettingsEntity>)Entitas.Matcher<SettingsEntity>.AllOf(SettingsComponentsLookup.GeneratePath);
                matcher.componentNames = SettingsComponentsLookup.componentNames;
                _matcherGeneratePath = matcher;
            }

            return _matcherGeneratePath;
        }
    }
}
