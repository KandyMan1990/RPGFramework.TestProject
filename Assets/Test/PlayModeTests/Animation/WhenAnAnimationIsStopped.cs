using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Test.PlayModeTests.Animation
{
    /// <summary>
    /// UseCaseStopAnimation: talked to, `PLAY_ANIMATION_LOOPING Spin`, `WAIT_SECONDS 0.5`, `STOP_ANIMATION` and `ADD_MONEY 1`.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenAnAnimationIsStopped
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
        public IEnumerator ThenItGoesBackToTheBaseAtOnce()
        {
            yield return UseCaseGame.Enter(UseCaseField.ANIMATION, 12);

            Transform actor  = UseCaseMovement.Body("UseCaseStopAnimation");
            uint      before = UseCaseMovement.Money;

            yield return m_Input.Talk();
            yield return Wait.Until(() => UseCaseAnimation.Plays(actor, "Spin"), 2f, "Spin playing");
            yield return Wait.Until(() => UseCaseMovement.Money == before + 1, 3f, "the script went on once it had stopped Spin");
            yield return Wait.Until(() => UseCaseAnimation.Plays(actor, "Stand"), 0.5f, "back to its base, Stand, at once");
        }
    }
}
