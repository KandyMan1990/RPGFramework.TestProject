using System.Collections;
using NUnit.Framework;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;
using UnityEngine.UIElements;

namespace Test.PlayModeTests.Windows
{
    /// <summary>
    /// UseCaseWindowStyle: talked to, `SET_DIALOGUE_WINDOW_STYLE 0 Thought`, then `SHOW_DIALOGUE_WINDOW`.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenAWindowIsGivenAStyle
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
        public IEnumerator ThenItShowsAsAThought()
        {
            yield return UseCaseGame.Enter(UseCaseField.WINDOWS, 3);

            VisualElement window = null;

            yield return m_Input.Talk();

            yield return Wait.Until(() => (window = UseCaseDialogue.Shown()) != null, 5f, "the window showed");

            Assert.That(UseCaseDialogue.HasStyle(window, "thought"), "the window styled as a thought");

            yield return UseCaseDialogue.Dismiss(m_Input);
        }
    }
}
