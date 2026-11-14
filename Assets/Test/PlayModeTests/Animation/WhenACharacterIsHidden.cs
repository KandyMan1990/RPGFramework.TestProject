using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Test.PlayModeTests.Animation
{
    /// <summary>
    /// UseCaseHidden: talked to, `VISIBILITY false`.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenACharacterIsHidden
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
        public IEnumerator ThenItsModelIsGone()
        {
            yield return UseCaseGame.Enter(UseCaseField.ANIMATION, 27);

            Transform actor  = UseCaseMovement.Body("UseCaseHidden");

            Transform model = UseCaseAnimation.Model(actor);

            Assert.That(model.gameObject.activeSelf, "its model shown to begin with");

            yield return m_Input.Talk();
            yield return Wait.Until(() => !model.gameObject.activeSelf, 2f, "its model hidden");
        }
    }
}
