using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Test.PlayModeTests.Audio
{
    /// <summary>
    /// UseCaseSoundPlayed: talked to, `PLAY_SOUND UseCaseSound 0.5 -0.5 1`.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenASoundIsPlayed
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
        public IEnumerator ThenItPlaysAtItsVolumeAndPan()
        {
            yield return UseCaseGame.Enter(UseCaseField.AUDIO, 8);
            yield return UseCaseAudio.Silence();

            yield return m_Input.Talk();

            yield return Wait.Until(() => UseCaseAudio.Sound("UseCaseSound") != null && UseCaseAudio.IsNear(UseCaseAudio.Sound("UseCaseSound").volume, 0.5f) && UseCaseAudio.IsNear(UseCaseAudio.Sound("UseCaseSound").panStereo, -0.5f), 5f, "the sound playing at half volume, panned half left");
        }
    }
}
