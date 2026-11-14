using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Test.PlayModeTests.ScreenAndCamera
{
    /// <summary>
    /// UseCaseScreenFaded: talked to, `FADE_SCREEN true false`, `ADD_MONEY 1`, `WAIT_FOR_FADE` and `ADD_MONEY 1`: the field's
    /// own fade out, the default subtractive one, which takes white once it has covered the screen.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenTheScreenIsFadedOut
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
            yield return UseCaseGame.Enter(UseCaseField.SCREEN, 2);

            uint before = UseCaseGame.ReadUint("Money");

            yield return m_Input.Talk();
            yield return Wait.Until(() => UseCaseGame.ReadUint("Money") >= before + 1, 3f, "the script went on as the fade began");

            Assert.That(UseCaseScreen.Taken().r, Is.LessThan(0.9f), "the script waited for the fade before going on");

            yield return Wait.Until(() => UseCaseGame.ReadUint("Money") == before + 2, 3f, "the script went on once the fade ended");

            Assert.That(UseCaseScreen.IsNear(UseCaseScreen.Taken(), Color.white), "the screen covered");
        }
    }
}
