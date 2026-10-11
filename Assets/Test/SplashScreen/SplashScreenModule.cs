using System.Threading.Tasks;
using RPGFramework.Core;
using RPGFramework.Core.SharedTypes;
using Test.SplashScreen.Constants;

namespace Test.SplashScreen
{
    public interface ISplashScreenModule : IModule
    {
    }

    internal sealed class SplashScreenModule : ISplashScreenModule
    {
        private readonly ICoreModule                      m_CoreModule;
        private readonly ISplashScreenModuleMonoBehaviour m_SplashScreenModuleMonoBehaviour;

        internal SplashScreenModule(ICoreModule                      coreModule,
                                    ISplashScreenModuleMonoBehaviour splashScreenModuleMonoBehaviour)
        {
            m_CoreModule                      = coreModule;
            m_SplashScreenModuleMonoBehaviour = splashScreenModuleMonoBehaviour;
        }

        Task IModule.OnEnterAsync(byte entry)
        {
            ShowSplashScreenAsync().FireAndForget();

            return Task.CompletedTask;
        }

        Task IModule.OnExitAsync()
        {
            return Task.CompletedTask;
        }

        Task IModule.OnSuspendAsync()
        {
            return Task.CompletedTask;
        }

        Task IModule.OnResumeAsync()
        {
            return Task.CompletedTask;
        }

        private async Task ShowSplashScreenAsync()
        {
            await m_SplashScreenModuleMonoBehaviour.ShowSplashScreenAsync();

            m_CoreModule.RequestModuleChangeAsync(SplashScreenConstants.OUTCOME_FINISHED).FireAndForget();
        }
    }
}