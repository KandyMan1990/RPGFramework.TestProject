using System.Collections;
using NUnit.Framework;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;
using UnityEngine.UIElements;

namespace Test.PlayModeTests.Windows
{
    /// <summary>
    /// UseCaseChoice: talked to, `ASK_PLAYER_TO_MAKE_A_CHOICE` starting on the first of two answers, Cancel moving to
    /// the last.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenAChoiceIsAsked
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
        public IEnumerator ThenCancelMovesToTheLastAnswerAndItIsStored()
        {
            yield return UseCaseGame.Enter(UseCaseField.WINDOWS, 5);

            yield return m_Input.Talk();

            yield return Wait.Until(() => UseCaseDialogue.Shown() != null && UseCaseDialogue.FocusedAnswer(UseCaseDialogue.Shown()) == 0, 5f, "the question typed, its first answer focused");
            yield return m_Input.Tap(m_Input.Keyboard.escapeKey);
            yield return Wait.Until(() => UseCaseDialogue.FocusedAnswer(UseCaseDialogue.Shown()) == 1, 3f, "Cancel moved the focus to the last answer");
            yield return m_Input.Talk();
            yield return Wait.Until(() => UseCaseGame.ReadByte(UseCaseProject.ANSWER) == 1, 5f, "the second answer stored, 1");
        }
    }
}
