using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Test.PlayModeTests.ScreenAndCamera
{
    /// <summary>
    /// UseCaseBlocker: talked to, `BLOCKER_ACTIVATION 0 true`: the wall behind its examine point there, its collider with it.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenABlockerIsClosed
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
        public IEnumerator ThenItsWallIsThere()
        {
            yield return UseCaseGame.Enter(UseCaseField.SCREEN, 9);

            GameObject wall = UseCaseScreen.Find(UseCaseField.Loaded().transform, "UseCaseWall");

            Assert.That(wall.activeInHierarchy, Is.False, "the wall there to begin with");

            yield return m_Input.Talk();
            yield return Wait.Until(() => wall.activeInHierarchy, 2f, "the wall closing the way");
        }
    }
}
