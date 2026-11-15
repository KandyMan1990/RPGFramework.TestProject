using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;

namespace Test.PlayModeTests.Flow
{
    /// <summary>
    /// The game played round its loop, as a player would: from UseCaseBattleWon's spot in UseCaseSystem, the menu opened
    /// over the field and closed, a battle won and the field back, one lost to the game-over field and Confirm there to
    /// the title, and New Game from the title into the game's starting field.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenAPlaythroughGoesRoundTheLoop
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
        public IEnumerator ThenItEndsInANewGame()
        {
            yield return UseCaseGame.Enter(UseCaseField.SYSTEM, 4);

            InputAction confirm = UseCaseGame.Action(UseCaseProject.CONFIRM);

            yield return m_Input.Tap(m_Input.Keyboard.qKey);
            yield return Wait.Until(() => UseCaseGame.IsMenuOpen, 5f, "Menu opened the menu over the field");

            Assert.That(UseCaseField.Loaded(), Is.Not.Null, "the field there beneath the menu");
            Assert.That(Time.timeScale, Is.EqualTo(0f), "the field frozen beneath the menu");

            yield return UseCaseGame.CloseMenu(m_Input);
            yield return Wait.Until(() => confirm.enabled, 5f, "the field took the player's input again");

            Assert.That(Time.timeScale, Is.EqualTo(1f), "the field running again");

            yield return m_Input.Talk();
            yield return Wait.Until(() => UseCaseGame.Scene == UseCaseGame.BATTLE_SCENE, 10f, "the battle started");
            yield return Wait.Until(() => UseCaseGame.Scene == UseCaseGame.FIELD_SCENE && UseCaseField.Loaded() != null && confirm.enabled, 15f, "the battle won and the field took the player back");
            yield return m_Input.Talk();
            yield return Wait.Until(() => UseCaseGame.Scene == UseCaseGame.BATTLE_SCENE, 10f, "the second battle started");

            float end = Time.realtimeSinceStartup + 10f;

            while (UseCaseGame.Scene == UseCaseGame.BATTLE_SCENE)
            {
                Assert.That(Time.realtimeSinceStartup, Is.LessThan(end), "the battle ended within ten seconds of Cancel");

                yield return m_Input.Tap(m_Input.Keyboard.escapeKey);
                yield return Wait.Seconds(0.25f);
            }

            yield return Wait.Until(() => UseCaseField.IsLoaded(UseCaseProject.GAME_OVER_FIELD), 15f, "the battle lost and the game-over field loaded");

            end = Time.realtimeSinceStartup + 15f;

            while (UseCaseGame.Scene != UseCaseGame.TITLE_SCENE)
            {
                Assert.That(Time.realtimeSinceStartup, Is.LessThan(end), "Confirm on the game-over field opened the title within fifteen seconds");

                yield return m_Input.Talk();
                yield return Wait.Seconds(0.25f);
            }

            yield return UseCaseMenu.ReachTitle();

            UseCaseMenu.Submit();

            yield return Wait.Until(() => UseCaseField.IsLoaded(UseCaseProject.STARTING_FIELD) && confirm.enabled, 15f, "New Game began a playthrough in the starting field");
        }
    }
}
