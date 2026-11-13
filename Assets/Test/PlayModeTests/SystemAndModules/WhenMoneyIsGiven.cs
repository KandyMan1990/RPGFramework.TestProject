using System.Collections;
using NUnit.Framework;
using RPGFramework.Field;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;

namespace Test.PlayModeTests.SystemAndModules
{
    /// <summary>
    /// UseCaseGiveMoney: talked to, `ADD_MONEY 50`.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenMoneyIsGiven
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
        public IEnumerator ThenThePartyHasFiftyMore()
        {
            yield return UseCaseGame.Enter(UseCaseField.SYSTEM, 9);

            uint before = UseCaseGame.ReadUint("Money");

            yield return m_Input.Talk();

            yield return Wait.Until(() => UseCaseGame.ReadUint("Money") == before + 50, 5f, "fifty more money");
        }
    }
}
