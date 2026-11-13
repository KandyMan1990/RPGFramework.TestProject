using System.Collections;
using NUnit.Framework;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;
using UnityEngine.UIElements;

namespace Test.PlayModeTests.Windows
{
    /// <summary>
    /// UseCaseDialogueAtNoWait: talked to, `SHOW_DIALOGUE_AT_NO_WAIT 0 UseCases/Line 300 400`, `ADD_MONEY 1`, two
    /// seconds, then `CLOSE_DIALOGUE_WINDOW`.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenDialogueIsShownAtAPointWithoutWaiting
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
        public IEnumerator ThenItShowsThereWhileTheScriptGoesOnAndClosesIt()
        {
            yield return UseCaseGame.Enter(UseCaseField.WINDOWS, 10);

            uint before = UseCaseGame.ReadUint("Money");

            yield return m_Input.Talk();

            yield return Wait.Until(() => UseCaseDialogue.Shown() != null && UseCaseDialogue.IsAt(UseCaseDialogue.Shown(), 300f, 400f) && UseCaseGame.ReadUint("Money") == before + 1, 5f, "the window showed at 300, 400 while the script went on");
            yield return Wait.Until(() => UseCaseDialogue.Shown() == null, 5f, "the script closed the window");
        }
    }
}
