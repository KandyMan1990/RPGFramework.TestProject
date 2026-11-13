using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.TestTools;

namespace Test.PlayModeTests.SystemAndModules
{
    /// <summary>
    /// UseCaseVibration: talked to, `SET_VIBRATION 0.25 0.75 1.0`, a gamepad of the test's own the current one; the
    /// motors are watched in the commands the Input System sends the gamepad.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenTheGamepadIsRumbled
    {
        // A rumble command's code; its payload is the two motors' speeds, low then high.
        private static readonly FourCC m_Rumble = new FourCC('R', 'M', 'B', 'L');

        private UseCaseInput m_Input;
        private Gamepad      m_Gamepad;
        private bool         m_Rumbled;

        [SetUp]
        public unsafe void SetUp()
        {
            m_Input   = new UseCaseInput();
            m_Gamepad = InputSystem.AddDevice<Gamepad>();
            m_Rumbled = false;

            m_Gamepad.MakeCurrent();
            InputSystem.onDeviceCommand += OnDeviceCommand;
        }

        [TearDown]
        public unsafe void TearDown()
        {
            InputSystem.onDeviceCommand -= OnDeviceCommand;
            m_Input.TearDown();
        }

        [UnityTest]
        public IEnumerator ThenItsMotorsRunAsAsked()
        {
            yield return UseCaseGame.Enter(UseCaseField.SYSTEM, 13);
            yield return m_Input.Talk();

            yield return Wait.Until(() => m_Rumbled, 5f, "the gamepad's motors ran at 0.25 and 0.75");
        }

        private unsafe long? OnDeviceCommand(InputDevice device, InputDeviceCommand* command)
        {
            if (device == m_Gamepad && command->type == m_Rumble)
            {
                float* speeds = (float*)command->payloadPtr;

                m_Rumbled |= Mathf.Approximately(speeds[0], 0.25f) && Mathf.Approximately(speeds[1], 0.75f);
            }

            return null;
        }
    }
}
