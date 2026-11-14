using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Test.PlayModeTests.Audio
{
    /// <summary>
    /// UseCaseAllSounds: talked to, UseCaseSound at full volume, then `SET_ALL_SOUND_VOLUME 0.5`.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenEverySoundIsSet
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
        public IEnumerator ThenTheSoundPlaysAtHalf()
        {
            yield return UseCaseGame.Enter(UseCaseField.AUDIO, 11);
            yield return UseCaseAudio.Silence();

            yield return m_Input.Talk();

            yield return Wait.Until(() => UseCaseAudio.Sound("UseCaseSound") != null && UseCaseAudio.IsNear(UseCaseAudio.Sound("UseCaseSound").volume, 0.5f), 2.5f, "the sound at half volume");
        }
    }
}
