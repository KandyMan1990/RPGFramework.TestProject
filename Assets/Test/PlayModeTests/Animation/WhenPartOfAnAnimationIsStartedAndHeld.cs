using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Test.PlayModeTests.Animation
{
    /// <summary>
    /// UseCasePartHolding: talked to, `PLAY_PART_OF_ANIMATION_HOLDING Spin 0.25 0.75` and `ADD_MONEY 1`.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenPartOfAnAnimationIsStartedAndHeld
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
        public IEnumerator ThenTheScriptGoesOnAndItHoldsThePartsEnd()
        {
            yield return UseCaseGame.Enter(UseCaseField.ANIMATION, 8);

            Transform actor  = UseCaseMovement.Body("UseCasePartHolding");
            uint      before = UseCaseMovement.Money;

            yield return m_Input.Talk();
            yield return Wait.Until(() => UseCaseMovement.Money == before + 1, 2f, "the script went on as the part began");
            yield return Wait.Until(() => UseCaseAnimation.Plays(actor, "Spin") && UseCaseAnimation.Through(actor) >= 0.7f, 5f, "Spin at the end of its part");
            yield return Wait.Seconds(0.5f);

            Assert.That(UseCaseAnimation.Plays(actor, "Spin"), "Spin still held");
            Assert.That(UseCaseAnimation.Through(actor), Is.InRange(0.7f, 0.8f), "held at the end of its part");
        }
    }
}
