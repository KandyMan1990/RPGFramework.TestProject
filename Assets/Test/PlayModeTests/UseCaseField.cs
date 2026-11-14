using RPGFramework.Field;
using UnityEngine;
using UnityEngine.UIElements;

namespace Test.PlayModeTests
{
    /// <summary>The field the module has loaded, and what is in it, found as a player sees it: by what it is called.</summary>
    internal static class UseCaseField
    {
        internal const string SYSTEM  = "UseCaseSystem";
        internal const string WINDOWS = "UseCaseWindows";
        internal const string AUDIO   = "UseCaseAudio";
        internal const string SCREEN  = "UseCaseScreen";

        /// <summary>The field loaded, or null while none is, or the next is loading before the last has gone.</summary>
        internal static FieldEntities Loaded()
        {
            FieldEntities[] fields = Object.FindObjectsByType<FieldEntities>();

            return fields.Length == 1 ? fields[0] : null;
        }

        /// <summary>Whether the field loaded is the one named, as the module instantiates it from its bundle.</summary>
        internal static bool IsLoaded(string field)
        {
            FieldEntities loaded = Loaded();

            return loaded != null && loaded.name.StartsWith(field);
        }

        internal static Vector3 PositionOf(FieldEntities field, string name)
        {
            return field.transform.Find(name).position;
        }

        /// <summary>How far apart two places are across the ground, whatever their heights.</summary>
        internal static float GroundDistance(Vector3 a, Vector3 b, Vector3 up)
        {
            return Vector3.ProjectOnPlane(a - b, up).magnitude;
        }

        /// <summary>Whether a transition has covered the screen: Core's overlay shows its backdrop, its first element, once it has.</summary>
        internal static bool IsCovered()
        {
            GameObject overlay = GameObject.Find("Transitions");

            return overlay != null && overlay.GetComponent<UIDocument>().rootVisualElement[0].resolvedStyle.display == DisplayStyle.Flex;
        }
    }
}
