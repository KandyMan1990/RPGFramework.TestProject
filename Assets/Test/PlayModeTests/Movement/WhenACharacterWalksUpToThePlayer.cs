using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Test.PlayModeTests.Movement
{
    /// <summary>
    /// UseCaseWalkToPlayerAndWait: talked to, `SET_ENTITY_POSITION` four to its right, `MOVE_ENTITY_TO_ENTITY_AND_WAIT 0 1.5` and `ADD_MONEY 1`.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenACharacterWalksUpToThePlayer
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
        public IEnumerator ThenItStopsShortOfThePlayer()
        {
            yield return UseCaseGame.Enter(UseCaseField.MOVEMENT, 6);

            Transform actor  = UseCaseMovement.Body("UseCaseWalkToPlayerAndWait");
            Transform player = UseCaseMovement.Body("Player0");
            Vector3   placed = actor.position + Vector3.right * 4f;
            uint      before = UseCaseMovement.Money;

            yield return m_Input.Talk();
            yield return Wait.Until(() => UseCaseMovement.Money == before + 1, 6f, "the script went on once it arrived");

            Assert.That(UseCaseMovement.Apart(actor, placed), Is.GreaterThan(1f), "it walked from where it was put");
            Assert.That(UseCaseMovement.Apart(actor, player), Is.InRange(0.9f, 1.7f), "it stopped short of the player");
        }
    }
}
