using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Test.PlayModeTests.Animation
{
    /// <summary>
    /// UseCasePartLooping: talked to, `PLAY_PART_OF_ANIMATION_LOOPING Spin 0.25 0.75` and `ADD_MONEY 1`.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenPartOfAnAnimationIsLooped
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
        public IEnumerator ThenItKeepsPlayingThatPart()
        {
            yield return UseCaseGame.Enter(UseCaseField.ANIMATION, 9);

            Transform actor  = UseCaseMovement.Body("UseCasePartLooping");
            uint      before = UseCaseMovement.Money;

            yield return m_Input.Talk();
            yield return Wait.Until(() => UseCaseMovement.Money == before + 1, 2f, "the script went on as the part began");
            yield return Wait.Until(() => UseCaseAnimation.Plays(actor, "Spin"), 2f, "Spin playing");

            float until = Time.realtimeSinceStartup + UseCaseAnimation.Length(actor) * 1.2f;

            while (Time.realtimeSinceStartup < until)
            {
                Assert.That(UseCaseAnimation.Plays(actor, "Spin"), "Spin still playing");
                Assert.That(UseCaseAnimation.Through(actor), Is.InRange(0.2f, 0.8f), "Spin within its part");

                yield return null;
            }
        }
    }
}
