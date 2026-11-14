using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Test.PlayModeTests.ScreenAndCamera
{
    /// <summary>
    /// UseCaseCamera: talked to, `SET_CAMERA 1 0 0`: a cut from the follow camera, id 0, to the fixed camera, id 1.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenTheCameraIsChanged
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
        public IEnumerator ThenTheViewIsThroughTheOtherCamera()
        {
            yield return UseCaseGame.Enter(UseCaseField.SCREEN, 7);

            Vector3 fixedCamera = UseCaseScreen.Find(UseCaseField.Loaded().transform, "Fixed Camera").transform.position;

            Assert.That((UseCaseScreen.View().transform.position - fixedCamera).magnitude, Is.GreaterThan(0.1f), "the view through the fixed camera already");

            yield return m_Input.Talk();
            yield return Wait.Until(() => (UseCaseScreen.View().transform.position - fixedCamera).magnitude < 0.01f, 2f, "the view through the fixed camera");
        }
    }
}
