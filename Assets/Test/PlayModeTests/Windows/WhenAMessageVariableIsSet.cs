using System.Collections;
using NUnit.Framework;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;
using UnityEngine.UIElements;

namespace Test.PlayModeTests.Windows
{
    /// <summary>
    /// UseCaseMessageVariable: talked to, `SET_MESSAGE_VARIABLE 0 1234`, then a line showing `{Var 0}`.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenAMessageVariableIsSet
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
        public IEnumerator ThenTheWindowShowsItsValue()
        {
            yield return UseCaseGame.Enter(UseCaseField.WINDOWS, 4);

            yield return m_Input.Talk();

            yield return Wait.Until(() => UseCaseDialogue.Shown() != null && UseCaseDialogue.Text(UseCaseDialogue.Shown()).Contains("1234"), 5f, "the window showed 1234");
            yield return UseCaseDialogue.Dismiss(m_Input);
        }
    }
}
