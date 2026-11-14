using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Test.PlayModeTests.ScreenAndCamera
{
    /// <summary>
    /// UseCaseScrollToEntity: talked to, `SCROLL_SCREEN_TO_ENTITY` the first use case's examine point over half a
    /// second, `WAIT_FOR_SCROLLING` and `ADD_MONEY 1`: the view on that examine point, at the other end of the row.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenTheViewIsScrolledToAnEntity
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
        public IEnumerator ThenItIsOnTheEntity()
        {
            yield return UseCaseGame.Enter(UseCaseField.SCREEN, 6);

            uint    before = UseCaseGame.ReadUint("Money");
            Vector3 start  = UseCaseScreen.View().transform.position;
            Vector3 target = UseCaseScreen.Find(UseCaseField.Loaded().transform, "UseCaseColourSubtracted1").transform.position;

            yield return m_Input.Talk();
            yield return Wait.Until(() => UseCaseGame.ReadUint("Money") == before + 1, 4f, "the script went on once the view arrived");

            Assert.That((UseCaseScreen.Focus() - target).magnitude, Is.LessThan(0.01f), "the view on the examine point");
            Assert.That((UseCaseScreen.View().transform.position - start).magnitude, Is.GreaterThan(1f), "the view moved");
        }
    }
}
