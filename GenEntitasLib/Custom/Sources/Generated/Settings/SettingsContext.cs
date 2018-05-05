public sealed partial class SettingsContext : Entitas.Context<SettingsEntity> {

    public SettingsContext()
        : base(
            SettingsComponentsLookup.TotalComponents,
            0,
            new Entitas.ContextInfo(
                "Settings",
                SettingsComponentsLookup.componentNames,
                SettingsComponentsLookup.componentTypes
            ),
            (entity) =>

#if (ENTITAS_FAST_AND_UNSAFE)
                new Entitas.UnsafeAERC()
#else
                new Entitas.SafeAERC(entity)
#endif

        ) {
    }
}
