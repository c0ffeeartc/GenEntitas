namespace GenEntitas {
public partial class SettingsContext {

    public SettingsEntity roslynPathToSolutionEntity { get { return GetGroup(SettingsMatcher.RoslynPathToSolution).GetSingleEntity(); } }
    public GenEntitas.RoslynPathToSolution roslynPathToSolution { get { return roslynPathToSolutionEntity.roslynPathToSolution; } }
    public bool hasRoslynPathToSolution { get { return roslynPathToSolutionEntity != null; } }

    public SettingsEntity SetRoslynPathToSolution(string newValue) {
        if (hasRoslynPathToSolution) {
            throw new Entitas.EntitasException("Could not set RoslynPathToSolution!\n" + this + " already has an entity with GenEntitas.RoslynPathToSolution!",
                "You should check if the context already has a roslynPathToSolutionEntity before setting it or use context.ReplaceRoslynPathToSolution().");
        }
        var entity = CreateEntity();
        entity.AddRoslynPathToSolution(newValue);
        return entity;
    }

    public void ReplaceRoslynPathToSolution(string newValue) {
        var entity = roslynPathToSolutionEntity;
        if (entity == null) {
            entity = SetRoslynPathToSolution(newValue);
        } else {
            entity.ReplaceRoslynPathToSolution(newValue);
        }
    }

    public void RemoveRoslynPathToSolution() {
        roslynPathToSolutionEntity.Destroy();
    }
}

}

namespace GenEntitas {
public partial class SettingsEntity {

    public GenEntitas.RoslynPathToSolution roslynPathToSolution { get { return (GenEntitas.RoslynPathToSolution)GetComponent(SettingsComponentsLookup.RoslynPathToSolution); } }
    public bool hasRoslynPathToSolution { get { return HasComponent(SettingsComponentsLookup.RoslynPathToSolution); } }

    public void AddRoslynPathToSolution(string newValue) {
        var index = SettingsComponentsLookup.RoslynPathToSolution;
        var component = (GenEntitas.RoslynPathToSolution)CreateComponent(index, typeof(GenEntitas.RoslynPathToSolution));
        component.Value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceRoslynPathToSolution(string newValue) {
        var index = SettingsComponentsLookup.RoslynPathToSolution;
        var component = (GenEntitas.RoslynPathToSolution)CreateComponent(index, typeof(GenEntitas.RoslynPathToSolution));
        component.Value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveRoslynPathToSolution() {
        RemoveComponent(SettingsComponentsLookup.RoslynPathToSolution);
    }
}

}

namespace GenEntitas {
public sealed partial class SettingsMatcher {

    static Entitas.IMatcher<SettingsEntity> _matcherRoslynPathToSolution;

    public static Entitas.IMatcher<SettingsEntity> RoslynPathToSolution {
        get {
            if (_matcherRoslynPathToSolution == null) {
                var matcher = (Entitas.Matcher<SettingsEntity>)Entitas.Matcher<SettingsEntity>.AllOf(SettingsComponentsLookup.RoslynPathToSolution);
                matcher.componentNames = SettingsComponentsLookup.componentNames;
                _matcherRoslynPathToSolution = matcher;
            }

            return _matcherRoslynPathToSolution;
        }
    }
}

}
