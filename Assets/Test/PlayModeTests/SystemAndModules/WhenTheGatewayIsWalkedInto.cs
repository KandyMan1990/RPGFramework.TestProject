using System.Collections;
using NUnit.Framework;
using RPGFramework.Field;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;

namespace Test.PlayModeTests.SystemAndModules
{
    /// <summary>
    /// UseCaseSystem's gateway: the player, entering at spawn point 0, holds right into the gateway, whose script jumps
    /// back into the field at spawn point 1 — input, a collision trigger, a script, a field change and a spawn in one.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenTheGatewayIsWalkedInto
    {
        private const string FIELD = "UseCaseSystem";

        private InputTestFixture m_Input;
        private Keyboard         m_Keyboard;

        [SetUp]
        public void SetUp()
        {
            m_Input = new InputTestFixture();
            m_Input.Setup();

            m_Keyboard = InputSystem.AddDevice<Keyboard>();
        }

        [TearDown]
        public void TearDown()
        {
            m_Input.TearDown();
        }

        [UnityTest]
        public IEnumerator ThenThePlayerArrivesAtTheSpawnPoint()
        {
            yield return UseCaseGame.Enter(FIELD, 0);

            FieldEntities entered = null;

            yield return Wait.Until(() => (entered = UseCaseField.Loaded()) != null, 10f, "the field loaded");

            m_Input.Press(m_Keyboard.dKey);

            yield return Wait.Until(() => entered == null, 10f, "the player walked into the gateway and left the field");

            m_Input.Release(m_Keyboard.dKey);

            FieldEntities arrived = null;

            yield return Wait.Until(() => (arrived = UseCaseField.Loaded()) != null, 10f, "the field loaded again");
            yield return Wait.Until(() => UseCaseField.GroundDistance(UseCaseField.PositionOf(arrived, "Player0"), UseCaseField.PositionOf(arrived, "SpawnPoint1"), Vector3.up) < 0.1f,
                                    5f,
                                    "the player stood at spawn point 1");
        }
    }
}
