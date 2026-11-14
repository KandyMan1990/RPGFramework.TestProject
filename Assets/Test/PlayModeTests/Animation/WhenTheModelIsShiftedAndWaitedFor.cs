using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Test.PlayModeTests.Animation
{
    /// <summary>
    /// UseCaseDrawOffsetAndWait: talked to, `SET_ENTITY_DRAW_OFFSET_AND_WAIT 0 1 0 0.5 0` and `ADD_MONEY 1`: its model one higher, the body
    /// where it was.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenTheModelIsShiftedAndWaitedFor
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
        public IEnumerator ThenItIsDrawnHigherAndTheScriptGoesOn()
        {
            yield return UseCaseGame.Enter(UseCaseField.ANIMATION, 15);

            Transform actor  = UseCaseMovement.Body("UseCaseDrawOffsetAndWait");
            uint      before = UseCaseMovement.Money;

            Vector3 rest = UseCaseAnimation.Drawn(actor);

            yield return m_Input.Talk();
            yield return Wait.Seconds(0.2f);

            Assert.That(UseCaseMovement.Money, Is.EqualTo(before), "the script went on before the model had moved");

            yield return Wait.Until(() => UseCaseMovement.Money == before + 1, 3f, "the script went on once the model had moved");

            Assert.That((UseCaseAnimation.Drawn(actor) - rest - Vector3.up).magnitude, Is.LessThan(0.02f), "the model drawn one higher");
        }
    }
}
