using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Test.PlayModeTests.Movement
{
    /// <summary>
    /// UseCaseInputLocked: talked to, `LOCK_INPUT true`, `WAIT_SECONDS 1.5` and `LOCK_INPUT false`, right held throughout.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenThePlayersInputIsLocked
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
        public IEnumerator ThenTheKeysDoNothingUntilItIsUnlocked()
        {
            yield return UseCaseGame.Enter(UseCaseField.MOVEMENT, 27);

            Transform player = UseCaseMovement.Body("Player0");
            Vector3   stood  = player.position;

            yield return m_Input.Talk();
            yield return Wait.Seconds(0.1f);

            m_Input.Hold(m_Input.Keyboard.dKey);

            yield return Wait.Seconds(1f);

            Assert.That(UseCaseMovement.Apart(player, stood), Is.LessThan(0.05f), "the player walked while locked");

            yield return Wait.Until(() => UseCaseMovement.Apart(player, stood) > 1f, 3f, "the player walked once unlocked");

            m_Input.Release(m_Input.Keyboard.dKey);
        }
    }
}
