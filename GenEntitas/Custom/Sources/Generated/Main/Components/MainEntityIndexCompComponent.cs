//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class MainEntity {

    public EntityIndexComp entityIndexComp { get { return (EntityIndexComp)GetComponent(MainComponentsLookup.EntityIndexComp); } }
    public bool hasEntityIndexComp { get { return HasComponent(MainComponentsLookup.EntityIndexComp); } }

    public void AddEntityIndexComp(System.Collections.Generic.List<EntityIndexInfo> newValues) {
        var index = MainComponentsLookup.EntityIndexComp;
        var component = CreateComponent<EntityIndexComp>(index);
        component.Values = newValues;
        AddComponent(index, component);
    }

    public void ReplaceEntityIndexComp(System.Collections.Generic.List<EntityIndexInfo> newValues) {
        var index = MainComponentsLookup.EntityIndexComp;
        var component = CreateComponent<EntityIndexComp>(index);
        component.Values = newValues;
        ReplaceComponent(index, component);
    }

    public void RemoveEntityIndexComp() {
        RemoveComponent(MainComponentsLookup.EntityIndexComp);
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

    static Entitas.IMatcher<MainEntity> _matcherEntityIndexComp;

    public static Entitas.IMatcher<MainEntity> EntityIndexComp {
        get {
            if (_matcherEntityIndexComp == null) {
                var matcher = (Entitas.Matcher<MainEntity>)Entitas.Matcher<MainEntity>.AllOf(MainComponentsLookup.EntityIndexComp);
                matcher.componentNames = MainComponentsLookup.componentNames;
                _matcherEntityIndexComp = matcher;
            }

            return _matcherEntityIndexComp;
        }
    }
}