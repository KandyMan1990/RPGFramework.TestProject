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
            IEntryPoint entryPoint = CoreModuleBuilder.Create(m_GlobalContainer);

            IModuleArgs args = new GenericMenuModuleArgs<IBeginMenu>();

            entryPoint.StartGameAsync<IMenuModule>(args).FireAndForget();
        }
    }
}