namespace GenEntitas {
public partial class MainEntity {

    public GenEntitas.EventComp eventComp { get { return (GenEntitas.EventComp)GetComponent(MainComponentsLookup.EventComp); } }
    public bool hasEventComp { get { return HasComponent(MainComponentsLookup.EventComp); } }

    public void AddEventComp(System.Collections.Generic.List<GenEntitas.EventInfo> newValues) {
        var index = MainComponentsLookup.EventComp;
        var component = CreateComponent<GenEntitas.EventComp>(index);
        component.Values = newValues;
        AddComponent(index, component);
    }

    public void ReplaceEventComp(System.Collections.Generic.List<GenEntitas.EventInfo> newValues) {
        var index = MainComponentsLookup.EventComp;
        var component = CreateComponent<GenEntitas.EventComp>(index);
        component.Values = newValues;
        ReplaceComponent(index, component);
    }

    public void RemoveEventComp() {
        RemoveComponent(MainComponentsLookup.EventComp);
    }
}

}

namespace GenEntitas {
public sealed partial class MainMatcher {

    static Entitas.IMatcher<MainEntity> _matcherEventComp;

    public static Entitas.IMatcher<MainEntity> EventComp {
        get {
            if (_matcherEventComp == null) {
                var matcher = (Entitas.Matcher<MainEntity>)Entitas.Matcher<MainEntity>.AllOf(MainComponentsLookup.EventComp);
                matcher.componentNames = MainComponentsLookup.componentNames;
                _matcherEventComp = matcher;
            }

            return _matcherEventComp;
        }
    }
}

}
