using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Test.PlayModeTests.Audio
{
    /// <summary>
    /// UseCaseMusicFaded: talked to, UseCaseSong, `FADE_MUSIC_VOLUME 0.25 1`, `WAIT_FOR_MUSIC_VOLUME`, then `ADD_MONEY
    /// 1`.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenTheMusicIsFadedAndWaitedFor
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
        public IEnumerator ThenTheScriptGoesOnOnceItIsAtAQuarter()
        {
            yield return UseCaseGame.Enter(UseCaseField.AUDIO, 4);
            yield return UseCaseAudio.Silence();

            uint before = UseCaseGame.ReadUint("Money");

            yield return m_Input.Talk();
            yield return Wait.Seconds(0.3f);

            Assert.That(UseCaseGame.ReadUint("Money"), Is.EqualTo(before), "the script went on before the fade ended");

            yield return Wait.Until(() => UseCaseGame.ReadUint("Money") == before + 1, 4f, "the script went on once the fade ended");

            Assert.That(UseCaseAudio.IsNear(UseCaseAudio.MusicLevel("UseCaseSongFirst"), 0.25f), "the song at a quarter of its volume");
        }
    }
}
