namespace GenEntitas {
public partial class MainEntity {

    public GenEntitas.NonIComp nonIComp { get { return (GenEntitas.NonIComp)GetComponent(MainComponentsLookup.NonIComp); } }
    public bool hasNonIComp { get { return HasComponent(MainComponentsLookup.NonIComp); } }

    public void AddNonIComp(string newFullCompName, string newFieldTypeName) {
        var index = MainComponentsLookup.NonIComp;
        var component = (GenEntitas.NonIComp)CreateComponent(index, typeof(GenEntitas.NonIComp));
        component.FullCompName = newFullCompName;
        component.FieldTypeName = newFieldTypeName;
        AddComponent(index, component);
    }

    public void ReplaceNonIComp(string newFullCompName, string newFieldTypeName) {
        var index = MainComponentsLookup.NonIComp;
        var component = (GenEntitas.NonIComp)CreateComponent(index, typeof(GenEntitas.NonIComp));
        component.FullCompName = newFullCompName;
        component.FieldTypeName = newFieldTypeName;
        ReplaceComponent(index, component);
    }

    public void RemoveNonIComp() {
        RemoveComponent(MainComponentsLookup.NonIComp);
    }
}

}

namespace GenEntitas {
public sealed partial class MainMatcher {

    static Entitas.IMatcher<MainEntity> _matcherNonIComp;

    public static Entitas.IMatcher<MainEntity> NonIComp {
        get {
            if (_matcherNonIComp == null) {
                var matcher = (Entitas.Matcher<MainEntity>)Entitas.Matcher<MainEntity>.AllOf(MainComponentsLookup.NonIComp);
                matcher.componentNames = MainComponentsLookup.componentNames;
                _matcherNonIComp = matcher;
            }

            return _matcherNonIComp;
        }
    }
}

}
