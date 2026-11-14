using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Test.PlayModeTests.Movement
{
    /// <summary>
    /// UseCaseJump: talked to, `JUMP_ENTITY_AND_WAIT` two to its right over half a second and `ADD_MONEY 1`.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenACharacterJumps
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
        public IEnumerator ThenItLandsAtThePointAndTheScriptGoesOn()
        {
            yield return UseCaseGame.Enter(UseCaseField.MOVEMENT, 10);

            Transform actor  = UseCaseMovement.Body("UseCaseJump");
            Vector3   target = actor.position + Vector3.right * 2f;
            uint      before = UseCaseMovement.Money;

            yield return m_Input.Talk();
            yield return Wait.Seconds(0.2f);

            Assert.That(UseCaseMovement.Money, Is.EqualTo(before), "the script went on before it landed");

            yield return Wait.Until(() => UseCaseMovement.Money == before + 1, 3f, "the script went on once it landed");

            Assert.That(UseCaseMovement.Apart(actor, target), Is.LessThan(0.05f), "landed at the point");
        }
    }
}
