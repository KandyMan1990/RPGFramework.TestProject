using System.Collections;
using NUnit.Framework;
using RPGFramework.Field;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;

namespace Test.PlayModeTests.SystemAndModules
{
    /// <summary>
    /// UseCaseGatewaysOff: talked to, `GATEWAY_TRIGGER_ACTIVATION false`; the player then walks through the gateway
    /// and stays in the field.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenGatewaysAreTurnedOff
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
        public IEnumerator ThenWalkingIntoOneStaysInTheField()
        {
            yield return UseCaseGame.Enter(UseCaseField.SYSTEM, 2);

            FieldEntities field  = UseCaseField.Loaded();
            float         beyond = UseCaseField.PositionOf(field, "UseCaseGateway1").x + 1.5f;

            yield return m_Input.Talk();

            m_Input.Hold(m_Input.Keyboard.dKey);

            yield return Wait.Until(() => field == null || UseCaseField.PositionOf(field, "Player0").x > beyond, 10f, "the player walked through the gateway");

            m_Input.Release(m_Input.Keyboard.dKey);

            Assert.That(field != null, "the field was left through a gateway turned off");
        }
    }
}
