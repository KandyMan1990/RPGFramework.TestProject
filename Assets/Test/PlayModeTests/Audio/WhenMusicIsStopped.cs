using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Test.PlayModeTests.Audio
{
    /// <summary>
    /// UseCaseMusicStopped: talked to, UseCaseSong, `IS_MUSIC_PLAYING`, a second, then `STOP_MUSIC`.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenMusicIsStopped
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
        public IEnumerator ThenItWasReadAsPlayingAndThenStops()
        {
            yield return UseCaseGame.Enter(UseCaseField.AUDIO, 2);
            yield return UseCaseAudio.Silence();

            yield return m_Input.Talk();

            yield return Wait.Until(() => UseCaseGame.ReadBool(UseCaseProject.MUSIC_PLAYING), 5f, "the script read the music as playing");
            yield return Wait.Until(() => UseCaseAudio.MusicLevel("UseCaseSongFirst") > 0.5f, 2f, "the music playing");
            yield return Wait.Until(() => UseCaseAudio.MusicLevel("UseCaseSongFirst") == 0f, 3f, "the music stopped");
        }
    }
}
