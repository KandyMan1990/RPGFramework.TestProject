using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Test.PlayModeTests.Animation
{
    /// <summary>
    /// UseCaseHeadAtEntity: talked to, `HEAD_LOOK_AT_ENTITY` `UseCaseHeadTargetToo` over half a second, `ADD_MONEY 1`, `WAIT_FOR_HEAD` and
    /// `ADD_MONEY 1`.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenAHeadIsSentToAnEntity
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
            yield return UseCaseGame.Enter(UseCaseField.ANIMATION, 18);

            Transform actor  = UseCaseMovement.Body("UseCaseHeadAtEntity");
            uint      before = UseCaseMovement.Money;

            Transform  head = UseCaseAnimation.Head(actor);
            Quaternion rest = head.rotation;

            yield return m_Input.Talk();
            yield return Wait.Until(() => UseCaseMovement.Money >= before + 1, 2f, "the script went on as the head began to turn");

            Assert.That(Quaternion.Angle(rest, head.rotation), Is.LessThan(30f), "the script waited for the head to turn before going on");

            yield return Wait.Until(() => UseCaseMovement.Money == before + 2, 3f, "the script went on once the head had turned");

            Assert.That(Quaternion.Angle(rest, head.rotation), Is.InRange(30f, 60f), "the head turned to the marker, an eighth of a turn round");
        }
    }
}
