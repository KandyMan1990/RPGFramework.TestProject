using System.Collections;
using NUnit.Framework;
using RPGFramework.Field;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;

namespace Test.PlayModeTests.SystemAndModules
{
    /// <summary>
    /// UseCaseNoExitFade: talked to, `SET_FIELD_EXIT_FADE false`; the player then walks into the gateway, and the
    /// field is left with nothing covering the screen.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenTheExitFadeIsTurnedOff
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
        public IEnumerator ThenTheScreenIsNotCoveredBeforeTheFieldIsLeft()
        {
            yield return UseCaseGame.Enter(UseCaseField.SYSTEM, 3);

            FieldEntities field   = UseCaseField.Loaded();
            bool          covered = false;

            yield return m_Input.Talk();

            m_Input.Hold(m_Input.Keyboard.dKey);

            yield return Wait.Until(() =>
                                    {
                                        covered |= UseCaseField.IsCovered();
                                        return field == null;
                                    },
                                    10f,
                                    "the player walked into the gateway and left the field");

            m_Input.Release(m_Input.Keyboard.dKey);

            Assert.That(covered, Is.False, "the screen was covered before the field was left");
        }
    }
}
