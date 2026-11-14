using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Test.PlayModeTests.Animation
{
    /// <summary>
    /// UseCaseHeadForward: talked to, `HEAD_LOOK_AT_POSITION_AND_WAIT` a point, `HEAD_LOOK_FORWARD 0.3`, `ADD_MONEY 1`, `WAIT_FOR_HEAD` and
    /// `ADD_MONEY 1`.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenAHeadIsSentBack
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
        public IEnumerator ThenTheScriptGoesOnAndThenWaitsForTheTurn()
        {
            yield return UseCaseGame.Enter(UseCaseField.ANIMATION, 22);

            Transform actor  = UseCaseMovement.Body("UseCaseHeadForward");
            uint      before = UseCaseMovement.Money;

            Transform  head = UseCaseAnimation.Head(actor);
            Quaternion rest = head.rotation;

            yield return m_Input.Talk();
            yield return Wait.Until(() => Quaternion.Angle(rest, head.rotation) > 30f, 2f, "the head turned to the point");
            yield return Wait.Until(() => UseCaseMovement.Money >= before + 1, 2f, "the script went on as the head began to turn back");
            yield return Wait.Until(() => UseCaseMovement.Money == before + 2, 3f, "the script went on once the head had turned back");

            Assert.That(Quaternion.Angle(rest, head.rotation), Is.LessThan(12f), "the head as the animation has it");
        }
    }
}
