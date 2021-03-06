namespace GenEntitas {
public partial class MainEntity {

    public GenEntitas.ContextNamesComp contextNamesComp { get { return (GenEntitas.ContextNamesComp)GetComponent(MainComponentsLookup.ContextNamesComp); } }
    public bool hasContextNamesComp { get { return HasComponent(MainComponentsLookup.ContextNamesComp); } }

    public void AddContextNamesComp(System.Collections.Generic.List<string> newValues) {
        var index = MainComponentsLookup.ContextNamesComp;
        var component = (GenEntitas.ContextNamesComp)CreateComponent(index, typeof(GenEntitas.ContextNamesComp));
        component.Values = newValues;
        AddComponent(index, component);
    }

    public void ReplaceContextNamesComp(System.Collections.Generic.List<string> newValues) {
        var index = MainComponentsLookup.ContextNamesComp;
        var component = (GenEntitas.ContextNamesComp)CreateComponent(index, typeof(GenEntitas.ContextNamesComp));
        component.Values = newValues;
        ReplaceComponent(index, component);
    }

    public void RemoveContextNamesComp() {
        RemoveComponent(MainComponentsLookup.ContextNamesComp);
    }
}

}

namespace GenEntitas {
public sealed partial class MainMatcher {

    static Entitas.IMatcher<MainEntity> _matcherContextNamesComp;

    public static Entitas.IMatcher<MainEntity> ContextNamesComp {
        get {
            if (_matcherContextNamesComp == null) {
                var matcher = (Entitas.Matcher<MainEntity>)Entitas.Matcher<MainEntity>.AllOf(MainComponentsLookup.ContextNamesComp);
                matcher.componentNames = MainComponentsLookup.componentNames;
                _matcherContextNamesComp = matcher;
            }

            return _matcherContextNamesComp;
        }
    }
}

}
