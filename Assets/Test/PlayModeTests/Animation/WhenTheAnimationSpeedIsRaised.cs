using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Test.PlayModeTests.Animation
{
    /// <summary>
    /// UseCaseAnimationSpeed: talked to, `SET_ANIMATION_SPEED 2`, `PLAY_ANIMATION_AND_WAIT Spin` and `ADD_MONEY 1`.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenTheAnimationSpeedIsRaised
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
        public IEnumerator ThenItPlaysFaster()
        {
            yield return UseCaseGame.Enter(UseCaseField.ANIMATION, 10);

            Transform actor  = UseCaseMovement.Body("UseCaseAnimationSpeed");
            uint      before = UseCaseMovement.Money;

            yield return m_Input.Talk();
            yield return Wait.Until(() => UseCaseAnimation.Plays(actor, "Spin"), 2f, "Spin playing");

            float started = Time.realtimeSinceStartup;
            float length  = UseCaseAnimation.Length(actor);

            yield return Wait.Until(() => UseCaseMovement.Money == before + 1, length * 2f, "the script went on once Spin had finished");

            Assert.That(Time.realtimeSinceStartup - started, Is.LessThan(length * 0.8f), "Spin played at twice its speed");
        }
    }
}
