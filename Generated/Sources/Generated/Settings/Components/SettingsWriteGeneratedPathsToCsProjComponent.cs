namespace GenEntitas {
public partial class SettingsContext {

    public SettingsEntity writeGeneratedPathsToCsProjEntity { get { return GetGroup(SettingsMatcher.WriteGeneratedPathsToCsProj).GetSingleEntity(); } }
    public GenEntitas.WriteGeneratedPathsToCsProj writeGeneratedPathsToCsProj { get { return writeGeneratedPathsToCsProjEntity.writeGeneratedPathsToCsProj; } }
    public bool hasWriteGeneratedPathsToCsProj { get { return writeGeneratedPathsToCsProjEntity != null; } }

    public SettingsEntity SetWriteGeneratedPathsToCsProj(string newValue) {
        if (hasWriteGeneratedPathsToCsProj) {
            throw new Entitas.EntitasException("Could not set WriteGeneratedPathsToCsProj!\n" + this + " already has an entity with GenEntitas.WriteGeneratedPathsToCsProj!",
                "You should check if the context already has a writeGeneratedPathsToCsProjEntity before setting it or use context.ReplaceWriteGeneratedPathsToCsProj().");
        }
        var entity = CreateEntity();
        entity.AddWriteGeneratedPathsToCsProj(newValue);
        return entity;
    }

    public void ReplaceWriteGeneratedPathsToCsProj(string newValue) {
        var entity = writeGeneratedPathsToCsProjEntity;
        if (entity == null) {
            entity = SetWriteGeneratedPathsToCsProj(newValue);
        } else {
            entity.ReplaceWriteGeneratedPathsToCsProj(newValue);
        }
    }

    public void RemoveWriteGeneratedPathsToCsProj() {
        writeGeneratedPathsToCsProjEntity.Destroy();
    }
}

}

namespace GenEntitas {
public partial class SettingsEntity {

    public GenEntitas.WriteGeneratedPathsToCsProj writeGeneratedPathsToCsProj { get { return (GenEntitas.WriteGeneratedPathsToCsProj)GetComponent(SettingsComponentsLookup.WriteGeneratedPathsToCsProj); } }
    public bool hasWriteGeneratedPathsToCsProj { get { return HasComponent(SettingsComponentsLookup.WriteGeneratedPathsToCsProj); } }

    public void AddWriteGeneratedPathsToCsProj(string newValue) {
        var index = SettingsComponentsLookup.WriteGeneratedPathsToCsProj;
        var component = (GenEntitas.WriteGeneratedPathsToCsProj)CreateComponent(index, typeof(GenEntitas.WriteGeneratedPathsToCsProj));
        component.Value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceWriteGeneratedPathsToCsProj(string newValue) {
        var index = SettingsComponentsLookup.WriteGeneratedPathsToCsProj;
        var component = (GenEntitas.WriteGeneratedPathsToCsProj)CreateComponent(index, typeof(GenEntitas.WriteGeneratedPathsToCsProj));
        component.Value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveWriteGeneratedPathsToCsProj() {
        RemoveComponent(SettingsComponentsLookup.WriteGeneratedPathsToCsProj);
    }
}

}

namespace GenEntitas {
public sealed partial class SettingsMatcher {

    static Entitas.IMatcher<SettingsEntity> _matcherWriteGeneratedPathsToCsProj;

    public static Entitas.IMatcher<SettingsEntity> WriteGeneratedPathsToCsProj {
        get {
            if (_matcherWriteGeneratedPathsToCsProj == null) {
                var matcher = (Entitas.Matcher<SettingsEntity>)Entitas.Matcher<SettingsEntity>.AllOf(SettingsComponentsLookup.WriteGeneratedPathsToCsProj);
                matcher.componentNames = SettingsComponentsLookup.componentNames;
                _matcherWriteGeneratedPathsToCsProj = matcher;
            }

            return _matcherWriteGeneratedPathsToCsProj;
        }
    }
}

}
