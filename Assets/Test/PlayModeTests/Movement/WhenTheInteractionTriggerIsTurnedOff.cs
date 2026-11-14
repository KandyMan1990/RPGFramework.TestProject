using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Test.PlayModeTests.Movement
{
    /// <summary>
    /// UseCaseInteractionOff: talked to, `ADD_MONEY 1` and `INTERACTION_TRIGGER_ACTIVATION false`.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenTheInteractionTriggerIsTurnedOff
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
        public IEnumerator ThenItCannotBeTalkedToAgain()
        {
            yield return UseCaseGame.Enter(UseCaseField.MOVEMENT, 21);

            uint before = UseCaseMovement.Money;

            yield return m_Input.Talk();
            yield return Wait.Until(() => UseCaseMovement.Money == before + 1, 2f, "talked to");
            yield return m_Input.Talk();
            yield return Wait.Seconds(0.5f);

            Assert.That(UseCaseMovement.Money, Is.EqualTo(before + 1), "talked to again with its trigger off");
        }
    }
}
