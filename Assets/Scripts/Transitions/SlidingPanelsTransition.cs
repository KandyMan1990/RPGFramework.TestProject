using RPGFramework.Core.Transitions;
using UnityEngine;
using UnityEngine.UIElements;

namespace Test.Transitions
{
    /// <summary>
    /// A panel arriving in two halves over the live screen, the left half sliding in from the left and the right from
    /// the right to meet in the middle; uncovering slides them apart. The panel is authored whole, as it looks once met,
    /// and each half is a copy of it clipped to its side, so they meet without a seam and the next screen can build the
    /// same panel behind them. UI elements alone, placed by how covered the screen is, so it plays either way.
    /// </summary>
    [CreateAssetMenu(menuName = "Test/Transitions/Sliding Panels", fileName = "SlidingPanels")]
    internal sealed class SlidingPanelsTransition : TransitionAsset
    {
        [SerializeField]
        [Tooltip("The panel as it looks once its halves have met, filling the screen")]
        private VisualTreeAsset m_Panel;

        protected override TransitionPlayback Begin(TransitionContext context)
        {
            VisualElement left  = AddHalf(context.Host, 0f, 0f);
            VisualElement right = AddHalf(context.Host, 50f, -100f);

            TransitionPlayback playback = new PanelsPlayback(left, right);

            return playback;
        }

        // Half the screen, clipping a whole copy of the panel placed so that half of it shows.
        private VisualElement AddHalf(VisualElement host, float left, float panelLeft)
        {
            VisualElement half  = new VisualElement { pickingMode = PickingMode.Ignore };
            VisualElement panel = m_Panel.Instantiate();

            half.style.position = Position.Absolute;
            half.style.left     = Length.Percent(left);
            half.style.top      = 0f;
            half.style.bottom   = 0f;
            half.style.width    = Length.Percent(50f);
            half.style.overflow = Overflow.Hidden;

            panel.pickingMode    = PickingMode.Ignore;
            panel.style.position = Position.Absolute;
            panel.style.left     = Length.Percent(panelLeft);
            panel.style.top      = 0f;
            panel.style.bottom   = 0f;
            panel.style.width    = Length.Percent(200f);

            half.Add(panel);
            host.Add(half);

            return half;
        }

        private sealed class PanelsPlayback : TransitionPlayback
        {
            private readonly VisualElement m_Left;
            private readonly VisualElement m_Right;

            internal PanelsPlayback(VisualElement left, VisualElement right)
            {
                m_Left  = left;
                m_Right = right;
            }

            // Each half moves by its own width, so clear has both just off screen whatever its size.
            protected override void Show(float covered)
            {
                float away = (1f - covered) * 100f;

                m_Left.style.translate  = new Translate(Length.Percent(-away), 0f);
                m_Right.style.translate = new Translate(Length.Percent(away), 0f);
            }

            protected override void End()
            {
                m_Left.RemoveFromHierarchy();
                m_Right.RemoveFromHierarchy();
            }
        }
    }
}
