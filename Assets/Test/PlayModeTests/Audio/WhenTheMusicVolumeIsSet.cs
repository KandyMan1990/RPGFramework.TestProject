using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Test.PlayModeTests.Audio
{
    /// <summary>
    /// UseCaseMusicVolume: talked to, UseCaseSong, then `SET_MUSIC_VOLUME 0.5`.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenTheMusicVolumeIsSet
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
        public IEnumerator ThenItPlaysAtHalf()
        {
            yield return UseCaseGame.Enter(UseCaseField.AUDIO, 3);
            yield return UseCaseAudio.Silence();

            yield return m_Input.Talk();

            yield return Wait.Until(() => UseCaseAudio.IsNear(UseCaseGame.ReadFloat("SongVolume"), 0.5f) && UseCaseAudio.IsNear(UseCaseAudio.MusicLevel("UseCaseSongFirst"), 0.5f), 5f, "the song at half its volume");
        }
    }
}
