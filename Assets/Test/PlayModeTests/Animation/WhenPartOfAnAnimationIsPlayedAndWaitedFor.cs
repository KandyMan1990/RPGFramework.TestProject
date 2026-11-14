using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Test.PlayModeTests.Animation
{
    /// <summary>
    /// UseCasePartAndWait: talked to, `PLAY_PART_OF_ANIMATION_AND_WAIT Spin 0.25 0.75` and `ADD_MONEY 1`.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenPartOfAnAnimationIsPlayedAndWaitedFor
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
        public IEnumerator ThenItPlaysThatPartAndGoesBackToTheBase()
        {
            yield return UseCaseGame.Enter(UseCaseField.ANIMATION, 5);

            Transform actor  = UseCaseMovement.Body("UseCasePartAndWait");
            uint      before = UseCaseMovement.Money;

            yield return m_Input.Talk();
            yield return Wait.Until(() => UseCaseAnimation.Plays(actor, "Spin"), 2f, "Spin playing");

            Assert.That(UseCaseAnimation.Through(actor), Is.InRange(0.2f, 0.8f), "Spin playing from its part");
            Assert.That(UseCaseMovement.Money, Is.EqualTo(before), "the script went on before the part had finished");

            yield return Wait.Until(() => UseCaseMovement.Money == before + 1, 5f, "the script went on once the part had finished");
            yield return Wait.Until(() => UseCaseAnimation.Plays(actor, "Stand"), 2f, "back to its base, Stand");
        }
    }
}
