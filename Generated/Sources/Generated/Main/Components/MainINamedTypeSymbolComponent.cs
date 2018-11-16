namespace GenEntitas {
public partial class MainEntity {

    public GenEntitas.INamedTypeSymbolComponent iNamedTypeSymbol { get { return (GenEntitas.INamedTypeSymbolComponent)GetComponent(MainComponentsLookup.INamedTypeSymbol); } }
    public bool hasINamedTypeSymbol { get { return HasComponent(MainComponentsLookup.INamedTypeSymbol); } }

    public void AddINamedTypeSymbol(Microsoft.CodeAnalysis.INamedTypeSymbol newValue) {
        var index = MainComponentsLookup.INamedTypeSymbol;
        var component = (GenEntitas.INamedTypeSymbolComponent)CreateComponent(index, typeof(GenEntitas.INamedTypeSymbolComponent));
        component.Value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceINamedTypeSymbol(Microsoft.CodeAnalysis.INamedTypeSymbol newValue) {
        var index = MainComponentsLookup.INamedTypeSymbol;
        var component = (GenEntitas.INamedTypeSymbolComponent)CreateComponent(index, typeof(GenEntitas.INamedTypeSymbolComponent));
        component.Value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveINamedTypeSymbol() {
        RemoveComponent(MainComponentsLookup.INamedTypeSymbol);
    }
}

}

namespace GenEntitas {
public sealed partial class MainMatcher {

    static Entitas.IMatcher<MainEntity> _matcherINamedTypeSymbol;

    public static Entitas.IMatcher<MainEntity> INamedTypeSymbol {
        get {
            if (_matcherINamedTypeSymbol == null) {
                var matcher = (Entitas.Matcher<MainEntity>)Entitas.Matcher<MainEntity>.AllOf(MainComponentsLookup.INamedTypeSymbol);
                matcher.componentNames = MainComponentsLookup.componentNames;
                _matcherINamedTypeSymbol = matcher;
            }

            return _matcherINamedTypeSymbol;
        }
    }
}

}
