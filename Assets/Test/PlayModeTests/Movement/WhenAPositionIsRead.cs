using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Test.PlayModeTests.Movement
{
    /// <summary>
    /// UseCasePositionRead: talked to, `GET_ENTITY_POSITION` of itself into the project's three stand variables.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenAPositionIsRead
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
        public IEnumerator ThenTheVariablesHoldIt()
        {
            yield return UseCaseGame.Enter(UseCaseField.MOVEMENT, 17);

            Transform actor = UseCaseMovement.Body("UseCasePositionRead");

            yield return m_Input.Talk();
            yield return Wait.Until(() => (Stored() - actor.position).magnitude < 0.01f, 2f, "where it stands stored");
        }

        private static Vector3 Stored()
        {
            return new Vector3(UseCaseGame.ReadFloat(UseCaseProject.STAND_X), UseCaseGame.ReadFloat(UseCaseProject.STAND_Y), UseCaseGame.ReadFloat(UseCaseProject.STAND_Z));
        }
    }
}
