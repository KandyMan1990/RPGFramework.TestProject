using System.Collections;
using NUnit.Framework;
using RPGFramework.Field;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;

namespace Test.PlayModeTests.SystemAndModules
{
    /// <summary>
    /// UseCaseRandomBattles: talked to, `SET_BATTLE_MODE` without fanfare or spoils and `SET_RANDOM_ENCOUNTERS_ENABLED
    /// true`, in a field whose random battles are at the highest rate and were turned off as it loaded.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenRandomBattlesAreTurnedOn
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
        public IEnumerator ThenWalkingMeetsOne()
        {
            yield return UseCaseGame.Enter(UseCaseField.SYSTEM, 6);

            yield return m_Input.Talk();

            m_Input.Hold(m_Input.Keyboard.aKey);

            yield return Wait.Until(() => UseCaseGame.Scene == UseCaseGame.BATTLE_SCENE, 10f, "a random battle started");

            m_Input.Release(m_Input.Keyboard.aKey);

            yield return Wait.Until(() => UseCaseGame.Scene == UseCaseGame.FIELD_SCENE && UseCaseField.Loaded() != null, 15f, "the battle was won and the field came back");
        }
    }
}
