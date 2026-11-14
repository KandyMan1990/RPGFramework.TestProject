using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Test.PlayModeTests.ScreenAndCamera
{
    /// <summary>
    /// UseCaseColourSubtracted: talked to, `SUBTRACT_SCREEN_COLOUR 1 0 0 0.5`, `WAIT_FOR_SCREEN_COLOUR` and `ADD_MONEY 1`:
    /// red taken from the screen over half a second, then the money.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenAColourIsTakenFromTheScreen
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
        public IEnumerator ThenTheScriptGoesOnOnceItIsTaken()
        {
            yield return UseCaseGame.Enter(UseCaseField.SCREEN, 0);

            uint before = UseCaseGame.ReadUint("Money");

            Assert.That(UseCaseScreen.IsClear(), "the screen clear to begin with");

            yield return m_Input.Talk();
            yield return Wait.Seconds(0.25f);

            Assert.That(UseCaseGame.ReadUint("Money"), Is.EqualTo(before), "the script went on before the colour was reached");

            yield return Wait.Until(() => UseCaseGame.ReadUint("Money") == before + 1, 4f, "the script went on once the colour was reached");

            Assert.That(UseCaseScreen.IsNear(UseCaseScreen.Taken(), Color.red), "red taken from the screen");
        }
    }
}
