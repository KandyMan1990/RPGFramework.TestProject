using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Test.PlayModeTests.Animation
{
    /// <summary>
    /// UseCaseClimb: talked to, `CLIMB_LADDER_AND_WAIT` up from where it stands, three up and off one on, and `ADD_MONEY 1`, on
    /// its ladder animations `GetOn`, `Climb` and `GetOff`.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenACharacterClimbsALadder
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
        public IEnumerator ThenItGetsOffAtTheTop()
        {
            yield return UseCaseGame.Enter(UseCaseField.ANIMATION, 26);

            Transform actor  = UseCaseMovement.Body("UseCaseClimb");
            uint      before = UseCaseMovement.Money;

            Vector3 finish = actor.position + Vector3.up * 3f + UseCaseProject.Ahead;

            yield return m_Input.Talk();
            yield return Wait.Until(() => UseCaseAnimation.Plays(actor, "Climb"), 3f, "climbing");

            Assert.That(UseCaseMovement.Money, Is.EqualTo(before), "the script went on before it got off");

            yield return Wait.Until(() => UseCaseMovement.Money == before + 1, 8f, "the script went on once it got off");

            Assert.That((actor.position - finish).magnitude, Is.LessThan(0.1f), "off the ladder at the top");
        }
    }
}
