using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Test.PlayModeTests.ScreenAndCamera
{
    /// <summary>
    /// UseCaseColourAdded: talked to, `ADD_SCREEN_COLOUR 0 0 1 0`: blue added to the screen at once.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenAColourIsAddedToTheScreen
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
        public IEnumerator ThenItIsAddedAtOnce()
        {
            yield return UseCaseGame.Enter(UseCaseField.SCREEN, 1);

            yield return m_Input.Talk();
            yield return Wait.Until(() => UseCaseScreen.IsNear(UseCaseScreen.Added(), Color.blue), 2f, "blue added to the screen");
        }
    }
}
