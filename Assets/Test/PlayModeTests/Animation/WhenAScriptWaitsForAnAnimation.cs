using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Test.PlayModeTests.Animation
{
    /// <summary>
    /// UseCaseWaitForAnimation: talked to, `PLAY_ANIMATION Wave`, `ADD_MONEY 1`, `WAIT_FOR_ANIMATION` and `ADD_MONEY 1`.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenAScriptWaitsForAnAnimation
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
        public IEnumerator ThenItGoesOnOnceTheAnimationHasFinished()
        {
            yield return UseCaseGame.Enter(UseCaseField.ANIMATION, 11);

            Transform actor  = UseCaseMovement.Body("UseCaseWaitForAnimation");
            uint      before = UseCaseMovement.Money;

            yield return m_Input.Talk();
            yield return Wait.Until(() => UseCaseMovement.Money >= before + 1, 2f, "the script went on as Wave began");
            yield return Wait.Until(() => UseCaseAnimation.Plays(actor, "Wave"), 2f, "Wave playing");

            Assert.That(UseCaseMovement.Money, Is.EqualTo(before + 1), "the script went on before Wave had finished");

            yield return Wait.Until(() => UseCaseMovement.Money == before + 2, 4f, "the script went on once Wave had finished");
        }
    }
}
