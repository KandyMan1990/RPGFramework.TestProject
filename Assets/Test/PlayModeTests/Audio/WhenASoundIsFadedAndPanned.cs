using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Test.PlayModeTests.Audio
{
    /// <summary>
    /// UseCaseSoundFaded: talked to, UseCaseSound on channel 1, then `FADE_SOUND_VOLUME 1 0.2 0.5` and `FADE_SOUND_PAN
    /// 1 0.8 0.5`.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenASoundIsFadedAndPanned
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
        public IEnumerator ThenItReachesItsNewVolumeAndPan()
        {
            yield return UseCaseGame.Enter(UseCaseField.AUDIO, 10);
            yield return UseCaseAudio.Silence();

            yield return m_Input.Talk();

            yield return Wait.Until(() => UseCaseAudio.Sound("UseCaseSound") != null && UseCaseAudio.IsNear(UseCaseAudio.Sound("UseCaseSound").volume, 0.2f) && UseCaseAudio.IsNear(UseCaseAudio.Sound("UseCaseSound").panStereo, 0.8f), 2.5f, "the sound at 0.2, panned 0.8 right");
        }
    }
}
