using RPGFramework.Core.Transitions;
using UnityEngine;
using UnityEngine.UIElements;

namespace Test
{
    /// <summary>
    /// The screen as it was, frozen, darkening to black: a cover over a captured frame, so the change it covers loads
    /// beneath it.
    /// </summary>
    [CreateAssetMenu(menuName = "Test/Frozen Frame Fade", fileName = "FrozenFrameFade")]
    internal sealed class FrozenFrameFade : TransitionAsset
    {
        protected override bool CapturesFrame => true;

        protected override TransitionPlayback Begin(TransitionContext context)
        {
            Image frame = new Image { image = context.CapturedFrame, scaleMode = ScaleMode.StretchToFill, pickingMode = PickingMode.Ignore };

            frame.style.position = Position.Absolute;
            frame.style.left     = 0f;
            frame.style.top      = 0f;
            frame.style.right    = 0f;
            frame.style.bottom   = 0f;

            context.Host.Add(frame);

            TransitionPlayback playback = new FramePlayback(frame);

            return playback;
        }

        private sealed class FramePlayback : TransitionPlayback
        {
            private readonly Image m_Frame;

            internal FramePlayback(Image frame)
            {
                m_Frame = frame;
            }

            protected override void Show(float covered)
            {
                m_Frame.tintColor = Color.Lerp(Color.white, Color.black, covered);
            }

            protected override void End()
            {
                m_Frame.RemoveFromHierarchy();
            }
        }
    }
}