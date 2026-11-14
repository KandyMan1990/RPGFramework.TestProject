using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Test.PlayModeTests.Movement
{
    /// <summary>
    /// UseCaseTurnsToPlayer: talked to, `SET_ENTITY_ROTATION` back to none, `TURN_TO_ENTITY_AND_WAIT 0 0.5 0`, `GET_ENTITY_DIRECTION`
    /// of itself and `ADD_MONEY 1`.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenACharacterTurnsToThePlayer
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
        public IEnumerator ThenTheScriptGoesOnOnceItHasTurned()
        {
            yield return UseCaseGame.Enter(UseCaseField.MOVEMENT, 16);

            Transform actor  = UseCaseMovement.Body("UseCaseTurnsToPlayer");
            uint      before = UseCaseMovement.Money;

            yield return m_Input.Talk();
            yield return Wait.Seconds(0.2f);

            Assert.That(UseCaseMovement.Money, Is.EqualTo(before), "the script went on before the turn ended");

            yield return Wait.Until(() => UseCaseMovement.Money == before + 1, 3f, "the script went on once the turn ended");

            Assert.That(UseCaseMovement.Faces(UseCaseGame.ReadFloat(UseCaseProject.STAND_DIRECTION), 180f), "the direction read back as facing the player");

            Assert.That(UseCaseMovement.Faces(actor.eulerAngles.y, 180f), "facing the player");
        }
    }
}
