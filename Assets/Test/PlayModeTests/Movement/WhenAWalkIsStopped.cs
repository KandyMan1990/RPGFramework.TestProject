using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Test.PlayModeTests.Movement
{
    /// <summary>
    /// UseCaseStopped: talked to, `MOVE_ENTITY` three to its right, `WAIT_SECONDS 0.4`, `STOP_MOVEMENT` and `ADD_MONEY 1`:
    /// short of the point, a tile field finishing the step it is on.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenAWalkIsStopped
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
        public IEnumerator ThenItStaysWhereItStopped()
        {
            yield return UseCaseGame.Enter(UseCaseField.MOVEMENT, 8);

            Transform actor  = UseCaseMovement.Body("UseCaseStopped");
            Vector3   start  = actor.position;
            Vector3   target = start + Vector3.right * 3f;
            uint      before = UseCaseMovement.Money;

            yield return m_Input.Talk();
            yield return Wait.Until(() => UseCaseMovement.Money == before + 1, 3f, "the script went on once it stopped");
            yield return Wait.Seconds(0.4f);

            Vector3 stopped = actor.position;

            Assert.That(UseCaseMovement.Apart(actor, start), Is.InRange(0.3f, 2.6f), "it walked some way");
            Assert.That(UseCaseMovement.Apart(actor, target), Is.GreaterThan(0.3f), "it stopped short of the point");

            yield return Wait.Seconds(0.5f);

            Assert.That(UseCaseMovement.Apart(actor, stopped), Is.LessThan(0.01f), "still where it stopped");
        }
    }
}
