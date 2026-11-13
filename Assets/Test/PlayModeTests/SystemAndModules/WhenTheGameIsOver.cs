using System.Collections;
using NUnit.Framework;
using RPGFramework.Field;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;

namespace Test.PlayModeTests.SystemAndModules
{
    /// <summary>
    /// UseCaseGameOver: talked to, `GAME_OVER`.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenTheGameIsOver
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
        public IEnumerator ThenTheTitleOpens()
        {
            yield return UseCaseGame.Enter(UseCaseField.SYSTEM, 7);

            yield return m_Input.Talk();

            yield return Wait.Until(() => UseCaseGame.Scene == UseCaseGame.TITLE_SCENE, 10f, "the title opened");
        }
    }
}
