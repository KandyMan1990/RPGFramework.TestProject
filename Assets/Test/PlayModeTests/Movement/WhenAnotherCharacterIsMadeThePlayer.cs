using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Test.PlayModeTests.Movement
{
    /// <summary>
    /// UseCaseTakesControl: talked to, `SET_PLAYER_ENTITY`, then right held: it walks, and `Player0` stays.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenAnotherCharacterIsMadeThePlayer
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
        public IEnumerator ThenTheKeysMoveIt()
        {
            yield return UseCaseGame.Enter(UseCaseField.MOVEMENT, 26);

            Transform actor  = UseCaseMovement.Body("UseCaseTakesControl");
            Transform player = UseCaseMovement.Body("Player0");
            Vector3   start  = actor.position;
            Vector3   stood  = player.position;

            yield return m_Input.Talk();
            yield return Wait.Seconds(0.2f);

            m_Input.Hold(m_Input.Keyboard.dKey);

            yield return Wait.Until(() => UseCaseMovement.Apart(actor, start) > 1f, 3f, "the new player walked");

            m_Input.Release(m_Input.Keyboard.dKey);

            Assert.That(UseCaseMovement.Apart(player, stood), Is.LessThan(0.05f), "Player0 walked too");
        }
    }
}
