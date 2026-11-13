using System.Collections;
using NUnit.Framework;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;
using UnityEngine.UIElements;

namespace Test.PlayModeTests.Windows
{
    /// <summary>
    /// UseCaseDialogueAt: talked to, `SHOW_DIALOGUE_AT 0 UseCases/Line 300 400`, then `ADD_MONEY 1`.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenDialogueIsShownAtAPoint
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
        public IEnumerator ThenItShowsThereFittedUntilDismissed()
        {
            yield return UseCaseGame.Enter(UseCaseField.WINDOWS, 9);

            uint          before = UseCaseGame.ReadUint("Money");
            VisualElement window = null;

            yield return m_Input.Talk();

            yield return Wait.Until(() => (window = UseCaseDialogue.Shown()) != null && UseCaseDialogue.IsFitted(window), 5f, "the window opened, sized to its text");

            Assert.That(UseCaseDialogue.IsAt(window, 300f, 400f), "the window at 300, 400");

            yield return Wait.Seconds(0.5f);

            Assert.That(UseCaseGame.ReadUint("Money"), Is.EqualTo(before), "the script went on before the window was dismissed");

            yield return UseCaseDialogue.Dismiss(m_Input);
            yield return Wait.Until(() => UseCaseGame.ReadUint("Money") == before + 1, 3f, "the script went on once the window was dismissed");
        }
    }
}
