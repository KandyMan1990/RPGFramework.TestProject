using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Test.PlayModeTests.Movement
{
    /// <summary>
    /// UseCaseMovementSpeed: talked to, `SET_MOVEMENT_SPEED 9`, `MOVE_ENTITY_AND_WAIT` three to its right and `ADD_MONEY 1`: there in
    /// a third of a second, where its own speed of three takes one.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenTheMovementSpeedIsRaised
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
        public IEnumerator ThenItArrivesSooner()
        {
            yield return UseCaseGame.Enter(UseCaseField.MOVEMENT, 9);

            Transform actor  = UseCaseMovement.Body("UseCaseMovementSpeed");
            Vector3   target = actor.position + Vector3.right * 3f;
            uint      before = UseCaseMovement.Money;

            yield return m_Input.Talk();

            float started = Time.realtimeSinceStartup;

            yield return Wait.Until(() => UseCaseMovement.Money == before + 1, 3f, "the script went on once it arrived");

            Assert.That(Time.realtimeSinceStartup - started, Is.LessThan(0.75f), "it arrived at the speed it was given");
            Assert.That(UseCaseMovement.Apart(actor, target), Is.LessThan(0.15f), "at the point");
        }
    }
}
