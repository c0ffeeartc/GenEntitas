namespace GenEntitas {
public partial class MainEntity {

    public GenEntitas.GenEntitasLangInputString genEntitasLangInputString { get { return (GenEntitas.GenEntitasLangInputString)GetComponent(MainComponentsLookup.GenEntitasLangInputString); } }
    public bool hasGenEntitasLangInputString { get { return HasComponent(MainComponentsLookup.GenEntitasLangInputString); } }

    public void AddGenEntitasLangInputString(string newValue) {
        var index = MainComponentsLookup.GenEntitasLangInputString;
        var component = (GenEntitas.GenEntitasLangInputString)CreateComponent(index, typeof(GenEntitas.GenEntitasLangInputString));
        component.Value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceGenEntitasLangInputString(string newValue) {
        var index = MainComponentsLookup.GenEntitasLangInputString;
        var component = (GenEntitas.GenEntitasLangInputString)CreateComponent(index, typeof(GenEntitas.GenEntitasLangInputString));
        component.Value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveGenEntitasLangInputString() {
        RemoveComponent(MainComponentsLookup.GenEntitasLangInputString);
    }
}

}

namespace GenEntitas {
public sealed partial class MainMatcher {

    static Entitas.IMatcher<MainEntity> _matcherGenEntitasLangInputString;

    public static Entitas.IMatcher<MainEntity> GenEntitasLangInputString {
        get {
            if (_matcherGenEntitasLangInputString == null) {
                var matcher = (Entitas.Matcher<MainEntity>)Entitas.Matcher<MainEntity>.AllOf(MainComponentsLookup.GenEntitasLangInputString);
                matcher.componentNames = MainComponentsLookup.componentNames;
                _matcherGenEntitasLangInputString = matcher;
            }

            return _matcherGenEntitasLangInputString;
        }
    }
}

}
