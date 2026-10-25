using RPGFramework.Core;
using RPGFramework.DI;
using UnityEngine;

namespace Test.SplashScreen
{
    internal class SplashScreenModuleMonoBehaviour : MonoBehaviour, IModuleBindings
    {
        void IModuleBindings.Bind(IDIContainer sceneContainer)
        {
            sceneContainer.BindSingletonIfNotRegistered<ISplashScreenModule, SplashScreenModule>();
        }
    }
}