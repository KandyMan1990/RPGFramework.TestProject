using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Test.PlayModeTests.ScreenAndCamera
{
    /// <summary>
    /// UseCaseEffect: talked to, `EFFECT_ACTIVATION 0 true`, `WAIT_SECONDS 1` and `EFFECT_ACTIVATION 0 false`: its sparks
    /// made, then left to run their course, the effect going off once they have.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenAnEffectIsSwitchedOnAndOff
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
        public IEnumerator ThenItShowsAndThenRunsItsCourse()
        {
            yield return UseCaseGame.Enter(UseCaseField.SCREEN, 8);

            GameObject sparks = UseCaseScreen.Find(UseCaseField.Loaded().transform, "UseCaseSparks");

            Assert.That(sparks.activeInHierarchy, Is.False, "the effect on to begin with");

            yield return m_Input.Talk();
            yield return Wait.Until(() => sparks.activeInHierarchy && sparks.GetComponentInChildren<ParticleSystem>().particleCount > 0, 2f, "the effect making sparks");
            yield return Wait.Until(() => !sparks.activeInHierarchy, 5f, "the effect off once its sparks had run their course");
        }
    }
}
