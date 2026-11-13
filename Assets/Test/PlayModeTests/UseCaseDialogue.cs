using System.Collections;
using NUnit.Framework;
using RPGFramework.Core.UI;
using UnityEngine;
using UnityEngine.UIElements;

namespace Test.PlayModeTests
{
    /// <summary>
    /// The dialogue window on the screen, read as a player sees it: Core's windows carry the class `dialogue-window`,
    /// their text in a label named `Text`, their place in their style and their look as a class.
    /// </summary>
    internal static class UseCaseDialogue
    {
        private const string WINDOW       = "dialogue-window";
        private const string STYLE_PREFIX = "dialogue-window--";
        private const string FITTED       = "dialogue-window--fitted";

        /// <summary>The window showing, or null while none is.</summary>
        internal static VisualElement Shown()
        {
            UIDocument[] documents = Object.FindObjectsByType<UIDocument>();

            for (int i = 0; i < documents.Length; i++)
            {
                VisualElement root = documents[i].rootVisualElement;

                if (root == null)
                {
                    continue;
                }

                VisualElement window = root.Query(className: WINDOW).Where(w => w.resolvedStyle.display == DisplayStyle.Flex).First();

                if (window != null)
                {
                    return window;
                }
            }

            return null;
        }

        internal static string Text(VisualElement window)
        {
            return window.Q<Label>("Text").text;
        }

        internal static bool IsAt(VisualElement window, float x, float y)
        {
            return Mathf.Approximately(window.style.left.value.value, x) && Mathf.Approximately(window.style.top.value.value, y);
        }

        internal static bool IsSized(VisualElement window, float width, float height)
        {
            return Mathf.Abs(window.resolvedStyle.width - width) < 1f && Mathf.Abs(window.resolvedStyle.height - height) < 1f;
        }

        /// <summary>Sized to its text, as a window shown at a point is.</summary>
        internal static bool IsFitted(VisualElement window)
        {
            return window.ClassListContains(FITTED);
        }

        internal static bool HasStyle(VisualElement window, string style)
        {
            return window.ClassListContains(STYLE_PREFIX + style);
        }

        /// <summary>
        /// Which answer has the focus, or -1 while none does: a question focuses its starting answer once it and its
        /// answers have typed, a press before then only finishing the typing, and Cancel moves the focus to its cancel
        /// answer.
        /// </summary>
        internal static int FocusedAnswer(VisualElement window)
        {
            Focusable focused = window.focusController?.focusedElement;
            int       index   = -1;
            int       next    = 0;

            window.Query<RPGUIButton>().ForEach(answer =>
                                                {
                                                    if (answer == focused)
                                                    {
                                                        index = next;
                                                    }

                                                    next++;
                                                });

            return index;
        }

        /// <summary>Confirm pressed until no window shows: the first press may only finish the typing.</summary>
        internal static IEnumerator Dismiss(UseCaseInput input)
        {
            float end = Time.realtimeSinceStartup + 8f;

            while (Shown() != null)
            {
                Assert.That(Time.realtimeSinceStartup, Is.LessThan(end), "the window was dismissed within eight seconds");

                yield return input.Talk();
                yield return Wait.Seconds(0.25f);
            }
        }
    }
}
