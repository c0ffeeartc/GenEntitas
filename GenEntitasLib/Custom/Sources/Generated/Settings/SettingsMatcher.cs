public sealed partial class SettingsMatcher {

    public static Entitas.IAllOfMatcher<SettingsEntity> AllOf(params int[] indices) {
        return Entitas.Matcher<SettingsEntity>.AllOf(indices);
    }

    public static Entitas.IAllOfMatcher<SettingsEntity> AllOf(params Entitas.IMatcher<SettingsEntity>[] matchers) {
          return Entitas.Matcher<SettingsEntity>.AllOf(matchers);
    }

    public static Entitas.IAnyOfMatcher<SettingsEntity> AnyOf(params int[] indices) {
          return Entitas.Matcher<SettingsEntity>.AnyOf(indices);
    }

    public static Entitas.IAnyOfMatcher<SettingsEntity> AnyOf(params Entitas.IMatcher<SettingsEntity>[] matchers) {
          return Entitas.Matcher<SettingsEntity>.AnyOf(matchers);
    }
}
