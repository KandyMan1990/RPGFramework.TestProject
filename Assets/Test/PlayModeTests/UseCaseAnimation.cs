using UnityEngine;

namespace Test.PlayModeTests
{
    /// <summary>
    /// The animation use cases' characters as a player sees them: the state their Animator plays and how far through it,
    /// their model — the Animator's object, which draw offsets move and visibility hides — and their head, the humanoid's
    /// head bone.
    /// </summary>
    internal static class UseCaseAnimation
    {
        internal static Animator AnimatorOf(Transform body)
        {
            return body.GetComponentInChildren<Animator>(true);
        }

        internal static bool Plays(Transform body, string state)
        {
            return AnimatorOf(body).GetCurrentAnimatorStateInfo(0).IsName(state);
        }

        /// <summary>How far through the state it is, a pass from 0 to 1, whichever pass it is on.</summary>
        internal static float Through(Transform body)
        {
            return Mathf.Repeat(AnimatorOf(body).GetCurrentAnimatorStateInfo(0).normalizedTime, 1f);
        }

        internal static float Finished(Transform body)
        {
            return AnimatorOf(body).GetCurrentAnimatorStateInfo(0).normalizedTime;
        }

        /// <summary>The length of the clip playing, at its own speed.</summary>
        internal static float Length(Transform body)
        {
            return AnimatorOf(body).GetCurrentAnimatorClipInfo(0)[0].clip.length;
        }

        internal static Transform Model(Transform body)
        {
            return AnimatorOf(body).transform;
        }

        /// <summary>Where the model is drawn from the body.</summary>
        internal static Vector3 Drawn(Transform body)
        {
            return Model(body).position - body.position;
        }

        internal static Transform Head(Transform body)
        {
            return AnimatorOf(body).GetBoneTransform(HumanBodyBones.Head);
        }
    }
}
