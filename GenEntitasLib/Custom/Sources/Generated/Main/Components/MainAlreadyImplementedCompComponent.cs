public partial class MainEntity {

    static readonly GenEntitas.AlreadyImplementedComp alreadyImplementedCompComponent = new GenEntitas.AlreadyImplementedComp();

    public bool isAlreadyImplementedComp {
        get { return HasComponent(MainComponentsLookup.AlreadyImplementedComp); }
        set {
            if (value != isAlreadyImplementedComp) {
                var index = MainComponentsLookup.AlreadyImplementedComp;
                if (value) {
                    var componentPool = GetComponentPool(index);
                    var component = componentPool.Count > 0
                            ? componentPool.Pop()
                            : alreadyImplementedCompComponent;

                    AddComponent(index, component);
                } else {
                    RemoveComponent(index);
                }
            }
        }
    }
}

public sealed partial class MainMatcher {

    static Entitas.IMatcher<MainEntity> _matcherAlreadyImplementedComp;

    public static Entitas.IMatcher<MainEntity> AlreadyImplementedComp {
        get {
            if (_matcherAlreadyImplementedComp == null) {
                var matcher = (Entitas.Matcher<MainEntity>)Entitas.Matcher<MainEntity>.AllOf(MainComponentsLookup.AlreadyImplementedComp);
                matcher.componentNames = MainComponentsLookup.componentNames;
                _matcherAlreadyImplementedComp = matcher;
            }

            return _matcherAlreadyImplementedComp;
        }
    }
}
