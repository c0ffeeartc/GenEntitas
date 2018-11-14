namespace GenEntitas {
public sealed partial class MainContext : Entitas.Context<MainEntity> {

    public MainContext()
        : base(
            MainComponentsLookup.TotalComponents,
            0,
            new Entitas.ContextInfo(
                "Main",
                MainComponentsLookup.componentNames,
                MainComponentsLookup.componentTypes
            ),
            (entity) =>

#if (ENTITAS_FAST_AND_UNSAFE)
                new Entitas.UnsafeAERC(),
#else
                new Entitas.SafeAERC(entity),
#endif
            () => new MainEntity()
        ) {
    }
}

}
