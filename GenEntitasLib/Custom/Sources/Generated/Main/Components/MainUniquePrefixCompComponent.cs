public partial class MainEntity {

    public UniquePrefixComp uniquePrefixComp { get { return (UniquePrefixComp)GetComponent(MainComponentsLookup.UniquePrefixComp); } }
    public bool hasUniquePrefixComp { get { return HasComponent(MainComponentsLookup.UniquePrefixComp); } }

    public void AddUniquePrefixComp(string newValue) {
        var index = MainComponentsLookup.UniquePrefixComp;
        var component = CreateComponent<UniquePrefixComp>(index);
        component.Value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceUniquePrefixComp(string newValue) {
        var index = MainComponentsLookup.UniquePrefixComp;
        var component = CreateComponent<UniquePrefixComp>(index);
        component.Value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveUniquePrefixComp() {
        RemoveComponent(MainComponentsLookup.UniquePrefixComp);
    }
}

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
