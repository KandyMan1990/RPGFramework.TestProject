using System.Collections;
using NUnit.Framework;
using RPGFramework.Field;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;

namespace Test.PlayModeTests.SystemAndModules
{
    /// <summary>
    /// UseCaseSaving: talked to, `SAVE_ACCESSIBILITY true`, in a field that starts at the map's default, saving
    /// blocked.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenSavingIsAllowed
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
        public IEnumerator ThenThePartyMenuCanSave()
        {
            yield return UseCaseGame.Enter(UseCaseField.SYSTEM, 8);

            Assert.That(UseCaseGame.ReadBool("SaveEnabled"), Is.False, "saving was allowed before the use case");

            yield return m_Input.Talk();

            yield return Wait.Until(() => UseCaseGame.ReadBool("SaveEnabled"), 5f, "saving allowed");
        }
    }
}
