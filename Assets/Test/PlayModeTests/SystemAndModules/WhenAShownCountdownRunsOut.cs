using System.Collections;
using NUnit.Framework;
using RPGFramework.Field;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;

namespace Test.PlayModeTests.SystemAndModules
{
    /// <summary>
    /// UseCaseCountdownRunsOut: talked to, `SET_COUNTDOWN_TIMER 1` and `SHOW_COUNTDOWN_TIMER`; a shown countdown at
    /// zero ends the game.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenAShownCountdownRunsOut
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
        public IEnumerator ThenTheGameOverFieldLoads()
        {
            yield return UseCaseGame.Enter(UseCaseField.SYSTEM, 11);

            yield return m_Input.Talk();

            yield return Wait.Until(() => UseCaseGame.ReadBool("CountdownShown"), 5f, "the countdown shown");
            yield return Wait.Until(() => UseCaseField.IsLoaded(UseCaseProject.GAME_OVER_FIELD), 10f, "the countdown ran out and the game-over field loaded");
        }
    }
}
