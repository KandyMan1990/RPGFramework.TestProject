using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UIElements;

namespace Test.PlayModeTests
{
    /// <summary>
    /// The field's screen as a player sees it. Its fades and colours each draw an element into the layer at the back of
    /// the field's UI, `ScreenFade`, that takes a colour from what is beneath or adds one, a fade out taking white once it
    /// has covered the screen. The view is the camera the field's Brain draws through, a shake shifting its projection,
    /// and its follow cameras follow the `Camera Focus` marker.
    /// </summary>
    internal static class UseCaseScreen
    {
        internal static Color Taken()
        {
            return Parameter(0);
        }

        internal static Color Added()
        {
            return Parameter(1);
        }

        /// <summary>Nothing taken or added: no fade or colour has anything in the layer.</summary>
        internal static bool IsClear()
        {
            VisualElement layer = Layer();

            return layer != null && layer.childCount == 0;
        }

        internal static bool IsNear(Color a, Color b)
        {
            return Mathf.Abs(a.r - b.r) < 0.01f && Mathf.Abs(a.g - b.g) < 0.01f && Mathf.Abs(a.b - b.b) < 0.01f;
        }

        internal static Camera View()
        {
            return Object.FindAnyObjectByType<CinemachineBrain>().OutputCamera;
        }

        /// <summary>How far a shake has moved the picture across and up, as it shifts the view's projection.</summary>
        internal static Vector2 Shift()
        {
            Camera    view       = View();
            Matrix4x4 projection = view.projectionMatrix;

            return view.orthographic ? new Vector2(projection.m03, projection.m13) : new Vector2(-projection.m02, -projection.m12);
        }

        internal static Vector3 Focus()
        {
            return GameObject.Find("Camera Focus").transform.position;
        }

        /// <summary>Something in the field by its name, at any depth, switched off or on.</summary>
        internal static GameObject Find(Transform parent, string name)
        {
            for (int i = 0; i < parent.childCount; i++)
            {
                Transform child = parent.GetChild(i);

                if (child.name == name)
                {
                    return child.gameObject;
                }

                GameObject found = Find(child, name);

                if (found != null)
                {
                    return found;
                }
            }

            return null;
        }

        private static VisualElement Layer()
        {
            UIDocument[] documents = Object.FindObjectsByType<UIDocument>();

            for (int i = 0; i < documents.Length; i++)
            {
                VisualElement layer = documents[i].rootVisualElement?.Q("ScreenFade");

                if (layer != null)
                {
                    return layer;
                }
            }

            return null;
        }

        private static Color Parameter(int index)
        {
            VisualElement layer = Layer();

            for (int i = 0; layer != null && i < layer.childCount; i++)
            {
                StyleList<FilterFunction> filter = layer[i].style.backdropFilter;

                if (filter.keyword == StyleKeyword.Undefined && filter.value != null && filter.value.Count > 0)
                {
                    return filter.value[0].GetParameter(index).colorValue;
                }
            }

            return Color.clear;
        }
    }
}
