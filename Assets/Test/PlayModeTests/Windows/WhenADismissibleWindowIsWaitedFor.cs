using System.Collections;
using NUnit.Framework;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;
using UnityEngine.UIElements;

namespace Test.PlayModeTests.Windows
{
    /// <summary>
    /// UseCaseWindowDismissible: talked to, `SHOW_DIALOGUE_WINDOW_NO_WAIT_DISMISSIBLE`, `WAIT_FOR_DIALOGUE_WINDOW`,
    /// then `ADD_MONEY 1`.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenADismissibleWindowIsWaitedFor
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
        public IEnumerator ThenTheScriptGoesOnOnceAPressClosesIt()
        {
            yield return UseCaseGame.Enter(UseCaseField.WINDOWS, 2);

            uint before = UseCaseGame.ReadUint("Money");

            yield return m_Input.Talk();

            yield return Wait.Until(() => UseCaseDialogue.Shown() != null, 5f, "the window showed");
            yield return Wait.Seconds(1f);

            Assert.That(UseCaseGame.ReadUint("Money"), Is.EqualTo(before), "the script went on before the window closed");

            yield return UseCaseDialogue.Dismiss(m_Input);
            yield return Wait.Until(() => UseCaseGame.ReadUint("Money") == before + 1, 3f, "the script went on once a press closed the window");
        }
    }
}
