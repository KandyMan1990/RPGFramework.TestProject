using System.Collections;
using NUnit.Framework;
using RPGFramework.Field;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;

namespace Test.PlayModeTests.SystemAndModules
{
    /// <summary>
    /// UseCaseBattleLost: talked to, `SET_BATTLE_MODE` letting a defeat pass, then a battle and
    /// `GET_LAST_BATTLE_RESULT`; Cancel loses the placeholder battle.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenABattleIsLostAndTheModeLetsItPass
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
        public IEnumerator ThenTheFieldGoesOnAndReadsADefeat()
        {
            yield return UseCaseGame.Enter(UseCaseField.SYSTEM, 5);

            yield return m_Input.Talk();

            yield return Wait.Until(() => UseCaseGame.Scene == UseCaseGame.BATTLE_SCENE, 10f, "the battle started");

            // As a player would, until the battle takes it: it listens a moment after its actions are on.
            float end = Time.realtimeSinceStartup + 10f;

            while (UseCaseGame.Scene == UseCaseGame.BATTLE_SCENE)
            {
                Assert.That(Time.realtimeSinceStartup, Is.LessThan(end), "the battle ended within ten seconds of Cancel");

                yield return m_Input.Tap(m_Input.Keyboard.escapeKey);
                yield return Wait.Seconds(0.25f);
            }
            yield return Wait.Until(() => UseCaseGame.Scene == UseCaseGame.FIELD_SCENE && UseCaseField.Loaded() != null, 15f, "the battle was lost and the field came back, not the title");
            yield return Wait.Until(() => UseCaseGame.ReadByte(UseCaseProject.BATTLE_RESULT) == 1, 5f, "the script read a defeat, 1");
        }
    }
}
