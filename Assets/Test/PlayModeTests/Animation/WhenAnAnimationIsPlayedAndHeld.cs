using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Test.PlayModeTests.Animation
{
    /// <summary>
    /// UseCasePlayAndHold: talked to, `PLAY_ANIMATION_AND_HOLD Wave` and `ADD_MONEY 1`.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenAnAnimationIsPlayedAndHeld
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
        public IEnumerator ThenItHoldsItsLastFrame()
        {
            yield return UseCaseGame.Enter(UseCaseField.ANIMATION, 1);

            Transform actor  = UseCaseMovement.Body("UseCasePlayAndHold");
            uint      before = UseCaseMovement.Money;

            yield return m_Input.Talk();
            yield return Wait.Until(() => UseCaseMovement.Money == before + 1, 4f, "the script went on once Wave had finished");
            yield return Wait.Seconds(0.5f);

            Assert.That(UseCaseAnimation.Plays(actor, "Wave"), "Wave still held");
            Assert.That(UseCaseAnimation.Finished(actor), Is.GreaterThanOrEqualTo(0.95f), "held at its last frame");
        }
    }
}
