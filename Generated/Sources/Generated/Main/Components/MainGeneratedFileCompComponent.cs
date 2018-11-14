namespace GenEntitas {
public partial class MainEntity {

    public GenEntitas.GeneratedFileComp generatedFileComp { get { return (GenEntitas.GeneratedFileComp)GetComponent(MainComponentsLookup.GeneratedFileComp); } }
    public bool hasGeneratedFileComp { get { return HasComponent(MainComponentsLookup.GeneratedFileComp); } }

    public void AddGeneratedFileComp(string newFilePath, string newContents, string newGeneratedBy) {
        var index = MainComponentsLookup.GeneratedFileComp;
        var component = (GenEntitas.GeneratedFileComp)CreateComponent(index, typeof(GenEntitas.GeneratedFileComp));
        component.FilePath = newFilePath;
        component.Contents = newContents;
        component.GeneratedBy = newGeneratedBy;
        AddComponent(index, component);
    }

    public void ReplaceGeneratedFileComp(string newFilePath, string newContents, string newGeneratedBy) {
        var index = MainComponentsLookup.GeneratedFileComp;
        var component = (GenEntitas.GeneratedFileComp)CreateComponent(index, typeof(GenEntitas.GeneratedFileComp));
        component.FilePath = newFilePath;
        component.Contents = newContents;
        component.GeneratedBy = newGeneratedBy;
        ReplaceComponent(index, component);
    }

    public void RemoveGeneratedFileComp() {
        RemoveComponent(MainComponentsLookup.GeneratedFileComp);
    }
}

}

namespace GenEntitas {
public sealed partial class MainMatcher {

    static Entitas.IMatcher<MainEntity> _matcherGeneratedFileComp;

    public static Entitas.IMatcher<MainEntity> GeneratedFileComp {
        get {
            if (_matcherGeneratedFileComp == null) {
                var matcher = (Entitas.Matcher<MainEntity>)Entitas.Matcher<MainEntity>.AllOf(MainComponentsLookup.GeneratedFileComp);
                matcher.componentNames = MainComponentsLookup.componentNames;
                _matcherGeneratedFileComp = matcher;
            }

            return _matcherGeneratedFileComp;
        }
    }
}

}
