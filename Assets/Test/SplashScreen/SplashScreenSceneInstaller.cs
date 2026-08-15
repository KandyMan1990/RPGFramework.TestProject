using RPGFramework.DI;
using Test.SplashScreen;

public class SplashScreenSceneInstaller : SceneInstallerBase
{
    public override void InstallBindings(IDIContainer container)
    {
        SplashScreenModuleMonoBehaviour screenModuleMonoBehaviour = FindAnyObjectByType<SplashScreenModuleMonoBehaviour>();

        container.BindSingletonFromInstance<ISplashScreenModuleMonoBehaviour>(screenModuleMonoBehaviour);
        container.BindSingleton<ISplashScreenModule, SplashScreenModule>();
    }
}