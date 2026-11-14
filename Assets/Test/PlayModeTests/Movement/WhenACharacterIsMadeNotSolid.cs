using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Test.PlayModeTests.Movement
{
    /// <summary>
    /// UseCaseWalkThrough: talked to, `SOLIDITY false` and `ADD_MONEY 1`, then walked into from behind.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenACharacterIsMadeNotSolid
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
        public IEnumerator ThenThePlayerWalksThroughIt()
        {
            yield return UseCaseGame.Enter(UseCaseField.MOVEMENT, 23);

            Transform actor  = UseCaseMovement.Body("UseCaseWalkThrough");
            Transform player = UseCaseMovement.Body("Player0");
            uint      before = UseCaseMovement.Money;

            yield return m_Input.Talk();
            yield return Wait.Until(() => UseCaseMovement.Money == before + 1, 2f, "talked to");

            m_Input.Hold(m_Input.Keyboard.wKey);

            yield return Wait.Until(() => UseCaseMovement.Ahead(player) > UseCaseMovement.Ahead(actor) + 0.5f, 3f, "the player walked through it");

            m_Input.Release(m_Input.Keyboard.wKey);
        }
    }
}
