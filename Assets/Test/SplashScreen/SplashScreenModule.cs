using System.Threading.Tasks;
using RPGFramework.Core;
using RPGFramework.Core.SaveData;
using RPGFramework.Core.SharedTypes;
using RPGFramework.Core.Store;
using RPGFramework.Field.SharedTypes.Providers;
using RPGFramework.Menu.SharedTypes.Constants;

namespace Test.SplashScreen
{
    public interface ISplashScreenModule : IModule
    {
    }

    internal sealed class SplashScreenModule : ISplashScreenModule
    {
        private readonly ICoreModule                      m_CoreModule;
        private readonly IChangeModuleStore               m_ChangeModuleStore;
        private readonly ISplashScreenModuleMonoBehaviour m_SplashScreenModuleMonoBehaviour;
        private readonly ISaveDataService                 m_SaveDataService;
        private readonly IFieldArgsProvider               m_FieldArgsProvider;

        internal SplashScreenModule(ICoreModule                      coreModule,
                                    IChangeModuleStore               changeModuleStore,
                                    ISplashScreenModuleMonoBehaviour splashScreenModuleMonoBehaviour,
                                    ISaveDataService                 saveDataService,
                                    IFieldArgsProvider               fieldArgsProvider)
        {
            m_CoreModule                      = coreModule;
            m_ChangeModuleStore               = changeModuleStore;
            m_SplashScreenModuleMonoBehaviour = splashScreenModuleMonoBehaviour;
            m_SaveDataService                 = saveDataService;
            m_FieldArgsProvider               = fieldArgsProvider;
        }

        Task IModule.OnEnterAsync()
        {
            ShowSplashScreenAsync().FireAndForget();

            return Task.CompletedTask;
        }

        Task IModule.OnExitAsync()
        {
            return Task.CompletedTask;
        }

        private async Task ShowSplashScreenAsync()
        {
            await m_SplashScreenModuleMonoBehaviour.ShowSplashScreenAsync();

            m_ChangeModuleStore.SetModuleId(MenuConstants.MODULE_ID);

            m_CoreModule.RequestModuleChangeAsync().FireAndForget();
        }
    }
}