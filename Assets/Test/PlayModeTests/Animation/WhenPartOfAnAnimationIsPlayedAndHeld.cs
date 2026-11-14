using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Test.PlayModeTests.Animation
{
    /// <summary>
    /// UseCasePartAndHold: talked to, `PLAY_PART_OF_ANIMATION_AND_HOLD Spin 0.25 0.75` and `ADD_MONEY 1`.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenPartOfAnAnimationIsPlayedAndHeld
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
        public IEnumerator ThenItHoldsThePartsEnd()
        {
            yield return UseCaseGame.Enter(UseCaseField.ANIMATION, 6);

            Transform actor  = UseCaseMovement.Body("UseCasePartAndHold");
            uint      before = UseCaseMovement.Money;

            yield return m_Input.Talk();
            yield return Wait.Until(() => UseCaseMovement.Money == before + 1, 5f, "the script went on once the part had finished");
            yield return Wait.Seconds(0.5f);

            Assert.That(UseCaseAnimation.Plays(actor, "Spin"), "Spin still held");
            Assert.That(UseCaseAnimation.Through(actor), Is.InRange(0.7f, 0.8f), "held at the end of its part");
        }
    }
}
