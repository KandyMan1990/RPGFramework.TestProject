using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Test.PlayModeTests.Audio
{
    /// <summary>
    /// UseCaseCrossfade: talked to, UseCaseSong, half a second, then `CROSSFADE_MUSIC UseCaseSongTwo Default 1 1`.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenMusicIsCrossfaded
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
        public IEnumerator ThenTheSecondSongPlaysAndTheFirstHasGone()
        {
            yield return UseCaseGame.Enter(UseCaseField.AUDIO, 6);
            yield return UseCaseAudio.Silence();

            yield return m_Input.Talk();

            yield return Wait.Until(() => UseCaseAudio.MusicLevel("UseCaseSongTwo") > 0.9f && UseCaseAudio.MusicLevel("UseCaseSongFirst") == 0f, 6f, "the second song playing, the first gone");
        }
    }
}
