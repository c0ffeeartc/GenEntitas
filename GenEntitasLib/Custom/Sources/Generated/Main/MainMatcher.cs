namespace GenEntitas {
public sealed partial class MainMatcher {

    public static Entitas.IAllOfMatcher<MainEntity> AllOf(params int[] indices) {
        return Entitas.Matcher<MainEntity>.AllOf(indices);
    }

    public static Entitas.IAllOfMatcher<MainEntity> AllOf(params Entitas.IMatcher<MainEntity>[] matchers) {
          return Entitas.Matcher<MainEntity>.AllOf(matchers);
    }

    public static Entitas.IAnyOfMatcher<MainEntity> AnyOf(params int[] indices) {
          return Entitas.Matcher<MainEntity>.AnyOf(indices);
    }

    public static Entitas.IAnyOfMatcher<MainEntity> AnyOf(params Entitas.IMatcher<MainEntity>[] matchers) {
          return Entitas.Matcher<MainEntity>.AnyOf(matchers);
    }
}

}
