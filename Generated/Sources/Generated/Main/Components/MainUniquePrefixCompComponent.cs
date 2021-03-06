namespace GenEntitas {
public partial class MainEntity {

    public GenEntitas.UniquePrefixComp uniquePrefixComp { get { return (GenEntitas.UniquePrefixComp)GetComponent(MainComponentsLookup.UniquePrefixComp); } }
    public bool hasUniquePrefixComp { get { return HasComponent(MainComponentsLookup.UniquePrefixComp); } }

    public void AddUniquePrefixComp(string newValue) {
        var index = MainComponentsLookup.UniquePrefixComp;
        var component = (GenEntitas.UniquePrefixComp)CreateComponent(index, typeof(GenEntitas.UniquePrefixComp));
        component.Value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceUniquePrefixComp(string newValue) {
        var index = MainComponentsLookup.UniquePrefixComp;
        var component = (GenEntitas.UniquePrefixComp)CreateComponent(index, typeof(GenEntitas.UniquePrefixComp));
        component.Value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveUniquePrefixComp() {
        RemoveComponent(MainComponentsLookup.UniquePrefixComp);
    }
}

}

namespace GenEntitas {
public sealed partial class MainMatcher {

    static Entitas.IMatcher<MainEntity> _matcherUniquePrefixComp;

    public static Entitas.IMatcher<MainEntity> UniquePrefixComp {
        get {
            if (_matcherUniquePrefixComp == null) {
                var matcher = (Entitas.Matcher<MainEntity>)Entitas.Matcher<MainEntity>.AllOf(MainComponentsLookup.UniquePrefixComp);
                matcher.componentNames = MainComponentsLookup.componentNames;
                _matcherUniquePrefixComp = matcher;
            }

            return _matcherUniquePrefixComp;
        }
    }
}

}
