namespace GenEntitas {
public partial class MainEntity {

    static readonly GenEntitas.ParsedByGenEntitasLang parsedByGenEntitasLangComponent = new GenEntitas.ParsedByGenEntitasLang();

    public bool isParsedByGenEntitasLang {
        get { return HasComponent(MainComponentsLookup.ParsedByGenEntitasLang); }
        set {
            if (value != isParsedByGenEntitasLang) {
                var index = MainComponentsLookup.ParsedByGenEntitasLang;
                if (value) {
                    var componentPool = GetComponentPool(index);
                    var component = componentPool.Count > 0
                            ? componentPool.Pop()
                            : parsedByGenEntitasLangComponent;

                    AddComponent(index, component);
                } else {
                    RemoveComponent(index);
                }
            }
        }
    }
}

}

namespace GenEntitas {
public sealed partial class MainMatcher {

    static Entitas.IMatcher<MainEntity> _matcherParsedByGenEntitasLang;

    public static Entitas.IMatcher<MainEntity> ParsedByGenEntitasLang {
        get {
            if (_matcherParsedByGenEntitasLang == null) {
                var matcher = (Entitas.Matcher<MainEntity>)Entitas.Matcher<MainEntity>.AllOf(MainComponentsLookup.ParsedByGenEntitasLang);
                matcher.componentNames = MainComponentsLookup.componentNames;
                _matcherParsedByGenEntitasLang = matcher;
            }

            return _matcherParsedByGenEntitasLang;
        }
    }
}

}
