using RPGFramework.Core;
using RPGFramework.Core.SharedTypes;
using RPGFramework.Menu;
using RPGFramework.Menu.SharedTypes;
using UnityEngine;

namespace Test
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField]
        private TestGlobalInstaller m_GlobalContainer;

        private void Start()
        {
            IEntryPoint entryPoint = CoreModule.Create(m_GlobalContainer);

            IModuleArgs args = new MenuModuleArgs<IBeginMenu>();

            entryPoint.StartGameAsync<IMenuModule>(args).FireAndForget();
        }
    }
}