using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Test.PlayModeTests.Audio
{
    /// <summary>
    /// UseCaseMusicState: talked to, UseCaseSong in its state First, then `MUSIC_STEM_STATE` to Both over half a
    /// second.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenTheStemStateIsSwitched
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
        public IEnumerator ThenBothStemsSound()
        {
            yield return UseCaseGame.Enter(UseCaseField.AUDIO, 1);
            yield return UseCaseAudio.Silence();

            yield return m_Input.Talk();

            yield return Wait.Until(() => UseCaseAudio.MusicLevel("UseCaseSongFirst") > 0.5f && UseCaseAudio.MusicLevel("UseCaseSongSecond") > 0.5f, 5f, "both stems sounding");
        }
    }
}
