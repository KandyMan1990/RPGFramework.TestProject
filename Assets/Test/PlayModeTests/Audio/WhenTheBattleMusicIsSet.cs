using System.Collections;
using NUnit.Framework;
using RPGFramework.Hashing;
using UnityEngine;
using UnityEngine.TestTools;

namespace Test.PlayModeTests.Audio
{
    /// <summary>
    /// UseCaseBattleMusic: talked to, `SET_BATTLE_MUSIC UseCaseSongTwo`.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenTheBattleMusicIsSet
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
        public IEnumerator ThenBattlesWillPlayIt()
        {
            yield return UseCaseGame.Enter(UseCaseField.AUDIO, 7);
            yield return UseCaseAudio.Silence();

            yield return m_Input.Talk();

            yield return Wait.Until(() => UseCaseGame.ReadUlong("BattleMusic") == Fnv1a64.Hash("UseCaseSongTwo"), 5f, "the battle music kept as UseCaseSongTwo");
        }
    }
}
