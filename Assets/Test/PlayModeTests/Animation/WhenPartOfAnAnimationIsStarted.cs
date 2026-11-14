using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Test.PlayModeTests.Animation
{
    /// <summary>
    /// UseCasePart: talked to, `PLAY_PART_OF_ANIMATION Spin 0.25 0.75` and `ADD_MONEY 1`.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenPartOfAnAnimationIsStarted
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
        public IEnumerator ThenTheScriptGoesOnAndItGoesBackToTheBase()
        {
            yield return UseCaseGame.Enter(UseCaseField.ANIMATION, 7);

            Transform actor  = UseCaseMovement.Body("UseCasePart");
            uint      before = UseCaseMovement.Money;

            yield return m_Input.Talk();
            yield return Wait.Until(() => UseCaseMovement.Money == before + 1, 2f, "the script went on as the part began");
            yield return Wait.Until(() => UseCaseAnimation.Plays(actor, "Spin"), 2f, "Spin playing");
            yield return Wait.Until(() => UseCaseAnimation.Plays(actor, "Stand"), 5f, "back to its base, Stand, once the part had finished");
        }
    }
}
