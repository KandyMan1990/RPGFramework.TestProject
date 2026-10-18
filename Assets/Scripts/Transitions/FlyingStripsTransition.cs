using RPGFramework.Core.Transitions;
using UnityEngine;
using UnityEngine.UIElements;

namespace Test.Transitions
{
    /// <summary>
    /// Half the captured frame torn into strips that fly apart in perspective, each added at a quarter of its brightness
    /// over a picture that keeps every frame drawn, so they smear into trails, with a tinted glow at the tear; then bars
    /// of black grow in from the same side. Eighty steps, a step every two frames at 60, from one side or the other.
    /// A cover only: it keeps no state to run backwards.
    /// </summary>
    [CreateAssetMenu(menuName = "Test/Transitions/Flying Strips", fileName = "FlyingStrips")]
    internal sealed class FlyingStripsTransition : TransitionAsset
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

            TransitionPlayback playback = new StripsPlayback(canvas, screen, Random.Range(0, 2) == 1);

            return playback;
        }

        private sealed class StripsPlayback : TransitionPlayback
        {
            private const int   STEPS           = 80;
            private const int   WIPE_START      = 48;
            private const int   WIPE_RAMP_START = 64;
            private const int   BLANK_FROM      = 78;
            private const int   STRIP_COUNT     = 128;
            private const float STRIP_LEFT      = 32f;
            private const int   BAR_COUNT       = 224;
            private const float BAR_MAXIMUM     = RetroCanvas.WIDTH * 2f;
            private const float SCREEN_DISTANCE = 512f;

            // A little blue, green and red, one a step in turn.
            private static readonly int[] m_GlowTints = { 0x060000, 0x000600, 0x000006 };

            private readonly RetroCanvas m_Canvas;
            private readonly Image       m_Screen;
            private readonly bool        m_Mirrored;
            private readonly float[]     m_StripX;
            private readonly float[]     m_StripSpeedX;
            private readonly float[]     m_StripAccelerationX;
            private readonly float[]     m_StripZ;
            private readonly float[]     m_StripSpeedZ;
            private readonly float[]     m_StripAccelerationZ;
            private readonly float[]     m_BarLength;
            private readonly float[]     m_BarSpeed;
            private readonly int[]       m_BarRed;

            private int m_Step;

            internal StripsPlayback(RetroCanvas canvas, Image screen, bool mirrored)
            {
                m_Canvas             = canvas;
                m_Screen             = screen;
                m_Mirrored           = mirrored;
                m_StripX             = new float[STRIP_COUNT];
                m_StripSpeedX        = new float[STRIP_COUNT];
                m_StripAccelerationX = new float[STRIP_COUNT];
                m_StripZ             = new float[STRIP_COUNT];
                m_StripSpeedZ        = new float[STRIP_COUNT];
                m_StripAccelerationZ = new float[STRIP_COUNT];
                m_BarLength          = new float[BAR_COUNT];
                m_BarSpeed           = new float[BAR_COUNT];
                m_BarRed             = new int[BAR_COUNT];
                m_Step               = 0;

                // Each strip starts further out, faster and accelerating harder than the one before it, and all of them
                // come towards the eye; the mirror is drawn as a reflection, so they are laid out for the left side only.
                for (int i = 0; i < STRIP_COUNT; i++)
                {
                    m_StripX[i]             = i * 0.5f;
                    m_StripSpeedX[i]        = i * 0.125f;
                    m_StripAccelerationX[i] = i / 256f;
                    m_StripSpeedZ[i]        = -1f;
                    m_StripAccelerationZ[i] = -i / 1024f;
                }

                for (int i = 0; i < BAR_COUNT; i++)
                {
                    m_BarLength[i] = Random.Range(0, 48);
                    m_BarSpeed[i]  = Random.Range(0, 48) + 20f;
                    m_BarRed[i]    = Random.Range(0, 16) + 16;
                }
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

                if (m_Step >= BLANK_FROM)
                {
                    m_Canvas.Clear();
                    m_Canvas.EndFrame();

                    return;
                }

                m_Canvas.Draw(RetroBlend.Opaque, m_Canvas.Picture);
                m_Canvas.Rectangle(0f, 0f, RetroCanvas.WIDTH, RetroCanvas.HEIGHT, 0f, 0f, RetroCanvas.WIDTH, RetroCanvas.HEIGHT, RetroCanvas.Textured(128));
                m_Canvas.EndDraw();

                if (m_Step < WIPE_START)
                {
                    DrawGlow();
                    DrawStrips();
                }
                else
                {
                    DrawWipe();
                }

                m_Canvas.EndFrame();

                if (m_Step >= 2 && m_Step < WIPE_START)
                {
                    m_Canvas.CopyFrameToPicture();
                }
            }

            private void DrawStrips()
            {
                float   angle  = m_Step * 4f / 4096f * Mathf.PI * 2f;
                float   cos    = Mathf.Cos(angle);
                float   sin    = Mathf.Sin(angle);
                Vector2 top    = Project(0f, 0f, m_StripZ[0], cos, sin);
                Vector2 bottom = Project(0f, RetroCanvas.HEIGHT, m_StripZ[0], cos, sin);
                float   texel  = 0f;

                m_Canvas.Draw(RetroBlend.AddQuarter, m_Canvas.Picture);

                for (int i = 0; i < STRIP_COUNT; i++)
                {
                    Color colour = RetroCanvas.Textured(m_Step + 64 - i);

                    m_StripSpeedX[i] += m_StripAccelerationX[i];
                    m_StripX[i]      += m_StripSpeedX[i];
                    m_StripSpeedZ[i] += m_StripAccelerationZ[i];
                    m_StripZ[i]      += m_StripSpeedZ[i];

                    float   x          = m_StripX[i] + i + STRIP_LEFT;
                    Vector2 nextTop    = Project(x, 0f, m_StripZ[i], cos, sin);
                    Vector2 nextBottom = Project(x, RetroCanvas.HEIGHT, m_StripZ[i], cos, sin);
                    float   nextTexel  = i + STRIP_LEFT;

                    m_Canvas.Quad(Mirror(top), Mirror(nextTop), Mirror(bottom), Mirror(nextBottom),
                                  Mirror(new Vector2(texel, 0f)), Mirror(new Vector2(nextTexel, 0f)),
                                  Mirror(new Vector2(texel, RetroCanvas.HEIGHT)), Mirror(new Vector2(nextTexel, RetroCanvas.HEIGHT)),
                                  colour);

                    top    = nextTop;
                    bottom = nextBottom;
                    texel  = nextTexel;
                }

                m_Canvas.EndDraw();
            }

            // Four bands of light at the tear, two from the top and bottom edges and two from the middle, growing taller
            // and narrower, bright grey at the tear fading to a tint.
            private void DrawGlow()
            {
                int   tint      = m_GlowTints[m_Step % 3];
                int   grey      = Mathf.Max(4, (32 - m_Step) / 2);
                Color tear      = RetroCanvas.Flat(grey + (tint & 0xFF), grey + ((tint >> 8) & 0xFF), grey + ((tint >> 16) & 0xFF));
                Color edge      = RetroCanvas.Flat(tint & 0xFF, (tint >> 8) & 0xFF, (tint >> 16) & 0xFF);
                int   narrowing = Mathf.Max(4, 32 - m_Step);
                int   growing   = Mathf.Min(m_Step, 32);
                float left      = STRIP_LEFT - m_Step / 2;
                float outer     = narrowing * (narrowing + 1) / 4 + STRIP_LEFT;
                float outerTall = growing * (growing + 1) / 4;
                float inner     = narrowing * (narrowing + 1) / 8 + STRIP_LEFT;
                float innerTall = growing * (growing + 1) / 8;
                float middle    = RetroCanvas.HEIGHT / 2f;

                m_Canvas.Draw(RetroBlend.Add, null);

                Band(left, outer, 0f, outerTall, tear, edge);
                Band(left, outer, RetroCanvas.HEIGHT - outerTall, RetroCanvas.HEIGHT, tear, edge);
                Band(left, inner, middle, middle + innerTall, tear, edge);
                Band(left, inner, middle - innerTall, middle, tear, edge);

                m_Canvas.EndDraw();
            }

            private void Band(float left, float right, float top, float bottom, Color tear, Color edge)
            {
                m_Canvas.Quad(Mirror(new Vector2(left, top)), Mirror(new Vector2(right, top)), Mirror(new Vector2(left, bottom)), Mirror(new Vector2(right, bottom)),
                              Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero,
                              tear, edge, tear, edge);
            }

            // A bar two lines tall for every line, growing in from the side: black at its root, taking away more of the
            // picture at its tip as the wipe strengthens.
            private void DrawWipe()
            {
                int   strength = m_Step >= WIPE_RAMP_START ? m_Step - (WIPE_RAMP_START - 1) : 1;
                Color root     = RetroCanvas.Flat(255, 255, 255);

                m_Canvas.Draw(RetroBlend.Subtract, null);

                for (int i = 0; i < BAR_COUNT; i++)
                {
                    if (m_BarLength[i] >= BAR_MAXIMUM)
                    {
                        m_BarLength[i] = BAR_MAXIMUM;
                        m_BarSpeed[i]  = 0f;
                    }

                    Color tip = RetroCanvas.Flat(strength * m_BarRed[i], strength * 16, strength * 16);

                    m_Canvas.Quad(Mirror(new Vector2(0f, i)), Mirror(new Vector2(m_BarLength[i], i)), Mirror(new Vector2(0f, i + 2f)), Mirror(new Vector2(m_BarLength[i], i + 2f)),
                                  Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero,
                                  root, tip, root, tip);

                    m_BarLength[i] += m_BarSpeed[i];
                }

                m_Canvas.EndDraw();
            }

            // Turned about the vertical by the step's angle, the picture's top left pulled to the screen's centre, and
            // projected onto a plane as far away as the picture, so it starts at its own size.
            private static Vector2 Project(float x, float y, float z, float cos, float sin)
            {
                float worldX = cos * x + sin * z - RetroCanvas.WIDTH / 2f;
                float worldY = y - RetroCanvas.HEIGHT / 2f;
                float worldZ = -sin * x + cos * z + SCREEN_DISTANCE;

                Vector2 projected = new Vector2(RetroCanvas.WIDTH / 2f + worldX * SCREEN_DISTANCE / worldZ, RetroCanvas.HEIGHT / 2f + worldY * SCREEN_DISTANCE / worldZ);

                return projected;
            }

            private Vector2 Mirror(Vector2 point)
            {
                Vector2 mirrored = m_Mirrored ? new Vector2(RetroCanvas.WIDTH - point.x, point.y) : point;

                return mirrored;
            }
        }
    }
}