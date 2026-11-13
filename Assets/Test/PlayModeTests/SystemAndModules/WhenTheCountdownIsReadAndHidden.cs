using System.Collections;
using NUnit.Framework;
using RPGFramework.Field;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;

namespace Test.PlayModeTests.SystemAndModules
{
    /// <summary>
    /// UseCaseCountdownRead: talked to, three seconds set and shown, one and a half waited, `GET_COUNTDOWN_TIMER` and
    /// `HIDE_COUNTDOWN_TIMER`.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenTheCountdownIsReadAndHidden
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
        public IEnumerator ThenItHeldTheSecondsLeftAndNeverEndsTheGame()
        {
            yield return UseCaseGame.Enter(UseCaseField.SYSTEM, 12);

            yield return m_Input.Talk();

            yield return Wait.Until(() => UseCaseGame.ReadInt(UseCaseProject.COUNTDOWN_LEFT) > 0, 5f, "the script read the countdown");

            Assert.That(UseCaseGame.ReadInt(UseCaseProject.COUNTDOWN_LEFT), Is.InRange(1, 2), "the seconds left after one and a half of three");

            yield return Wait.Until(() => !UseCaseGame.ReadBool("CountdownShown"), 2f, "the countdown hidden");
            yield return Wait.Seconds(4f);

            Assert.That(UseCaseGame.Scene, Is.EqualTo(UseCaseGame.FIELD_SCENE), "a hidden countdown ended the game at zero");
        }
    }
}
