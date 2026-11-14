using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Test.PlayModeTests.Audio
{
    /// <summary>
    /// UseCaseMusicPlayed: talked to, `PLAY_MUSIC UseCaseSong First`, the state with only its first stem.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenMusicIsPlayed
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
        public IEnumerator ThenItsFirstStemSoundsAndTheSecondDoesNot()
        {
            yield return UseCaseGame.Enter(UseCaseField.AUDIO, 0);
            yield return UseCaseAudio.Silence();

            yield return m_Input.Talk();

            yield return Wait.Until(() => UseCaseAudio.MusicLevel("UseCaseSongFirst") > 0.5f && UseCaseAudio.MusicLevel("UseCaseSongSecond") < UseCaseAudio.CLOSE, 5f, "the first stem sounding, the second silent");
        }
    }
}
