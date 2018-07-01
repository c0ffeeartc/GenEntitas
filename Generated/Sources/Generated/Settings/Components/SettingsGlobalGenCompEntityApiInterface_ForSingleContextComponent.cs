namespace GenEntitas {
public partial class SettingsContext {

    public SettingsEntity globalGenCompEntityApiInterface_ForSingleContextEntity { get { return GetGroup(SettingsMatcher.GlobalGenCompEntityApiInterface_ForSingleContext).GetSingleEntity(); } }

    public bool isGlobalGenCompEntityApiInterface_ForSingleContext {
        get { return globalGenCompEntityApiInterface_ForSingleContextEntity != null; }
        set {
            var entity = globalGenCompEntityApiInterface_ForSingleContextEntity;
            if (value != (entity != null)) {
                if (value) {
                    CreateEntity().isGlobalGenCompEntityApiInterface_ForSingleContext = true;
                } else {
                    entity.Destroy();
                }
            }
        }
    }
}

}

namespace GenEntitas {
public partial class SettingsEntity {

    static readonly GenEntitas.GlobalGenCompEntityApiInterface_ForSingleContext globalGenCompEntityApiInterface_ForSingleContextComponent = new GenEntitas.GlobalGenCompEntityApiInterface_ForSingleContext();

    public bool isGlobalGenCompEntityApiInterface_ForSingleContext {
        get { return HasComponent(SettingsComponentsLookup.GlobalGenCompEntityApiInterface_ForSingleContext); }
        set {
            if (value != isGlobalGenCompEntityApiInterface_ForSingleContext) {
                var index = SettingsComponentsLookup.GlobalGenCompEntityApiInterface_ForSingleContext;
                if (value) {
                    var componentPool = GetComponentPool(index);
                    var component = componentPool.Count > 0
                            ? componentPool.Pop()
                            : globalGenCompEntityApiInterface_ForSingleContextComponent;

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
public sealed partial class SettingsMatcher {

    static Entitas.IMatcher<SettingsEntity> _matcherGlobalGenCompEntityApiInterface_ForSingleContext;

    public static Entitas.IMatcher<SettingsEntity> GlobalGenCompEntityApiInterface_ForSingleContext {
        get {
            if (_matcherGlobalGenCompEntityApiInterface_ForSingleContext == null) {
                var matcher = (Entitas.Matcher<SettingsEntity>)Entitas.Matcher<SettingsEntity>.AllOf(SettingsComponentsLookup.GlobalGenCompEntityApiInterface_ForSingleContext);
                matcher.componentNames = SettingsComponentsLookup.componentNames;
                _matcherGlobalGenCompEntityApiInterface_ForSingleContext = matcher;
            }

            return _matcherGlobalGenCompEntityApiInterface_ForSingleContext;
        }
    }
}

}
