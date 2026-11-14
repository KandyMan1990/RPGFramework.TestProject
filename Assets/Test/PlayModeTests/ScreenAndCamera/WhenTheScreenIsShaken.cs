using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Test.PlayModeTests.ScreenAndCamera
{
    /// <summary>
    /// UseCaseShake: talked to, `SHAKE_SCREEN 30 0.1 30 0.15`, `WAIT_SECONDS 1` and `STOP_SCREEN_SHAKE`: the picture
    /// shifted, then still again.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenTheScreenIsShaken
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
        public IEnumerator ThenItShakesAndComesToRestOnceStopped()
        {
            yield return UseCaseGame.Enter(UseCaseField.SCREEN, 4);

            Vector2 rest = UseCaseScreen.Shift();

            yield return m_Input.Talk();
            yield return Wait.Until(() => (UseCaseScreen.Shift() - rest).magnitude > 0.001f, 2f, "the picture shaken");
            yield return Wait.Seconds(2f);

            Assert.That((UseCaseScreen.Shift() - rest).magnitude, Is.LessThan(0.00001f), "the picture at rest once the shake stopped");
        }
    }
}
