using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Test.PlayModeTests.Animation
{
    /// <summary>
    /// UseCasePushPop: talked to, `PLAY_ANIMATION_LOOPING Spin`, `WAIT_SECONDS 0.3`, `PUSH_ANIMATION_STATE`,
    /// `PLAY_ANIMATION_AND_WAIT Wave`, `POP_ANIMATION_STATE` and `ADD_MONEY 1`.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenAnAnimationIsRememberedAndPutBack
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
        public IEnumerator ThenItCarriesOnAfterTheInterruption()
        {
            yield return UseCaseGame.Enter(UseCaseField.ANIMATION, 13);

            Transform actor  = UseCaseMovement.Body("UseCasePushPop");
            uint      before = UseCaseMovement.Money;

            yield return m_Input.Talk();
            yield return Wait.Until(() => UseCaseAnimation.Plays(actor, "Spin"), 2f, "Spin playing");
            yield return Wait.Until(() => UseCaseAnimation.Plays(actor, "Wave"), 3f, "Wave interrupting it");
            yield return Wait.Until(() => UseCaseMovement.Money == before + 1, 4f, "the script went on once it had put Spin back");
            yield return Wait.Until(() => UseCaseAnimation.Plays(actor, "Spin"), 1f, "Spin put back");
            yield return Wait.Seconds(0.5f);

            Assert.That(UseCaseAnimation.Plays(actor, "Spin"), "Spin still looping");
        }
    }
}
