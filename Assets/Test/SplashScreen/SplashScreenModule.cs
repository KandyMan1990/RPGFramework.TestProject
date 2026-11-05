using System.Threading.Tasks;
using RPGFramework.Core;
using RPGFramework.Core.SharedTypes;
using RPGFramework.Menu.SharedTypes;
using RPGFramework.Menu.SharedTypes.Constants;
using RPGFramework.Menu.SharedTypes.Stores;
using Test.SplashScreen.Constants;
using UnityEngine;

namespace Test.SplashScreen
{
    public interface ISplashScreenModule : IModule
    {
    }

    internal sealed class SplashScreenModule : ISplashScreenModule
    {
        private const float HOLD_SECONDS = 3f;

        private readonly ICoreModule    m_CoreModule;
        private readonly IMenuArgsStore m_MenuArgsStore;

        internal SplashScreenModule(ICoreModule coreModule, IMenuArgsStore menuArgsStore)
        {
            m_CoreModule    = coreModule;
            m_MenuArgsStore = menuArgsStore;
        }

        Task IModule.OnEnterAsync(byte entry)
        {
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

        void IModule.OnRevealed()
        {
            LeaveAfterHoldAsync().FireAndForget();
        }

        private async Task LeaveAfterHoldAsync()
        {
            await Awaitable.WaitForSecondsAsync(HOLD_SECONDS);

            m_MenuArgsStore.Set(new MenuArgs(MenuIds.Begin));
            m_CoreModule.RequestModuleChangeAsync(SplashScreenConstants.OUTCOME_FINISHED).FireAndForget();
        }
    }
}
