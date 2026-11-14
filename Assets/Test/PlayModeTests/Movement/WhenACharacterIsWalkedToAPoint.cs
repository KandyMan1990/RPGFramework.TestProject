using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Test.PlayModeTests.Movement
{
    /// <summary>
    /// UseCaseMoveAndWait: talked to, `MOVE_ENTITY_AND_WAIT` three to its right and `ADD_MONEY 1`.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenACharacterIsWalkedToAPoint
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
        public IEnumerator ThenItArrivesAndTheScriptGoesOn()
        {
            yield return UseCaseGame.Enter(UseCaseField.MOVEMENT, 0);

            Transform actor  = UseCaseMovement.Body("UseCaseMoveAndWait");
            Vector3   target = actor.position + Vector3.right * 3f;
            uint      before = UseCaseMovement.Money;

            yield return m_Input.Talk();
            yield return Wait.Seconds(0.2f);

            Assert.That(UseCaseMovement.Money, Is.EqualTo(before), "the script went on before it arrived");

            yield return Wait.Until(() => UseCaseMovement.Money == before + 1, 5f, "the script went on once it arrived");

            Assert.That(UseCaseMovement.Apart(actor, target), Is.LessThan(0.15f), "at the point");

            Assert.That(UseCaseMovement.Faces(actor.eulerAngles.y, 90f), "facing the way it went");
        }
    }
}
