using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Test.PlayModeTests.Movement
{
    /// <summary>
    /// UseCaseRotated: talked to, `SET_ENTITY_ROTATION` a quarter turn and `GET_ENTITY_DIRECTION` of itself.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenACharacterIsTurned
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
        public IEnumerator ThenItFacesThatWay()
        {
            yield return UseCaseGame.Enter(UseCaseField.MOVEMENT, 12);

            Transform actor = UseCaseMovement.Body("UseCaseRotated");

            yield return m_Input.Talk();
            yield return Wait.Until(() => UseCaseMovement.Faces(UseCaseGame.ReadFloat(UseCaseProject.STAND_DIRECTION), 90f), 2f, "the direction read back as a quarter turn");

            Assert.That(UseCaseMovement.Faces(actor.eulerAngles.y, 90f), "facing a quarter turn round");
        }
    }
}
