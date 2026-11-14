using System.Text.RegularExpressions;
using UnityEngine;

namespace Test.PlayModeTests
{
    /// <summary>
    /// The movement use cases' entities, found by the names their build gave them — the use case's name and its id — and
    /// where they stand and face, as a player sees it: across the ground, and in 3D the body's turn about up.
    /// </summary>
    internal static class UseCaseMovement
    {
        internal static Transform Body(string name)
        {
            Transform field = UseCaseField.Loaded().transform;
            Regex     named = new Regex("^" + name + "[0-9]*$");

            for (int i = 0; i < field.childCount; i++)
            {
                if (named.IsMatch(field.GetChild(i).name))
                {
                    return field.GetChild(i);
                }
            }

            return null;
        }

        internal static float Apart(Transform a, Transform b)
        {
            return UseCaseField.GroundDistance(a.position, b.position, UseCaseProject.Up);
        }

        internal static float Apart(Transform a, Vector3 b)
        {
            return UseCaseField.GroundDistance(a.position, b, UseCaseProject.Up);
        }

        /// <summary>How far ahead of the spawn points' facing it stands.</summary>
        internal static float Ahead(Transform body)
        {
            return Vector3.Dot(body.position, UseCaseProject.Ahead);
        }

        internal static bool Faces(float degrees, float expected)
        {
            return Mathf.Abs(Mathf.DeltaAngle(degrees, expected)) < 1f;
        }

        internal static uint Money => UseCaseGame.ReadUint("Money");
    }
}
