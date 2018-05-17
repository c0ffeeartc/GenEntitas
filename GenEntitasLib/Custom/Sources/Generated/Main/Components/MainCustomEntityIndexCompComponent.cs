public partial class MainEntity {

    public GenEntitas.CustomEntityIndexComp customEntityIndexComp { get { return (GenEntitas.CustomEntityIndexComp)GetComponent(MainComponentsLookup.CustomEntityIndexComp); } }
    public bool hasCustomEntityIndexComp { get { return HasComponent(MainComponentsLookup.CustomEntityIndexComp); } }

    public void AddCustomEntityIndexComp(Entitas.CodeGeneration.Plugins.EntityIndexData newEntityIndexData) {
        var index = MainComponentsLookup.CustomEntityIndexComp;
        var component = CreateComponent<GenEntitas.CustomEntityIndexComp>(index);
        component.EntityIndexData = newEntityIndexData;
        AddComponent(index, component);
    }

    public void ReplaceCustomEntityIndexComp(Entitas.CodeGeneration.Plugins.EntityIndexData newEntityIndexData) {
        var index = MainComponentsLookup.CustomEntityIndexComp;
        var component = CreateComponent<GenEntitas.CustomEntityIndexComp>(index);
        component.EntityIndexData = newEntityIndexData;
        ReplaceComponent(index, component);
    }

    public void RemoveCustomEntityIndexComp() {
        RemoveComponent(MainComponentsLookup.CustomEntityIndexComp);
    }
}

public sealed partial class MainMatcher {

    static Entitas.IMatcher<MainEntity> _matcherCustomEntityIndexComp;

    public static Entitas.IMatcher<MainEntity> CustomEntityIndexComp {
        get {
            if (_matcherCustomEntityIndexComp == null) {
                var matcher = (Entitas.Matcher<MainEntity>)Entitas.Matcher<MainEntity>.AllOf(MainComponentsLookup.CustomEntityIndexComp);
                matcher.componentNames = MainComponentsLookup.componentNames;
                _matcherCustomEntityIndexComp = matcher;
            }

            return _matcherCustomEntityIndexComp;
        }
    }
}
