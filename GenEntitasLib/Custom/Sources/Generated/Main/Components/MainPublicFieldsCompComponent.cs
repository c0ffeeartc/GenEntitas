public partial class MainEntity {

    public PublicFieldsComp publicFieldsComp { get { return (PublicFieldsComp)GetComponent(MainComponentsLookup.PublicFieldsComp); } }
    public bool hasPublicFieldsComp { get { return HasComponent(MainComponentsLookup.PublicFieldsComp); } }

    public void AddPublicFieldsComp(System.Collections.Generic.List<FieldInfo> newValues) {
        var index = MainComponentsLookup.PublicFieldsComp;
        var component = CreateComponent<PublicFieldsComp>(index);
        component.Values = newValues;
        AddComponent(index, component);
    }

    public void ReplacePublicFieldsComp(System.Collections.Generic.List<FieldInfo> newValues) {
        var index = MainComponentsLookup.PublicFieldsComp;
        var component = CreateComponent<PublicFieldsComp>(index);
        component.Values = newValues;
        ReplaceComponent(index, component);
    }

    public void RemovePublicFieldsComp() {
        RemoveComponent(MainComponentsLookup.PublicFieldsComp);
    }
}

public sealed partial class MainMatcher {

    static Entitas.IMatcher<MainEntity> _matcherPublicFieldsComp;

    public static Entitas.IMatcher<MainEntity> PublicFieldsComp {
        get {
            if (_matcherPublicFieldsComp == null) {
                var matcher = (Entitas.Matcher<MainEntity>)Entitas.Matcher<MainEntity>.AllOf(MainComponentsLookup.PublicFieldsComp);
                matcher.componentNames = MainComponentsLookup.componentNames;
                _matcherPublicFieldsComp = matcher;
            }

            return _matcherPublicFieldsComp;
        }
    }
}