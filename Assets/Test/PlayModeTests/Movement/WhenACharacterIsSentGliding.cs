using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Test.PlayModeTests.Movement
{
    /// <summary>
    /// UseCaseGlide: talked to, `GLIDE_ENTITY` three to its right, `ADD_MONEY 1`, `WAIT_FOR_MOVEMENT` and `ADD_MONEY 1`.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenACharacterIsSentGliding
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
        public IEnumerator ThenTheScriptGoesOnAndThenWaitsForIt()
        {
            yield return UseCaseGame.Enter(UseCaseField.MOVEMENT, 3);

            Transform actor  = UseCaseMovement.Body("UseCaseGlide");
            Vector3   target = actor.position + Vector3.right * 3f;
            uint      before = UseCaseMovement.Money;

            yield return m_Input.Talk();
            yield return Wait.Until(() => UseCaseMovement.Money >= before + 1, 2f, "the script went on as it set off");

            Assert.That(UseCaseMovement.Apart(actor, target), Is.GreaterThan(0.5f), "the script waited for it to arrive before going on");

            yield return Wait.Until(() => UseCaseMovement.Money == before + 2, 5f, "the script went on once it arrived");

            Assert.That(UseCaseMovement.Apart(actor, target), Is.LessThan(0.15f), "at the point");

            Assert.That(UseCaseMovement.Faces(actor.eulerAngles.y, 90f), "facing the way it went");
        }
    }
}
