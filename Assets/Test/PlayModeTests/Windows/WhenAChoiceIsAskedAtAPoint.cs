using System.Collections;
using NUnit.Framework;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;
using UnityEngine.UIElements;

namespace Test.PlayModeTests.Windows
{
    /// <summary>
    /// UseCaseAskAt: talked to, `ASK_PLAYER_AT 0 300 200`, starting on the second of two answers.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenAChoiceIsAskedAtAPoint
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
        public IEnumerator ThenItShowsThereFittedAndStoresTheAnswerStartedOn()
        {
            yield return UseCaseGame.Enter(UseCaseField.WINDOWS, 6);

            VisualElement window = null;

            yield return m_Input.Talk();

            yield return Wait.Until(() => (window = UseCaseDialogue.Shown()) != null && UseCaseDialogue.FocusedAnswer(window) == 1, 5f, "the question typed, the answer it starts on focused");

            Assert.That(UseCaseDialogue.IsAt(window, 300f, 200f), "the window at 300, 200");
            Assert.That(UseCaseDialogue.IsFitted(window), "the window sized to its question and answers");

            yield return m_Input.Talk();
            yield return Wait.Until(() => UseCaseGame.ReadByte(UseCaseProject.ANSWER) == 1, 5f, "the answer started on stored, 1");
        }
    }
}
