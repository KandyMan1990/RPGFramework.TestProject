using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Test.PlayModeTests.Animation
{
    /// <summary>
    /// UseCasePlay: talked to, `PLAY_ANIMATION Wave` and `ADD_MONEY 1`.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenAnAnimationIsStarted
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
        public IEnumerator ThenTheScriptGoesOnAndItGoesBackToTheBase()
        {
            yield return UseCaseGame.Enter(UseCaseField.ANIMATION, 2);

            Transform actor  = UseCaseMovement.Body("UseCasePlay");
            uint      before = UseCaseMovement.Money;

            yield return m_Input.Talk();
            yield return Wait.Until(() => UseCaseMovement.Money == before + 1, 2f, "the script went on as Wave began");
            yield return Wait.Until(() => UseCaseAnimation.Plays(actor, "Wave"), 2f, "Wave playing");
            yield return Wait.Until(() => UseCaseAnimation.Plays(actor, "Stand"), 4f, "back to its base, Stand, once Wave had finished");
        }
    }
}
