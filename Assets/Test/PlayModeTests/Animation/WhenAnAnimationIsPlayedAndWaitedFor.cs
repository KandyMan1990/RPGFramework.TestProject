using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Test.PlayModeTests.Animation
{
    /// <summary>
    /// UseCasePlayAndWait: talked to, `PLAY_ANIMATION_AND_WAIT Wave` and `ADD_MONEY 1`.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenAnAnimationIsPlayedAndWaitedFor
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
        public IEnumerator ThenItPlaysAndThenGoesBackToTheBase()
        {
            yield return UseCaseGame.Enter(UseCaseField.ANIMATION, 0);

            Transform actor  = UseCaseMovement.Body("UseCasePlayAndWait");
            uint      before = UseCaseMovement.Money;

            yield return m_Input.Talk();
            yield return Wait.Until(() => UseCaseAnimation.Plays(actor, "Wave"), 2f, "Wave playing");

            Assert.That(UseCaseMovement.Money, Is.EqualTo(before), "the script went on before Wave had finished");

            yield return Wait.Until(() => UseCaseMovement.Money == before + 1, 4f, "the script went on once Wave had finished");
            yield return Wait.Until(() => UseCaseAnimation.Plays(actor, "Stand"), 2f, "back to its base, Stand");
        }
    }
}
