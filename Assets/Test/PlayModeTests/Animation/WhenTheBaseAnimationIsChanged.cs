using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Test.PlayModeTests.Animation
{
    /// <summary>
    /// UseCaseBaseAnimation: talked to, `SET_BASE_ANIMATION Walk` and `ADD_MONEY 1`.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenTheBaseAnimationIsChanged
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
        public IEnumerator ThenItIsWhatTheCharacterDoes()
        {
            yield return UseCaseGame.Enter(UseCaseField.ANIMATION, 14);

            Transform actor  = UseCaseMovement.Body("UseCaseBaseAnimation");
            uint      before = UseCaseMovement.Money;

            Assert.That(UseCaseAnimation.Plays(actor, "Stand"), "Stand its base to begin with");

            yield return m_Input.Talk();
            yield return Wait.Until(() => UseCaseMovement.Money == before + 1, 2f, "talked to");
            yield return Wait.Until(() => UseCaseAnimation.Plays(actor, "Walk"), 2f, "Walk its base");
        }
    }
}
