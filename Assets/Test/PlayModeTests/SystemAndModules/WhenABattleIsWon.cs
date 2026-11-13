using System.Collections;
using NUnit.Framework;
using RPGFramework.Field;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;

namespace Test.PlayModeTests.SystemAndModules
{
    /// <summary>
    /// UseCaseBattleWon: talked to, `START_BATTLE` without fanfare or spoils, then `GET_LAST_BATTLE_RESULT`; the
    /// placeholder battle wins after three seconds.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenABattleIsWon
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
        public IEnumerator ThenTheScriptReadsAVictory()
        {
            yield return UseCaseGame.Enter(UseCaseField.SYSTEM, 4);

            yield return m_Input.Talk();

            yield return Wait.Until(() => UseCaseGame.Scene == UseCaseGame.BATTLE_SCENE, 10f, "the battle started");
            yield return Wait.Until(() => UseCaseGame.Scene == UseCaseGame.FIELD_SCENE && UseCaseField.Loaded() != null, 15f, "the battle was won and the field came back");
            yield return Wait.Until(() => UseCaseGame.ReadByte(UseCaseProject.BATTLE_RESULT) == 4, 5f, "the script read a victory, 4");
        }
    }
}
