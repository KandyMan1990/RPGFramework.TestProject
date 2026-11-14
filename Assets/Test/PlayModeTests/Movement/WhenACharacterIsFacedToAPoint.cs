using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Test.PlayModeTests.Movement
{
    /// <summary>
    /// UseCaseFacesPoint: talked to, `SET_DIRECTION_TO_FACE_POSITION` three to its right and `GET_ENTITY_DIRECTION` of itself.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenACharacterIsFacedToAPoint
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
        public IEnumerator ThenItFacesThePoint()
        {
            yield return UseCaseGame.Enter(UseCaseField.MOVEMENT, 15);

            Transform actor = UseCaseMovement.Body("UseCaseFacesPoint");

            yield return m_Input.Talk();
            yield return Wait.Until(() => UseCaseMovement.Faces(UseCaseGame.ReadFloat(UseCaseProject.STAND_DIRECTION), UseCaseProject.FACING_RIGHT), 2f, "the direction read back as facing right");

            Assert.That(UseCaseMovement.Faces(actor.eulerAngles.y, 90f), "facing the point");
        }
    }
}
