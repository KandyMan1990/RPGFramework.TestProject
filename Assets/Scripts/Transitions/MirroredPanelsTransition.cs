using RPGFramework.Core.Transitions;
using UnityEngine;
using UnityEngine.UIElements;

namespace Test.Transitions
{
    /// <summary>
    /// The captured frame growing and fading away while four copies of it, mirrored into the screen's quarters like a
    /// kaleidoscope and faded towards their edges, shrink and then swell, smeared by a slightly larger copy of each frame
    /// laid half over it; then a white flash that fades to black. Eighty steps, a step every three frames at 60. A cover
    /// only: it keeps no state to run backwards.
    /// </summary>
    [CreateAssetMenu(menuName = "Test/Transitions/Mirrored Panels", fileName = "MirroredPanels")]
    internal sealed class MirroredPanelsTransition : TransitionAsset
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

            TransitionPlayback playback = new PanelsPlayback(canvas, screen);

            return playback;
        }

        private sealed class PanelsPlayback : TransitionPlayback
        {
            private const int   STEPS        = 80;
            private const int   ZOOM_END     = 40;
            private const int   PANELS_END   = 64;
            private const int   FLASH_START  = 48;
            private const int   FLASH_RISE   = 16;
            private const int   BLANK_FROM   = 78;
            private const int   GRID         = 8;
            private const float CELL_WIDTH   = RetroCanvas.WIDTH / GRID;
            private const float CELL_HEIGHT  = 27f;
            private const float GRID_HEIGHT  = CELL_HEIGHT * GRID;
            private const float SMEAR_HEIGHT = 208f;
            private const float ONE          = 4096f;

            // Each line of the grid's brightness, across and down: nothing at the border, full from the second line in.
            private static readonly int[] m_EdgeBrightness = { 0, 64, 128, 128, 128, 128, 128, 64, 0 };

            // Where each cell samples the frame: even but for the last column and row, a pixel short.
            private static readonly float[] m_TexelLeft  = { 0f, 40f, 80f, 120f, 160f, 200f, 240f, 279f };
            private static readonly float[] m_TexelRight = { 40f, 80f, 120f, 160f, 200f, 240f, 280f, 319f };
            private static readonly float[] m_TexelRows  = { 0f, 27f, 54f, 81f, 108f, 135f, 162f, 189f, 215f };

            private readonly RetroCanvas m_Canvas;
            private readonly Image       m_Screen;

            private int m_Step;

            internal PanelsPlayback(RetroCanvas canvas, Image screen)
            {
                m_Canvas = canvas;
                m_Screen = screen;
                m_Step   = 0;
            }

            protected override void Show(float covered)
            {
                int step = Mathf.Clamp(Mathf.FloorToInt(covered * STEPS), 0, STEPS);

                while (m_Step < step)
                {
                    m_Step++;

                    DrawStep();
                }

                m_Screen.MarkDirtyRepaint();
            }

            protected override void End()
            {
                m_Screen.RemoveFromHierarchy();
                m_Canvas.Release();
            }

            private void DrawStep()
            {
                m_Canvas.BeginFrame();
                m_Canvas.Clear();

                if (m_Step >= BLANK_FROM)
                {
                    m_Canvas.EndFrame();

                    return;
                }

                if (m_Step < ZOOM_END)
                {
                    DrawGrowing();
                }

                if (m_Step < PANELS_END)
                {
                    DrawPanels();
                    DrawSmear();
                }

                if (m_Step > FLASH_START)
                {
                    DrawFlash();
                }

                m_Canvas.EndFrame();
            }

            // The whole frame, centred, growing a little each step as it fades.
            private void DrawGrowing()
            {
                float scale      = 1f + m_Step * 32f / ONE;
                int   brightness = (ZOOM_END - 1 - m_Step) * 8 / 2 & 0xFF;
                float centreX    = RetroCanvas.WIDTH / 2f;
                float centreY    = RetroCanvas.HEIGHT / 2f;

                m_Canvas.Draw(RetroBlend.Add, m_Canvas.Picture);

                DrawGrid(centreX, centreY, scale, scale, brightness, false);

                m_Canvas.EndDraw();
            }

            // Four copies, one a quarter, each the mirror of the one before; they shrink from 1.3 to 0.8 by step 24, then
            // swell to 2.3, brightening until step 32.
            private void DrawPanels()
            {
                float size       = (ONE * 3f / 8f + m_Step * 64f + (m_Step - 32) * (m_Step - 31) / 2 * 8) / ONE;
                int   brightness = Mathf.Min(m_Step * 8, 256);
                float quarterX   = RetroCanvas.WIDTH / 4f;
                float quarterY   = RetroCanvas.HEIGHT / 4f;

                m_Canvas.Draw(RetroBlend.Add, m_Canvas.Picture);

                DrawPanel(-quarterX, -quarterY, size, size, brightness);
                DrawPanel(quarterX, -quarterY, -size, size, brightness);
                DrawPanel(quarterX, quarterY, -size, -size, brightness);
                DrawPanel(-quarterX, quarterY, size, -size, brightness);

                m_Canvas.EndDraw();
            }

            private void DrawPanel(float offsetX, float offsetY, float scaleX, float scaleY, int brightness)
            {
                DrawGrid(RetroCanvas.WIDTH / 2f + offsetX, RetroCanvas.HEIGHT / 2f + offsetY, scaleX, scaleY, brightness, true);
            }

            // The frame as an 8 by 8 grid laid out evenly about a centre, in one brightness, or faded towards its border
            // with each corner lit by the grid lines it sits on.
            private void DrawGrid(float centreX, float centreY, float scaleX, float scaleY, int brightness, bool faded)
            {
                Color flat = RetroCanvas.Textured(brightness);

                for (int row = 0; row < GRID; row++)
                {
                    for (int column = 0; column < GRID; column++)
                    {
                        float left   = column * CELL_WIDTH;
                        float right  = left + CELL_WIDTH;
                        float top    = row * CELL_HEIGHT;
                        float bottom = top + CELL_HEIGHT;

                        Vector2 topLeft     = Place(left, top, centreX, centreY, scaleX, scaleY);
                        Vector2 topRight    = Place(right, top, centreX, centreY, scaleX, scaleY);
                        Vector2 bottomLeft  = Place(left, bottom, centreX, centreY, scaleX, scaleY);
                        Vector2 bottomRight = Place(right, bottom, centreX, centreY, scaleX, scaleY);

                        Vector2 texelTopLeft     = new Vector2(m_TexelLeft[column], m_TexelRows[row]);
                        Vector2 texelTopRight    = new Vector2(m_TexelRight[column], m_TexelRows[row]);
                        Vector2 texelBottomLeft  = new Vector2(m_TexelLeft[column], m_TexelRows[row + 1]);
                        Vector2 texelBottomRight = new Vector2(m_TexelRight[column], m_TexelRows[row + 1]);

                        if (!faded)
                        {
                            m_Canvas.Quad(topLeft, topRight, bottomLeft, bottomRight, texelTopLeft, texelTopRight, texelBottomLeft, texelBottomRight, flat);

                            continue;
                        }

                        m_Canvas.Quad(topLeft, topRight, bottomLeft, bottomRight, texelTopLeft, texelTopRight, texelBottomLeft, texelBottomRight,
                                      Corner(column, row, brightness), Corner(column + 1, row, brightness),
                                      Corner(column, row + 1, brightness), Corner(column + 1, row + 1, brightness));
                    }
                }
            }

            // The frame so far, a little larger and stretched down, laid half over itself.
            private void DrawSmear()
            {
                float scale      = 1f + Mathf.Min(m_Step, PANELS_END) * 4f / ONE;
                float halfWidth  = RetroCanvas.WIDTH / 2f * scale;
                float halfHeight = RetroCanvas.HEIGHT / 2f * scale;
                float centreX    = RetroCanvas.WIDTH / 2f;
                float centreY    = RetroCanvas.HEIGHT / 2f;

                m_Canvas.CopyFrameToScratch();
                m_Canvas.Draw(RetroBlend.Half, m_Canvas.Scratch);
                m_Canvas.Rectangle(centreX - halfWidth, centreY - halfHeight, centreX + halfWidth, centreY + halfHeight,
                                   0f, 0f, RetroCanvas.WIDTH, SMEAR_HEIGHT, RetroCanvas.Textured(128));
                m_Canvas.EndDraw();
            }

            // White added over the screen, rising for sixteen steps and falling as long.
            private void DrawFlash()
            {
                int since = m_Step - FLASH_START;
                int white = Mathf.Clamp(since <= FLASH_RISE ? since * 16 : (FLASH_RISE - since) * 16 + 256, 0, 255);

                m_Canvas.Draw(RetroBlend.Add, null);
                m_Canvas.Rectangle(0f, 0f, RetroCanvas.WIDTH, RetroCanvas.HEIGHT, 0f, 0f, 0f, 0f, RetroCanvas.Flat(white, white, white));
                m_Canvas.EndDraw();
            }

            private static Vector2 Place(float x, float y, float centreX, float centreY, float scaleX, float scaleY)
            {
                Vector2 placed = new Vector2(centreX + (x - RetroCanvas.WIDTH / 2f) * scaleX, centreY + (y - GRID_HEIGHT / 2f) * scaleY);

                return placed;
            }

            // A corner takes the dimmer of the two grid lines it sits on.
            private static Color Corner(int column, int row, int brightness)
            {
                int   edge   = Mathf.Min(m_EdgeBrightness[column], m_EdgeBrightness[row]);
                Color colour = RetroCanvas.Textured(edge * brightness / 256);

                return colour;
            }
        }
    }
}