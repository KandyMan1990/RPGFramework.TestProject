using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace Test.PlayModeTests
{
    /// <summary>A keyboard of the test's own, through the Input System's test fixture, pressed as a player presses one.</summary>
    internal sealed class UseCaseInput
    {
        private readonly InputTestFixture m_Fixture;
        private readonly Keyboard         m_Keyboard;

        internal UseCaseInput()
        {
            m_Fixture = new InputTestFixture();
            m_Fixture.Setup();

            m_Keyboard = InputSystem.AddDevice<Keyboard>();
        }

        internal InputTestFixture Fixture  => m_Fixture;
        internal Keyboard         Keyboard => m_Keyboard;

        internal void TearDown()
        {
            m_Fixture.TearDown();
        }

        internal void Hold(ButtonControl key)
        {
            m_Fixture.Press(key);
        }

        internal void Release(ButtonControl key)
        {
            m_Fixture.Release(key);
        }

        /// <summary>Confirm pressed and let go, as a player talks to what they face.</summary>
        internal IEnumerator Talk()
        {
            return Tap(m_Keyboard.eKey);
        }

        internal IEnumerator Tap(ButtonControl key)
        {
            m_Fixture.Press(key);

            yield return null;
            yield return null;

            m_Fixture.Release(key);

            yield return null;
        }
    }
}
