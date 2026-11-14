using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Test.PlayModeTests.Animation
{
    /// <summary>
    /// UseCaseHeadAtEntityAndWait: talked to, `HEAD_LOOK_AT_ENTITY_AND_WAIT` `UseCaseHeadTarget` over half a second and `ADD_MONEY 1`.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenAHeadTurnsToAnEntityAndWaits
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
        public IEnumerator ThenItLooksAtItAndTheScriptGoesOn()
        {
            yield return UseCaseGame.Enter(UseCaseField.ANIMATION, 17);

            Transform actor  = UseCaseMovement.Body("UseCaseHeadAtEntityAndWait");
            uint      before = UseCaseMovement.Money;

            Transform  head = UseCaseAnimation.Head(actor);
            Quaternion rest = head.rotation;

            yield return m_Input.Talk();
            yield return Wait.Seconds(0.2f);

            Assert.That(UseCaseMovement.Money, Is.EqualTo(before), "the script went on before the head had turned");

            yield return Wait.Until(() => UseCaseMovement.Money == before + 1, 3f, "the script went on once the head had turned");

            Assert.That(Quaternion.Angle(rest, head.rotation), Is.InRange(30f, 60f), "the head turned to the marker, an eighth of a turn round");
        }
    }
}
