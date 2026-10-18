using RPGFramework.Core.Transitions;
using UnityEngine;
using UnityEngine.UIElements;

namespace Test.Transitions
{
    /// <summary>
    /// The captured frame spun into a vortex: every step draws the last frame back over itself as a grid, turned a little
    /// further and zoomed a little more each step, half over it but a quarter added every fifth step, so its copies
    /// smear into trails that spiral into the centre and brighten; then the grid dims, laid half over it. Nothing is
    /// cleared, so the corners the turned grid leaves keep what was there. Seventy-nine steps, a step every two frames
    /// at 60. A cover only: it keeps no state to run backwards.
    /// </summary>
    [CreateAssetMenu(menuName = "Test/Transitions/Swirl", fileName = "Swirl")]
    internal sealed class SwirlTransition : TransitionAsset
    {
        [SerializeField]
        private Shader m_Shader;

        protected override bool CapturesFrame => true;

        protected override TransitionPlayback Begin(TransitionContext context)
        {
            RetroCanvas canvas = new RetroCanvas(m_Shader, context.CapturedFrame);
            Image       screen = new Image { image = canvas.Display, scaleMode = ScaleMode.StretchToFill, pickingMode = PickingMode.Ignore };

            screen.style.position = Position.Absolute;
            screen.style.left     = 0f;
            screen.style.top      = 0f;
            screen.style.right    = 0f;
            screen.style.bottom   = 0f;

            context.Host.Add(screen);

            TransitionPlayback playback = new SwirlPlayback(canvas, screen);

            return playback;
        }

        private sealed class SwirlPlayback : TransitionPlayback
        {
            private const int   STEPS         = 79;
            private const int   DIM_AFTER     = 46;
            private const int   ADD_EVERY     = 5;
            private const int   COLUMNS       = 10;
            private const int   ROWS          = 7;
            private const float CELL          = 32f;
            private const float CELL_TEXELS   = 31f;
            private const float CENTRE_X      = 159f;
            private const float CENTRE_Y      = 119f;
            private const float TURN_PER_STEP = 3f / 4096f * Mathf.PI * 2f;
            private const float ONE           = 4096f;

            // The projection divides by a depth of zero, which the hardware saturates at a hair under 2.
            private const float PROJECTION = 131071f / 65536f;

            private readonly RetroCanvas m_Canvas;
            private readonly Image       m_Screen;

            private int m_Step;
            private int m_Scale;
            private int m_ScaleSpeed;
            private int m_Brightness;

            internal SwirlPlayback(RetroCanvas canvas, Image screen)
            {
                m_Canvas     = canvas;
                m_Screen     = screen;
                m_Step       = 0;
                m_Scale      = 2080;
                m_ScaleSpeed = 4;
                m_Brightness = 128;
            }

            protected override void Show(float covered)
            {
                int step = Mathf.Clamp(Mathf.FloorToInt(covered * STEPS), 0, STEPS);

                while (m_Step < step)
                {
                    DrawStep(m_Step);

                    m_Step++;
                }

                m_Screen.MarkDirtyRepaint();
            }

            protected override void End()
            {
                m_Screen.RemoveFromHierarchy();
                m_Canvas.Release();
            }

            private void DrawStep(int step)
            {
                if (step > DIM_AFTER && m_Brightness > 0)
                {
                    m_Brightness--;
                }

                m_Canvas.BeginFrame();

                m_Canvas.Draw(RetroBlend.Opaque, m_Canvas.Picture);
                m_Canvas.Rectangle(0f, 0f, RetroCanvas.WIDTH, RetroCanvas.HEIGHT, 0f, 0f, RetroCanvas.WIDTH, RetroCanvas.HEIGHT, RetroCanvas.Textured(128));
                m_Canvas.EndDraw();

                m_Canvas.Draw(Blend(step), m_Canvas.Picture);
                DrawGrid(-step * TURN_PER_STEP, m_Scale / ONE * PROJECTION, RetroCanvas.Textured(m_Brightness));
                m_Canvas.EndDraw();

                m_Canvas.EndFrame();
                m_Canvas.CopyFrameToPicture();

                m_Scale      += m_ScaleSpeed >> 4;
                m_ScaleSpeed += 10;
            }

            // Chosen from the step count once it has moved on, so a quarter is added on the fifth step, the tenth and so
            // on. Dimming stops the choosing, and the two buffers drawn with keep the last two choices, both averages.
            private static RetroBlend Blend(int step)
            {
                int        counted = Mathf.Min(step, DIM_AFTER) + 1;
                RetroBlend blend   = counted % ADD_EVERY == 0 ? RetroBlend.AddQuarter : RetroBlend.Half;

                return blend;
            }

            private void DrawGrid(float angle, float scale, Color colour)
            {
                float cos = Mathf.Cos(angle) * scale;
                float sin = Mathf.Sin(angle) * scale;

                for (int row = 0; row < ROWS; row++)
                {
                    for (int column = 0; column < COLUMNS; column++)
                    {
                        float left = column * CELL;
                        float top  = row * CELL;

                        m_Canvas.Quad(Turn(left, top, cos, sin), Turn(left + CELL, top, cos, sin),
                                      Turn(left, top + CELL, cos, sin), Turn(left + CELL, top + CELL, cos, sin),
                                      new Vector2(left, top), new Vector2(left + CELL_TEXELS, top),
                                      new Vector2(left, top + CELL_TEXELS), new Vector2(left + CELL_TEXELS, top + CELL_TEXELS),
                                      colour);
                    }
                }
            }

            // A grid point, laid out about the picture's centre, turned and scaled about the swirl's.
            private static Vector2 Turn(float x, float y, float cos, float sin)
            {
                float   fromCentreX = x - RetroCanvas.WIDTH / 2f;
                float   fromCentreY = y - RetroCanvas.HEIGHT / 2f;
                Vector2 turned      = new Vector2(CENTRE_X + cos * fromCentreX - sin * fromCentreY, CENTRE_Y + sin * fromCentreX + cos * fromCentreY);

                return turned;
            }
        }
    }
}