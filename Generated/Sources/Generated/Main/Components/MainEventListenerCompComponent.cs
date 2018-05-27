namespace GenEntitas {
public partial class MainEntity {

    static readonly GenEntitas.EventListenerComp eventListenerCompComponent = new GenEntitas.EventListenerComp();

    public bool isEventListenerComp {
        get { return HasComponent(MainComponentsLookup.EventListenerComp); }
        set {
            if (value != isEventListenerComp) {
                var index = MainComponentsLookup.EventListenerComp;
                if (value) {
                    var componentPool = GetComponentPool(index);
                    var component = componentPool.Count > 0
                            ? componentPool.Pop()
                            : eventListenerCompComponent;

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

    static Entitas.IMatcher<MainEntity> _matcherEventListenerComp;

    public static Entitas.IMatcher<MainEntity> EventListenerComp {
        get {
            if (_matcherEventListenerComp == null) {
                var matcher = (Entitas.Matcher<MainEntity>)Entitas.Matcher<MainEntity>.AllOf(MainComponentsLookup.EventListenerComp);
                matcher.componentNames = MainComponentsLookup.componentNames;
                _matcherEventListenerComp = matcher;
            }

            return _matcherEventListenerComp;
        }
    }
}

}
