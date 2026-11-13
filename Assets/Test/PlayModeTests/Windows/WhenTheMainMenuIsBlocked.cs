using System.Collections;
using NUnit.Framework;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;
using UnityEngine.UIElements;

namespace Test.PlayModeTests.Windows
{
    /// <summary>
    /// UseCaseMenuBlocked: Menu opens the menu; talked to, `MAIN_MENU_ACCESSIBILITY false`, after which it does not.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenTheMainMenuIsBlocked
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
        public IEnumerator ThenMenuNoLongerOpensIt()
        {
            yield return UseCaseGame.Enter(UseCaseField.WINDOWS, 7);

            yield return m_Input.Tap(m_Input.Keyboard.qKey);
            yield return Wait.Until(() => UseCaseGame.IsMenuOpen, 5f, "Menu opened the menu before it was blocked");
            yield return UseCaseGame.CloseMenu(m_Input);

            InputAction confirm = UseCaseGame.Action(UseCaseProject.CONFIRM);

            yield return Wait.Until(() => confirm.enabled, 5f, "the field took the player's input again");
            yield return m_Input.Talk();
            yield return m_Input.Tap(m_Input.Keyboard.qKey);
            yield return Wait.Seconds(1.5f);

            Assert.That(UseCaseGame.IsMenuOpen, Is.False, "Menu opened the menu once it was blocked");
        }
    }
}
