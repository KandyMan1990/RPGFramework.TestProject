using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Test.PlayModeTests.Movement
{
    /// <summary>
    /// UseCasePlaced: talked to, `SET_ENTITY_POSITION` two to its right.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenACharacterIsPlaced
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
        public IEnumerator ThenItIsThereAtOnce()
        {
            yield return UseCaseGame.Enter(UseCaseField.MOVEMENT, 11);

            Transform actor  = UseCaseMovement.Body("UseCasePlaced");
            Vector3   target = actor.position + Vector3.right * 2f;

            yield return m_Input.Talk();
            yield return Wait.Until(() => UseCaseMovement.Apart(actor, target) < 0.01f, 0.5f, "there at once");
        }
    }
}
