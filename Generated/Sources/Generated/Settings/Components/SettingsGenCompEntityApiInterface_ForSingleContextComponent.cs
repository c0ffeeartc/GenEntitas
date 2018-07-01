namespace GenEntitas {
public partial class SettingsContext {

    public SettingsEntity genCompEntityApiInterface_ForSingleContextEntity { get { return GetGroup(SettingsMatcher.GenCompEntityApiInterface_ForSingleContext).GetSingleEntity(); } }

    public bool isGenCompEntityApiInterface_ForSingleContext {
        get { return genCompEntityApiInterface_ForSingleContextEntity != null; }
        set {
            var entity = genCompEntityApiInterface_ForSingleContextEntity;
            if (value != (entity != null)) {
                if (value) {
                    CreateEntity().isGenCompEntityApiInterface_ForSingleContext = true;
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

    static readonly GenEntitas.GenCompEntityApiInterface_ForSingleContext genCompEntityApiInterface_ForSingleContextComponent = new GenEntitas.GenCompEntityApiInterface_ForSingleContext();

    public bool isGenCompEntityApiInterface_ForSingleContext {
        get { return HasComponent(SettingsComponentsLookup.GenCompEntityApiInterface_ForSingleContext); }
        set {
            if (value != isGenCompEntityApiInterface_ForSingleContext) {
                var index = SettingsComponentsLookup.GenCompEntityApiInterface_ForSingleContext;
                if (value) {
                    var componentPool = GetComponentPool(index);
                    var component = componentPool.Count > 0
                            ? componentPool.Pop()
                            : genCompEntityApiInterface_ForSingleContextComponent;

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

    static Entitas.IMatcher<SettingsEntity> _matcherGenCompEntityApiInterface_ForSingleContext;

    public static Entitas.IMatcher<SettingsEntity> GenCompEntityApiInterface_ForSingleContext {
        get {
            if (_matcherGenCompEntityApiInterface_ForSingleContext == null) {
                var matcher = (Entitas.Matcher<SettingsEntity>)Entitas.Matcher<SettingsEntity>.AllOf(SettingsComponentsLookup.GenCompEntityApiInterface_ForSingleContext);
                matcher.componentNames = SettingsComponentsLookup.componentNames;
                _matcherGenCompEntityApiInterface_ForSingleContext = matcher;
            }

            return _matcherGenCompEntityApiInterface_ForSingleContext;
        }
    }
}

}
