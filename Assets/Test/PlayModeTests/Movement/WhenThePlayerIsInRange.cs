using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Test.PlayModeTests.Movement
{
    /// <summary>
    /// UseCaseInRange: talked to, `IS_ENTITY_IN_RANGE 0` into the project's in-range variable: the player two away, inside
    /// its reach of two and a half.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenThePlayerIsInRange
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
        public IEnumerator ThenItIsStored()
        {
            yield return UseCaseGame.Enter(UseCaseField.MOVEMENT, 19);

            Assert.That(UseCaseGame.ReadBool(UseCaseProject.IN_RANGE), Is.False, "in range already");

            yield return m_Input.Talk();
            yield return Wait.Until(() => UseCaseGame.ReadBool(UseCaseProject.IN_RANGE), 2f, "the player found in range");
        }
    }
}
