namespace GenEntitas {
public partial class MainEntity {

    static readonly GenEntitas.GenCompEntApiInterface_ForSingleContext genCompEntApiInterface_ForSingleContextComponent = new GenEntitas.GenCompEntApiInterface_ForSingleContext();

    public bool isGenCompEntApiInterface_ForSingleContext {
        get { return HasComponent(MainComponentsLookup.GenCompEntApiInterface_ForSingleContext); }
        set {
            if (value != isGenCompEntApiInterface_ForSingleContext) {
                var index = MainComponentsLookup.GenCompEntApiInterface_ForSingleContext;
                if (value) {
                    var componentPool = GetComponentPool(index);
                    var component = componentPool.Count > 0
                            ? componentPool.Pop()
                            : genCompEntApiInterface_ForSingleContextComponent;

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

    static Entitas.IMatcher<MainEntity> _matcherGenCompEntApiInterface_ForSingleContext;

    public static Entitas.IMatcher<MainEntity> GenCompEntApiInterface_ForSingleContext {
        get {
            if (_matcherGenCompEntApiInterface_ForSingleContext == null) {
                var matcher = (Entitas.Matcher<MainEntity>)Entitas.Matcher<MainEntity>.AllOf(MainComponentsLookup.GenCompEntApiInterface_ForSingleContext);
                matcher.componentNames = MainComponentsLookup.componentNames;
                _matcherGenCompEntApiInterface_ForSingleContext = matcher;
            }

            return _matcherGenCompEntApiInterface_ForSingleContext;
        }
    }
}

}
