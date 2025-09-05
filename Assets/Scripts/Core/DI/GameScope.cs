using VContainer;
using VContainer.Unity;

public class GameScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<ScoreService>(Lifetime.Singleton).As<IScoreService>();
        builder.RegisterEntryPoint<CoinGenerator>();
    }
}
