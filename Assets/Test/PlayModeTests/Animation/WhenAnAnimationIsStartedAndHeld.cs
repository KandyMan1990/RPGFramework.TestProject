using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Test.PlayModeTests.Animation
{
    /// <summary>
    /// UseCasePlayHolding: talked to, `PLAY_ANIMATION_HOLDING Wave` and `ADD_MONEY 1`.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenAnAnimationIsStartedAndHeld
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
        public IEnumerator ThenTheScriptGoesOnAndItHoldsItsLastFrame()
        {
            yield return UseCaseGame.Enter(UseCaseField.ANIMATION, 3);

            Transform actor  = UseCaseMovement.Body("UseCasePlayHolding");
            uint      before = UseCaseMovement.Money;

            yield return m_Input.Talk();
            yield return Wait.Until(() => UseCaseMovement.Money == before + 1, 2f, "the script went on as Wave began");
            yield return Wait.Until(() => UseCaseAnimation.Plays(actor, "Wave") && UseCaseAnimation.Finished(actor) >= 0.95f, 4f, "Wave at its last frame");
            yield return Wait.Seconds(0.5f);

            Assert.That(UseCaseAnimation.Plays(actor, "Wave"), "Wave still held");
        }
    }
}
