using RPGFramework.Core;
using Test.SplashScreen.Constants;
using UnityEngine;

namespace Test
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField]
        private TestGlobalInstaller m_GlobalContainer;

        private void Start()
        {
            ICoreModule entryPoint = CoreModuleBuilder.Create(m_GlobalContainer, SplashScreenConstants.MODULE_ID);

            entryPoint.RequestModuleChangeAsync().FireAndForget();
        }
    }
}