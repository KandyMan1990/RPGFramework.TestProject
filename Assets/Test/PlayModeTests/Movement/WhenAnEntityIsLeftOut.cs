using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Test.PlayModeTests.Movement
{
    /// <summary>
    /// UseCaseLeftOut: its init, `ENTITY_IN_FIELD false`: its body switched off once every init has run.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenAnEntityIsLeftOut
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
        public IEnumerator ThenItsBodyIsNotInTheField()
        {
            yield return UseCaseGame.Enter(UseCaseField.MOVEMENT, 20);

            Transform body = UseCaseMovement.Body("UseCaseLeftOut");

            yield return Wait.Until(() => !body.gameObject.activeInHierarchy, 2f, "its body left out");
        }
    }
}
