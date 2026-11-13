using System.Collections;
using NUnit.Framework;
using RPGFramework.Field;
using RPGFramework.Core.Store;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;

namespace Test.PlayModeTests.SystemAndModules
{
    /// <summary>
    /// UseCaseTakeMoney: talked to, `SUBTRACT_MONEY` of more than the party holds.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenMoreMoneyIsTakenThanThePartyHas
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
        public IEnumerator ThenItHasNone()
        {
            yield return UseCaseGame.Enter(UseCaseField.SYSTEM, 10);

            UseCaseGame.Resolve<IMoneyStore>().AddMoney(100);

            Assert.That(UseCaseGame.ReadUint("Money"), Is.GreaterThan(0u), "the party had no money to take");

            yield return m_Input.Talk();

            yield return Wait.Until(() => UseCaseGame.ReadUint("Money") == 0, 5f, "no money left");
        }
    }
}
