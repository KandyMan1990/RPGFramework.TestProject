using System.Collections;
using NUnit.Framework;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;
using UnityEngine.UIElements;

namespace Test.PlayModeTests.Windows
{
    /// <summary>
    /// UseCaseWindowNoWait: talked to, `SHOW_DIALOGUE_WINDOW_NO_WAIT`, `ADD_MONEY 1`, two seconds, then
    /// `CLOSE_DIALOGUE_WINDOW`.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenAWindowIsShownWithoutWaiting
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
        public IEnumerator ThenTheScriptGoesOnAndClosesIt()
        {
            yield return UseCaseGame.Enter(UseCaseField.WINDOWS, 1);

            uint before = UseCaseGame.ReadUint("Money");

            yield return m_Input.Talk();

            yield return Wait.Until(() => UseCaseDialogue.Shown() != null && UseCaseGame.ReadUint("Money") == before + 1, 5f, "the window showed while the script went on");
            yield return Wait.Until(() => UseCaseDialogue.Shown() == null, 5f, "the script closed the window");
        }
    }
}
