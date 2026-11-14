using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Test.PlayModeTests.Audio
{
    /// <summary>
    /// UseCaseReverb: talked to, `SET_REVERB_PRESET 3` and `SET_REVERB_VOLUME 0.5`, the mixer's `ReverbPreset` and
    /// `ReverbDepth`.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenTheReverbIsSet
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
        public IEnumerator ThenTheMixerTakesItsPresetAndDepth()
        {
            yield return UseCaseGame.Enter(UseCaseField.AUDIO, 13);
            yield return UseCaseAudio.Silence();

            float depth = UseCaseAudio.Mixer("ReverbDepth");

            Assert.That(UseCaseAudio.Mixer("ReverbPreset"), Is.Not.EqualTo(3f), "the reverb was preset 3 already");

            yield return m_Input.Talk();

            yield return Wait.Until(() => Mathf.Approximately(UseCaseAudio.Mixer("ReverbPreset"), 3f) && !Mathf.Approximately(UseCaseAudio.Mixer("ReverbDepth"), depth), 5f, "the reverb at preset 3 and a new depth");
        }
    }
}
