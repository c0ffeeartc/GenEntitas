//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by GenEntitas.Sources.GenCompEntityApiSystem
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class MainEntity {

    static readonly DontGenerateComp dontGenerateCompComponent = new DontGenerateComp();

    public bool isDontGenerateComp {
        get { return HasComponent(MainComponentsLookup.DontGenerateComp); }
        set {
            if (value != isDontGenerateComp) {
                var index = MainComponentsLookup.DontGenerateComp;
                if (value) {
                    var componentPool = GetComponentPool(index);
                    var component = componentPool.Count > 0
                            ? componentPool.Pop()
                            : dontGenerateCompComponent;

                    AddComponent(index, component);
                } else {
                    RemoveComponent(index);
                }
            }
        }
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by GenEntitas.Sources.GenCompMatcherApiSystem
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public sealed partial class MainMatcher {

    static Entitas.IMatcher<MainEntity> _matcherDontGenerateComp;

    public static Entitas.IMatcher<MainEntity> DontGenerateComp {
        get {
            if (_matcherDontGenerateComp == null) {
                var matcher = (Entitas.Matcher<MainEntity>)Entitas.Matcher<MainEntity>.AllOf(MainComponentsLookup.DontGenerateComp);
                matcher.componentNames = MainComponentsLookup.componentNames;
                _matcherDontGenerateComp = matcher;
            }

            return _matcherDontGenerateComp;
        }
    }
}
