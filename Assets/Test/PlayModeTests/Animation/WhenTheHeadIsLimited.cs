using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Test.PlayModeTests.Animation
{
    /// <summary>
    /// UseCaseHeadLimits: talked to, `SET_HEAD_LIMITS 20 20`, `HEAD_LOOK_AT_POSITION_AND_WAIT` a point an eighth of a turn round and
    /// `ADD_MONEY 1`.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenTheHeadIsLimited
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
        public IEnumerator ThenItTurnsNoFurther()
        {
            yield return UseCaseGame.Enter(UseCaseField.ANIMATION, 23);

            Transform actor  = UseCaseMovement.Body("UseCaseHeadLimits");
            uint      before = UseCaseMovement.Money;

            Transform  head = UseCaseAnimation.Head(actor);
            Quaternion rest = head.rotation;

            yield return m_Input.Talk();
            yield return Wait.Until(() => UseCaseMovement.Money == before + 1, 3f, "the script went on once the head had turned");

            Assert.That(Quaternion.Angle(rest, head.rotation), Is.InRange(8f, 32f), "the head turned only as far as its limit");
        }
    }
}
