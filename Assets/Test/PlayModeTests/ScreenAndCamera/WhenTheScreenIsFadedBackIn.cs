using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Test.PlayModeTests.ScreenAndCamera
{
    /// <summary>
    /// UseCaseScreenFadedAndWaited: talked to, `FADE_SCREEN true true`, `FADE_SCREEN_AND_WAIT false false` and
    /// `ADD_MONEY 1`: covered at once, then faded back in, the script waiting for it.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenTheScreenIsFadedBackIn
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
        public IEnumerator ThenTheScriptWaitsUntilItIsClear()
        {
            yield return UseCaseGame.Enter(UseCaseField.SCREEN, 3);

            uint before = UseCaseGame.ReadUint("Money");

            yield return m_Input.Talk();
            yield return Wait.Until(() => UseCaseScreen.Taken().r > 0.5f || UseCaseGame.ReadUint("Money") != before, 3f, "the screen covered at once");

            Assert.That(UseCaseGame.ReadUint("Money"), Is.EqualTo(before), "the script went on before the screen had faded back in");

            yield return Wait.Until(() => UseCaseGame.ReadUint("Money") == before + 1, 3f, "the script went on once the screen was clear");

            Assert.That(UseCaseScreen.IsClear(), "the screen clear");
        }
    }
}
