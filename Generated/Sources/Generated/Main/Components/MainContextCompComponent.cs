namespace GenEntitas {
public partial class MainEntity {

    public GenEntitas.ContextComp contextComp { get { return (GenEntitas.ContextComp)GetComponent(MainComponentsLookup.ContextComp); } }
    public bool hasContextComp { get { return HasComponent(MainComponentsLookup.ContextComp); } }

    public void AddContextComp(string newName) {
        var index = MainComponentsLookup.ContextComp;
        var component = (GenEntitas.ContextComp)CreateComponent(index, typeof(GenEntitas.ContextComp));
        component.Name = newName;
        AddComponent(index, component);
    }

    public void ReplaceContextComp(string newName) {
        var index = MainComponentsLookup.ContextComp;
        var component = (GenEntitas.ContextComp)CreateComponent(index, typeof(GenEntitas.ContextComp));
        component.Name = newName;
        ReplaceComponent(index, component);
    }

    public void RemoveContextComp() {
        RemoveComponent(MainComponentsLookup.ContextComp);
    }
}

}

namespace GenEntitas {
public sealed partial class MainMatcher {

    static Entitas.IMatcher<MainEntity> _matcherContextComp;

    public static Entitas.IMatcher<MainEntity> ContextComp {
        get {
            if (_matcherContextComp == null) {
                var matcher = (Entitas.Matcher<MainEntity>)Entitas.Matcher<MainEntity>.AllOf(MainComponentsLookup.ContextComp);
                matcher.componentNames = MainComponentsLookup.componentNames;
                _matcherContextComp = matcher;
            }

            return _matcherContextComp;
        }
    }
}

}
