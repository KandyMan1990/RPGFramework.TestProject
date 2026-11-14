using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Test.PlayModeTests.Movement
{
    /// <summary>
    /// UseCaseFacesPlayer: talked to, `SET_ENTITY_ROTATION` back to none, `SET_DIRECTION_TO_FACE_ENTITY 0` and
    /// `GET_ENTITY_DIRECTION` of itself: turned round to the player behind it.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenACharacterIsFacedToThePlayer
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
        public IEnumerator ThenItFacesThePlayer()
        {
            yield return UseCaseGame.Enter(UseCaseField.MOVEMENT, 14);

            Transform actor = UseCaseMovement.Body("UseCaseFacesPlayer");

            yield return m_Input.Talk();
            yield return Wait.Until(() => UseCaseMovement.Faces(UseCaseGame.ReadFloat(UseCaseProject.STAND_DIRECTION), 180f), 2f, "the direction read back as facing the player");

            Assert.That(UseCaseMovement.Faces(actor.eulerAngles.y, 180f), "facing the player");
        }
    }
}
