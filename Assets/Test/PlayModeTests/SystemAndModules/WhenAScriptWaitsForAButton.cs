using System.Collections;
using NUnit.Framework;
using RPGFramework.Field;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;

namespace Test.PlayModeTests.SystemAndModules
{
    /// <summary>
    /// UseCaseWaitForButton: talked to, `WAIT_FOR_BUTTON Confirm`, then `ADD_MONEY 1`.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenAScriptWaitsForAButton
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
        public IEnumerator ThenItGoesOnOnlyOnceTheButtonIsPressed()
        {
            yield return UseCaseGame.Enter(UseCaseField.SYSTEM, 14);

            uint before = UseCaseGame.ReadUint("Money");

            yield return m_Input.Talk();
            yield return Wait.Seconds(1f);

            Assert.That(UseCaseGame.ReadUint("Money"), Is.EqualTo(before), "the script went on before the button");

            yield return m_Input.Talk();

            yield return Wait.Until(() => UseCaseGame.ReadUint("Money") == before + 1, 5f, "the script went on once the button was pressed");
        }
    }
}
