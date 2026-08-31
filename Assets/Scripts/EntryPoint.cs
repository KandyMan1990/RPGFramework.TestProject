using System.Threading.Tasks;
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
            async Task Run()
            {
                ICoreModule entryPoint = await CoreModuleBuilder.Create(m_GlobalContainer, SplashScreenConstants.MODULE_ID);

                await entryPoint.RequestModuleChangeAsync();
            }
            
            Run().FireAndForget();
        }
    }
}