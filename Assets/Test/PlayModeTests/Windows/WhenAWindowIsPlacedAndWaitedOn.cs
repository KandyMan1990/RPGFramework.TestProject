using System.Collections;
using NUnit.Framework;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;
using UnityEngine.UIElements;

namespace Test.PlayModeTests.Windows
{
    /// <summary>
    /// UseCaseWindowPlaced: talked to, `CREATE_DIALOGUE_WINDOW 0 200 150 900 200`, then `SHOW_DIALOGUE_WINDOW`, then
    /// `ADD_MONEY 1`.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenAWindowIsPlacedAndWaitedOn
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
        public IEnumerator ThenItShowsWhereAndAsLargeAsSetUntilDismissed()
        {
            yield return UseCaseGame.Enter(UseCaseField.WINDOWS, 0);

            uint          before = UseCaseGame.ReadUint("Money");
            VisualElement window = null;

            yield return m_Input.Talk();

            yield return Wait.Until(() => (window = UseCaseDialogue.Shown()) != null && UseCaseDialogue.IsSized(window, 900f, 200f), 5f, "the window opened to 900 by 200");

            Assert.That(UseCaseDialogue.IsAt(window, 200f, 150f), "the window at 200, 150");
            Assert.That(UseCaseDialogue.Text(window), Is.Not.Empty, "the window's text");

            yield return Wait.Seconds(0.5f);

            Assert.That(UseCaseGame.ReadUint("Money"), Is.EqualTo(before), "the script went on before the window was dismissed");

            yield return UseCaseDialogue.Dismiss(m_Input);
            yield return Wait.Until(() => UseCaseGame.ReadUint("Money") == before + 1, 3f, "the script went on once the window was dismissed");
        }
    }
}
