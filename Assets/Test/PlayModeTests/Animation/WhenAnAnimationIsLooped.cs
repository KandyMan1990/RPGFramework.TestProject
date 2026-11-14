using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Test.PlayModeTests.Animation
{
    /// <summary>
    /// UseCasePlayLooping: talked to, `PLAY_ANIMATION_LOOPING Wave` and `ADD_MONEY 1`.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenAnAnimationIsLooped
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
        public IEnumerator ThenItKeepsPlaying()
        {
            yield return UseCaseGame.Enter(UseCaseField.ANIMATION, 4);

            Transform actor  = UseCaseMovement.Body("UseCasePlayLooping");
            uint      before = UseCaseMovement.Money;

            yield return m_Input.Talk();
            yield return Wait.Until(() => UseCaseMovement.Money == before + 1, 2f, "the script went on as Wave began");
            yield return Wait.Until(() => UseCaseAnimation.Plays(actor, "Wave"), 2f, "Wave playing");
            yield return Wait.Seconds(UseCaseAnimation.Length(actor) * 2.5f);

            Assert.That(UseCaseAnimation.Plays(actor, "Wave"), "Wave still playing after two passes");
        }
    }
}
