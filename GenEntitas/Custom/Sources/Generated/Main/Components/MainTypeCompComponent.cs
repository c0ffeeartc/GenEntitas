//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class MainEntity {

    public TypeComp typeComp { get { return (TypeComp)GetComponent(MainComponentsLookup.TypeComp); } }
    public bool hasTypeComp { get { return HasComponent(MainComponentsLookup.TypeComp); } }

    public void AddTypeComp(System.Type newValue) {
        var index = MainComponentsLookup.TypeComp;
        var component = CreateComponent<TypeComp>(index);
        component.Value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceTypeComp(System.Type newValue) {
        var index = MainComponentsLookup.TypeComp;
        var component = CreateComponent<TypeComp>(index);
        component.Value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveTypeComp() {
        RemoveComponent(MainComponentsLookup.TypeComp);
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentMatcherApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public sealed partial class MainMatcher {

    static Entitas.IMatcher<MainEntity> _matcherTypeComp;

    public static Entitas.IMatcher<MainEntity> TypeComp {
        get {
            if (_matcherTypeComp == null) {
                var matcher = (Entitas.Matcher<MainEntity>)Entitas.Matcher<MainEntity>.AllOf(MainComponentsLookup.TypeComp);
                matcher.componentNames = MainComponentsLookup.componentNames;
                _matcherTypeComp = matcher;
            }

            return _matcherTypeComp;
        }
    }
}
