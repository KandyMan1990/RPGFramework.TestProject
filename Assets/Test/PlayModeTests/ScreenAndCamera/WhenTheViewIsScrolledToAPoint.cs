using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Test.PlayModeTests.ScreenAndCamera
{
    /// <summary>
    /// UseCaseScrollToPoint: talked to, `SCROLL_SCREEN_TO_POSITION` the project's scroll point over half a second,
    /// `WAIT_FOR_SCROLLING` and `ADD_MONEY 1`: the view on the point, then the money.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenTheViewIsScrolledToAPoint
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
        public IEnumerator ThenItArrivesAndTheScriptGoesOn()
        {
            yield return UseCaseGame.Enter(UseCaseField.SCREEN, 5);

            uint    before = UseCaseGame.ReadUint("Money");
            Vector3 start  = UseCaseScreen.View().transform.position;

            yield return m_Input.Talk();
            yield return Wait.Seconds(0.25f);

            Assert.That(UseCaseGame.ReadUint("Money"), Is.EqualTo(before), "the script went on before the view arrived");

            yield return Wait.Until(() => UseCaseGame.ReadUint("Money") == before + 1, 4f, "the script went on once the view arrived");

            Assert.That((UseCaseScreen.Focus() - UseCaseProject.ScrollPoint).magnitude, Is.LessThan(0.01f), "the view on the point");
            Assert.That((UseCaseScreen.View().transform.position - start).magnitude, Is.GreaterThan(1f), "the view moved");
        }
    }
}
