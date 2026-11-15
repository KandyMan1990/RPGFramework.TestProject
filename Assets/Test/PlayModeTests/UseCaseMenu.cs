using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UIElements;

namespace Test.PlayModeTests
{
    /// <summary>
    /// The menus as a player moves through them. The test's keyboard does not reach UI Toolkit's navigation, so a menu is
    /// moved through and chosen from with the navigation events that navigation sends the focused element.
    /// </summary>
    internal static class UseCaseMenu
    {
        internal const string NEW_GAME  = "NewGameBtn";
        internal const string LOAD_GAME = "LoadGameBtn";

        internal static VisualElement Focused()
        {
            UIDocument[] documents = Object.FindObjectsByType<UIDocument>();

            for (int i = 0; i < documents.Length; i++)
            {
                IPanel        panel   = documents[i].rootVisualElement?.panel;
                VisualElement focused = panel?.focusController?.focusedElement as VisualElement;

                if (focused != null)
                {
                    return focused;
                }
            }

            return null;
        }

        internal static string FocusedName => Focused()?.name;

        internal static void Submit()
        {
            using (NavigationSubmitEvent submit = NavigationSubmitEvent.GetPooled())
            {
                Focused().SendEvent(submit);
            }
        }

        internal static void Move(NavigationMoveEvent.Direction direction)
        {
            using (NavigationMoveEvent move = NavigationMoveEvent.GetPooled(direction))
            {
                Focused().SendEvent(move);
            }
        }

        /// <summary>
        /// The title taking the player's keys with New Game focused, a language chosen first if it asks for one, as on a
        /// first run. A menu drops every key until it has finished entering, after it has focused its first button, so
        /// the title has them once New Game moves down to Load Game and back.
        /// </summary>
        internal static IEnumerator ReachTitle()
        {
            float end = Time.realtimeSinceStartup + 10f;

            while (true)
            {
                Assert.That(Time.realtimeSinceStartup, Is.LessThan(end), "the title took the player's keys within ten seconds");

                string focused = FocusedName;

                if (focused == NEW_GAME)
                {
                    Move(NavigationMoveEvent.Direction.Down);

                    yield return null;

                    if (FocusedName != NEW_GAME)
                    {
                        Move(NavigationMoveEvent.Direction.Up);

                        yield return null;

                        Assert.That(FocusedName, Is.EqualTo(NEW_GAME), "New Game focused again");

                        yield break;
                    }
                }
                else if (focused == LOAD_GAME)
                {
                    Move(NavigationMoveEvent.Direction.Up);
                }
                else if (focused != null)
                {
                    Submit();
                }

                yield return Wait.Seconds(0.3f);
            }
        }
    }
}
