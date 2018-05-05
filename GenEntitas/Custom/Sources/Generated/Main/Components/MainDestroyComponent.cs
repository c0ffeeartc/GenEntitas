public partial class MainEntity {

    static readonly Destroy destroyComponent = new Destroy();

    public bool isDestroy {
        get { return HasComponent(MainComponentsLookup.Destroy); }
        set {
            if (value != isDestroy) {
                var index = MainComponentsLookup.Destroy;
                if (value) {
                    var componentPool = GetComponentPool(index);
                    var component = componentPool.Count > 0
                            ? componentPool.Pop()
                            : destroyComponent;

                    AddComponent(index, component);
                } else {
                    RemoveComponent(index);
                }
            }
        }
    }
}

public sealed partial class MainMatcher {

    static Entitas.IMatcher<MainEntity> _matcherDestroy;

    public static Entitas.IMatcher<MainEntity> Destroy {
        get {
            if (_matcherDestroy == null) {
                var matcher = (Entitas.Matcher<MainEntity>)Entitas.Matcher<MainEntity>.AllOf(MainComponentsLookup.Destroy);
                matcher.componentNames = MainComponentsLookup.componentNames;
                _matcherDestroy = matcher;
            }

            return _matcherDestroy;
        }
    }
}
