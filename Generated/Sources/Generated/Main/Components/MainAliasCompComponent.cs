namespace GenEntitas {
public partial class MainContext {

    public MainEntity aliasCompEntity { get { return GetGroup(MainMatcher.AliasComp).GetSingleEntity(); } }
    public GenEntitas.AliasComp aliasComp { get { return aliasCompEntity.aliasComp; } }
    public bool hasAliasComp { get { return aliasCompEntity != null; } }

    public MainEntity SetAliasComp(System.Collections.Generic.Dictionary<string, string> newValues) {
        if (hasAliasComp) {
            throw new Entitas.EntitasException("Could not set AliasComp!\n" + this + " already has an entity with GenEntitas.AliasComp!",
                "You should check if the context already has a aliasCompEntity before setting it or use context.ReplaceAliasComp().");
        }
        var entity = CreateEntity();
        entity.AddAliasComp(newValues);
        return entity;
    }

    public void ReplaceAliasComp(System.Collections.Generic.Dictionary<string, string> newValues) {
        var entity = aliasCompEntity;
        if (entity == null) {
            entity = SetAliasComp(newValues);
        } else {
            entity.ReplaceAliasComp(newValues);
        }
    }

    public void RemoveAliasComp() {
        aliasCompEntity.Destroy();
    }
}

}

namespace GenEntitas {
public partial class MainEntity {

    public GenEntitas.AliasComp aliasComp { get { return (GenEntitas.AliasComp)GetComponent(MainComponentsLookup.AliasComp); } }
    public bool hasAliasComp { get { return HasComponent(MainComponentsLookup.AliasComp); } }

    public void AddAliasComp(System.Collections.Generic.Dictionary<string, string> newValues) {
        var index = MainComponentsLookup.AliasComp;
        var component = CreateComponent<GenEntitas.AliasComp>(index);
        component.Values = newValues;
        AddComponent(index, component);
    }

    public void ReplaceAliasComp(System.Collections.Generic.Dictionary<string, string> newValues) {
        var index = MainComponentsLookup.AliasComp;
        var component = CreateComponent<GenEntitas.AliasComp>(index);
        component.Values = newValues;
        ReplaceComponent(index, component);
    }

    public void RemoveAliasComp() {
        RemoveComponent(MainComponentsLookup.AliasComp);
    }
}

}

namespace GenEntitas {
public sealed partial class MainMatcher {

    static Entitas.IMatcher<MainEntity> _matcherAliasComp;

    public static Entitas.IMatcher<MainEntity> AliasComp {
        get {
            if (_matcherAliasComp == null) {
                var matcher = (Entitas.Matcher<MainEntity>)Entitas.Matcher<MainEntity>.AllOf(MainComponentsLookup.AliasComp);
                matcher.componentNames = MainComponentsLookup.componentNames;
                _matcherAliasComp = matcher;
            }

            return _matcherAliasComp;
        }
    }
}

}
