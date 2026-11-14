using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Test.PlayModeTests.Audio
{
    /// <summary>
    /// UseCaseSoundStopped: talked to, UseCaseSound on channel 1, half a second, then `STOP_SOUND 1`, well before its
    /// three seconds end.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenASoundIsStopped
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
        public IEnumerator ThenItStops()
        {
            yield return UseCaseGame.Enter(UseCaseField.AUDIO, 9);
            yield return UseCaseAudio.Silence();

            yield return m_Input.Talk();

            yield return Wait.Until(() => UseCaseAudio.Sound("UseCaseSound") != null, 5f, "the sound playing");
            yield return Wait.Until(() => UseCaseAudio.Sound("UseCaseSound") == null, 1.5f, "the sound stopped before it ended");
        }
    }
}
