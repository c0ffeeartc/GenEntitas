public partial class MainEntity {

    public Comp comp { get { return (Comp)GetComponent(MainComponentsLookup.Comp); } }
    public bool hasComp { get { return HasComponent(MainComponentsLookup.Comp); } }

    public void AddComp(string newName, string newFullTypeName) {
        var index = MainComponentsLookup.Comp;
        var component = CreateComponent<Comp>(index);
        component.Name = newName;
        component.FullTypeName = newFullTypeName;
        AddComponent(index, component);
    }

    public void ReplaceComp(string newName, string newFullTypeName) {
        var index = MainComponentsLookup.Comp;
        var component = CreateComponent<Comp>(index);
        component.Name = newName;
        component.FullTypeName = newFullTypeName;
        ReplaceComponent(index, component);
    }

    public void RemoveComp() {
        RemoveComponent(MainComponentsLookup.Comp);
    }
}

public sealed partial class MainMatcher {

    static Entitas.IMatcher<MainEntity> _matcherComp;

    public static Entitas.IMatcher<MainEntity> Comp {
        get {
            if (_matcherComp == null) {
                var matcher = (Entitas.Matcher<MainEntity>)Entitas.Matcher<MainEntity>.AllOf(MainComponentsLookup.Comp);
                matcher.componentNames = MainComponentsLookup.componentNames;
                _matcherComp = matcher;
            }

            return _matcherComp;
        }
    }
}
