#if ENABLE_RPG_FRAMEWORK_DEBUG
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UIElements;

namespace Test.PlayModeTests.Flow
{
    /// <summary>
    /// The debug room from the title and back, as a player would, under the debug define: UseCaseGameOver talked to for
    /// the title, down from New Game to Debug Room, chosen, and Cancel at the room's top closing it.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenTheDebugRoomIsOpenedFromTheTitle
    {
        private const string DEBUG_ROOM_SCENE = "DebugRoomModule";
        private const string DEBUG_ROOM       = "DebugRoomBtn";

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
        public IEnumerator ThenItClosesBackToTheTitle()
        {
            yield return UseCaseGame.Enter(UseCaseField.SYSTEM, 7);
            yield return m_Input.Talk();
            yield return Wait.Until(() => UseCaseGame.Scene == UseCaseGame.TITLE_SCENE, 10f, "the title opened");
            yield return UseCaseMenu.ReachTitle();

            for (int i = 0; i < 3; i++)
            {
                UseCaseMenu.Move(NavigationMoveEvent.Direction.Down);

                yield return null;
            }

            Assert.That(UseCaseMenu.FocusedName, Is.EqualTo(DEBUG_ROOM), "Debug Room three below New Game");

            UseCaseMenu.Submit();

            yield return Wait.Until(() => UseCaseGame.Scene == DEBUG_ROOM_SCENE, 10f, "the debug room opened");

            float end = Time.realtimeSinceStartup + 10f;

            while (UseCaseGame.Scene != UseCaseGame.TITLE_SCENE)
            {
                Assert.That(Time.realtimeSinceStartup, Is.LessThan(end), "Cancel closed the room back to the title within ten seconds");

                yield return m_Input.Tap(m_Input.Keyboard.escapeKey);
                yield return Wait.Seconds(0.25f);
            }

            yield return UseCaseMenu.ReachTitle();
        }
    }
}
#endif
