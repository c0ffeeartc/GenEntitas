public partial class MainEntity {

    static readonly UniqueComp uniqueCompComponent = new UniqueComp();

    public bool isUniqueComp {
        get { return HasComponent(MainComponentsLookup.UniqueComp); }
        set {
            if (value != isUniqueComp) {
                var index = MainComponentsLookup.UniqueComp;
                if (value) {
                    var componentPool = GetComponentPool(index);
                    var component = componentPool.Count > 0
                            ? componentPool.Pop()
                            : uniqueCompComponent;

                    AddComponent(index, component);
                } else {
                    RemoveComponent(index);
                }
            }
        }
    }
}

public sealed partial class MainMatcher {

    static Entitas.IMatcher<MainEntity> _matcherUniqueComp;

    public static Entitas.IMatcher<MainEntity> UniqueComp {
        get {
            if (_matcherUniqueComp == null) {
                var matcher = (Entitas.Matcher<MainEntity>)Entitas.Matcher<MainEntity>.AllOf(MainComponentsLookup.UniqueComp);
                matcher.componentNames = MainComponentsLookup.componentNames;
                _matcherUniqueComp = matcher;
            }

            return _matcherUniqueComp;
        }
    }
}
