using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Test.PlayModeTests.Animation
{
    /// <summary>
    /// UseCaseDrawOffset: talked to, `SET_ENTITY_DRAW_OFFSET 0 1 0 0.5 0`, `ADD_MONEY 1`, `WAIT_FOR_ENTITY_DRAW_OFFSET` and `ADD_MONEY 1`.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenTheModelIsShifted
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
        public IEnumerator ThenTheScriptGoesOnAndThenWaitsForIt()
        {
            yield return UseCaseGame.Enter(UseCaseField.ANIMATION, 16);

            Transform actor  = UseCaseMovement.Body("UseCaseDrawOffset");
            uint      before = UseCaseMovement.Money;

            Vector3 rest = UseCaseAnimation.Drawn(actor);

            yield return m_Input.Talk();
            yield return Wait.Until(() => UseCaseMovement.Money >= before + 1, 2f, "the script went on as the model began to move");

            Assert.That((UseCaseAnimation.Drawn(actor) - rest - Vector3.up).magnitude, Is.GreaterThan(0.1f), "the script waited for the model to move before going on");

            yield return Wait.Until(() => UseCaseMovement.Money == before + 2, 3f, "the script went on once the model had moved");

            Assert.That((UseCaseAnimation.Drawn(actor) - rest - Vector3.up).magnitude, Is.LessThan(0.02f), "the model drawn one higher");
        }
    }
}
