using RPGFramework.Core;
using RPGFramework.Core.SaveData;
using RPGFramework.Core.Store;
using RPGFramework.Menu.SharedTypes.Constants;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Test
{
    /// <summary>
    /// Temporary, until the Save menu exists (backlog item 91). F5 commits the current save while the menu is open
    /// during play, which is where the Save menu will make one.
    /// </summary>
    internal sealed class SaveKey : IUpdatable
    {
        private readonly ISaveDataService   m_SaveDataService;
        private readonly IChangeModuleStore m_ChangeModuleStore;

        public SaveKey(ISaveDataService saveDataService, IChangeModuleStore changeModuleStore)
        {
            m_SaveDataService   = saveDataService;
            m_ChangeModuleStore = changeModuleStore;
        }

        void IUpdatable.Update()
        {
            // New Game sets the next module before its Config menu opens, so this also keeps F5 out of that menu,
            // where the player has not stood anywhere yet.
            bool inMenu = m_ChangeModuleStore.GetModuleId == MenuConstants.MODULE_ID;

            if (!inMenu || !m_SaveDataService.HasSaveLoaded() || !Keyboard.current.f5Key.wasPressedThisFrame)
            {
                return;
            }

            m_SaveDataService.CommitSave();

            Debug.Log($"{nameof(SaveKey)} Saved");
        }
    }
}
