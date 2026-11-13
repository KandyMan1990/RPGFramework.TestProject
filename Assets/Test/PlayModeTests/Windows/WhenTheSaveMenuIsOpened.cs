using System.Collections;
using NUnit.Framework;
using RPGFramework.Menu.SharedTypes.Constants;
using RPGFramework.Menu.SharedTypes.Stores;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;
using UnityEngine.UIElements;

namespace Test.PlayModeTests.Windows
{
    /// <summary>
    /// UseCaseSaveMenu: talked to, `OPEN_SAVE_MENU`, then `ADD_MONEY 1`.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenTheSaveMenuIsOpened
    {
        private UseCaseInput m_Input;

        [SetUp]
        public void SetUp()
        {
            m_Input = new UseCaseInput();
        }

        [TearDown]
        public void TearDown()
        {
            m_Input.TearDown();
        }

        [UnityTest]
        public IEnumerator ThenItIsTheSaveMenuAndTheScriptGoesOnOnceItCloses()
        {
            yield return UseCaseGame.Enter(UseCaseField.WINDOWS, 8);

            uint before = UseCaseGame.ReadUint("Money");

            yield return m_Input.Talk();

            yield return Wait.Until(() => UseCaseGame.IsMenuOpen && UseCaseGame.Resolve<IMenuArgsStore>().Args.MenuId == MenuIds.Save, 5f, "the save menu opened");

            Assert.That(UseCaseGame.ReadUint("Money"), Is.EqualTo(before), "the script went on before the save menu closed");

            yield return UseCaseGame.CloseMenu(m_Input);
            yield return Wait.Until(() => UseCaseGame.ReadUint("Money") == before + 1, 5f, "the script went on once the save menu closed");
        }
    }
}
