public partial class MainEntity {

    public GenEntitas.EntityIndexComp entityIndexComp { get { return (GenEntitas.EntityIndexComp)GetComponent(MainComponentsLookup.EntityIndexComp); } }
    public bool hasEntityIndexComp { get { return HasComponent(MainComponentsLookup.EntityIndexComp); } }

    public void AddEntityIndexComp(System.Collections.Generic.List<GenEntitas.EntityIndexInfo> newValues) {
        var index = MainComponentsLookup.EntityIndexComp;
        var component = CreateComponent<GenEntitas.EntityIndexComp>(index);
        component.Values = newValues;
        AddComponent(index, component);
    }

    public void ReplaceEntityIndexComp(System.Collections.Generic.List<GenEntitas.EntityIndexInfo> newValues) {
        var index = MainComponentsLookup.EntityIndexComp;
        var component = CreateComponent<GenEntitas.EntityIndexComp>(index);
        component.Values = newValues;
        ReplaceComponent(index, component);
    }

    public void RemoveEntityIndexComp() {
        RemoveComponent(MainComponentsLookup.EntityIndexComp);
    }
}

public sealed partial class MainMatcher {

    static Entitas.IMatcher<MainEntity> _matcherEntityIndexComp;

    public static Entitas.IMatcher<MainEntity> EntityIndexComp {
        get {
            if (_matcherEntityIndexComp == null) {
                var matcher = (Entitas.Matcher<MainEntity>)Entitas.Matcher<MainEntity>.AllOf(MainComponentsLookup.EntityIndexComp);
                matcher.componentNames = MainComponentsLookup.componentNames;
                _matcherEntityIndexComp = matcher;
            }

            return _matcherEntityIndexComp;
        }
    }
}
