using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Test.PlayModeTests.Audio
{
    /// <summary>
    /// UseCaseMusicFadedFrom: talked to, UseCaseSong, then `FADE_MUSIC_VOLUME_FROM 0 0.75 1`.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenTheMusicIsFadedFromSilence
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
        public IEnumerator ThenItRisesFromQuietToThreeQuarters()
        {
            yield return UseCaseGame.Enter(UseCaseField.AUDIO, 5);
            yield return UseCaseAudio.Silence();

            yield return m_Input.Talk();

            yield return Wait.Until(() => UseCaseAudio.MusicLevel("UseCaseSongFirst") < 0.5f, 2f, "the song starting quiet");
            yield return Wait.Until(() => UseCaseAudio.IsNear(UseCaseAudio.MusicLevel("UseCaseSongFirst"), 0.75f), 4f, "the song risen to three quarters");
        }
    }
}
