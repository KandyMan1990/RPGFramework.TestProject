using System.Collections;
using NUnit.Framework;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;
using UnityEngine.UIElements;

namespace Test.PlayModeTests.Windows
{
    /// <summary>
    /// UseCaseDialogueAtDismissible: talked to, `SHOW_DIALOGUE_AT_NO_WAIT_DISMISSIBLE 0 UseCases/Line 300 400`,
    /// `WAIT_FOR_DIALOGUE_WINDOW`, then `ADD_MONEY 1`.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenDismissibleDialogueIsShownAtAPoint
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
        public IEnumerator ThenItShowsThereAndTheScriptGoesOnOnceAPressClosesIt()
        {
            yield return UseCaseGame.Enter(UseCaseField.WINDOWS, 11);

            uint before = UseCaseGame.ReadUint("Money");

            yield return m_Input.Talk();

            yield return Wait.Until(() => UseCaseDialogue.Shown() != null && UseCaseDialogue.IsAt(UseCaseDialogue.Shown(), 300f, 400f), 5f, "the window showed at 300, 400");
            yield return Wait.Seconds(1f);

            Assert.That(UseCaseGame.ReadUint("Money"), Is.EqualTo(before), "the script went on before the window closed");

            yield return UseCaseDialogue.Dismiss(m_Input);
            yield return Wait.Until(() => UseCaseGame.ReadUint("Money") == before + 1, 3f, "the script went on once a press closed the window");
        }
    }
}
