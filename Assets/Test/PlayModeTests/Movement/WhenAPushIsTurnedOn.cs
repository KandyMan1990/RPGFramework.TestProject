using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Test.PlayModeTests.Movement
{
    /// <summary>
    /// UseCasePushed: talked to, `PUSH_SCRIPT_ACTIVATION true` and `ADD_MONEY 1`, then walked into: its push script,
    /// `ADD_MONEY 1`.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenAPushIsTurnedOn
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
        public IEnumerator ThenWalkingIntoItRunsItsPushScript()
        {
            yield return UseCaseGame.Enter(UseCaseField.MOVEMENT, 24);

            uint before = UseCaseMovement.Money;

            yield return m_Input.Talk();
            yield return Wait.Until(() => UseCaseMovement.Money == before + 1, 2f, "talked to");

            m_Input.Hold(m_Input.Keyboard.wKey);

            yield return Wait.Until(() => UseCaseMovement.Money >= before + 2, 3f, "its push script run");

            m_Input.Release(m_Input.Keyboard.wKey);
        }
    }
}
