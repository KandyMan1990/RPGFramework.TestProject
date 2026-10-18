using System;
using RPGFramework.Core.Transitions;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

namespace Test.Transitions
{
    /// <summary>
    /// The captured frame breaking like glass: cracks open outwards from a point near the centre, and each shard, once
    /// free, tumbles towards the eye in perspective, shaded as it turns, until the screen is black. Drawn in 3D with GL
    /// into a texture, so nothing is made in a scene for a change to unload. Worked out from the time alone, so it plays
    /// backwards as well, the glass flying back together; set for about a second and a half, and fading to black over
    /// its last 30%, so a shorter one is cut off sooner.
    /// </summary>
    [CreateAssetMenu(menuName = "Test/Transitions/Shatter", fileName = "Shatter")]
    internal sealed class ShatterTransition : TransitionAsset
    {
        [SerializeField]
        private Shader m_Shader;

        [SerializeField]
        [Min(1)]
        [Tooltip("Shards across the screen, two to a cell")]
        private int m_Columns = 12;

        [SerializeField]
        [Min(1)]
        [Tooltip("Shards down the screen, two to a cell")]
        private int m_Rows = 7;

        protected override bool CapturesFrame => true;

        protected override TransitionPlayback Begin(TransitionContext context)
        {
            Texture       frame   = context.CapturedFrame;
            RenderTexture display = new RenderTexture(frame.width, frame.height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);
            Image         screen  = new Image { image = display, scaleMode = ScaleMode.StretchToFill, pickingMode = PickingMode.Ignore };

            display.Create();

            screen.style.position = Position.Absolute;
            screen.style.left     = 0f;
            screen.style.top      = 0f;
            screen.style.right    = 0f;
            screen.style.bottom   = 0f;

            context.Host.Add(screen);

            TransitionPlayback playback = new ShatterPlayback(new Material(m_Shader), frame, display, screen, m_Columns, m_Rows, Seconds);

            return playback;
        }

        private sealed class ShatterPlayback : TransitionPlayback
        {
            private const float FIELD_OF_VIEW = 40f;
            private const float DISTANCE      = 10f;
            private const float NEAR          = 0.1f;
            private const float GRAVITY       = 9f;
            private const float CRACK_SECONDS = 0.08f;
            private const float CRACK_INSET   = 0.035f;

            private static readonly int     m_MainTexId = Shader.PropertyToID("_MainTex");
            private static readonly Vector3 m_Light     = new Vector3(-0.4f, 0.6f, -1f).normalized;

            private readonly Material      m_Material;
            private readonly Texture       m_Frame;
            private readonly RenderTexture m_Display;
            private readonly Image         m_Screen;
            private readonly float         m_Seconds;
            private readonly Matrix4x4     m_Projection;
            private readonly Vector3[]     m_Corners;
            private readonly Vector2[]     m_Texels;
            private readonly Vector3[]     m_Centres;
            private readonly float[]       m_BreakAt;
            private readonly Vector3[]     m_Velocities;
            private readonly Vector3[]     m_Axes;
            private readonly float[]       m_Spins;
            private readonly float[]       m_Depths;
            private readonly int[]         m_Order;

            internal ShatterPlayback(Material material, Texture frame, RenderTexture display, Image screen, int columns, int rows, float seconds)
            {
                int shards = columns * rows * 2;

                m_Material   = material;
                m_Frame      = frame;
                m_Display    = display;
                m_Screen     = screen;
                m_Seconds    = seconds;
                m_Projection = Matrix4x4.Perspective(FIELD_OF_VIEW, (float)frame.width / frame.height, NEAR, DISTANCE * 4f);
                m_Corners    = new Vector3[shards * 3];
                m_Texels     = new Vector2[shards * 3];
                m_Centres    = new Vector3[shards];
                m_BreakAt    = new float[shards];
                m_Velocities = new Vector3[shards];
                m_Axes       = new Vector3[shards];
                m_Spins      = new float[shards];
                m_Depths     = new float[shards];
                m_Order      = new int[shards];

                m_Material.SetTexture(m_MainTexId, frame);

                BuildShards(columns, rows, (float)frame.width / frame.height);
            }

            protected override void Show(float covered)
            {
                float time = covered * m_Seconds;

                for (int i = 0; i < m_Order.Length; i++)
                {
                    m_Order[i]  = i;
                    m_Depths[i] = -Place(i, m_Centres[i], time).z;
                }

                // Furthest first, so the nearer shards are drawn over them.
                Array.Sort(m_Depths, m_Order);

                RenderTexture previous = RenderTexture.active;

                RenderTexture.active = m_Display;

                GL.Clear(false, true, Color.black);
                GL.PushMatrix();
                GL.LoadProjectionMatrix(m_Projection);
                GL.modelview = Matrix4x4.Scale(new Vector3(1f, 1f, -1f));

                m_Material.SetPass(0);

                GL.Begin(GL.TRIANGLES);

                float fade = 1f - Mathf.SmoothStep(0f, 1f, Mathf.InverseLerp(0.7f, 1f, covered));

                for (int i = 0; i < m_Order.Length; i++)
                {
                    DrawShard(m_Order[i], time, fade);
                }

                GL.End();
                GL.PopMatrix();

                RenderTexture.active = previous;

                m_Screen.MarkDirtyRepaint();
            }

            protected override void End()
            {
                m_Screen.RemoveFromHierarchy();
                m_Display.Release();

                UnityEngine.Object.Destroy(m_Display);
                UnityEngine.Object.Destroy(m_Material);
            }

            // A grid of cells over the picture, its inner corners jittered, each cell split corner to corner either way;
            // the shards nearest a point near the centre break first, and fly out from it.
            private void BuildShards(int columns, int rows, float aspect)
            {
                Vector2[] grid   = new Vector2[(columns + 1) * (rows + 1)];
                Vector2   impact = new Vector2(Random.Range(0.35f, 0.65f), Random.Range(0.35f, 0.6f));
                int       shard  = 0;

                for (int row = 0; row <= rows; row++)
                {
                    for (int column = 0; column <= columns; column++)
                    {
                        bool  inside = row > 0 && row < rows && column > 0 && column < columns;
                        float jitterX = inside ? Random.Range(-0.35f, 0.35f) / columns : 0f;
                        float jitterY = inside ? Random.Range(-0.35f, 0.35f) / rows : 0f;

                        grid[row * (columns + 1) + column] = new Vector2((float)column / columns + jitterX, (float)row / rows + jitterY);
                    }
                }

                for (int row = 0; row < rows; row++)
                {
                    for (int column = 0; column < columns; column++)
                    {
                        Vector2 topLeft     = grid[row * (columns + 1) + column];
                        Vector2 topRight    = grid[row * (columns + 1) + column + 1];
                        Vector2 bottomLeft  = grid[(row + 1) * (columns + 1) + column];
                        Vector2 bottomRight = grid[(row + 1) * (columns + 1) + column + 1];

                        if (Random.value < 0.5f)
                        {
                            AddShard(shard++, topLeft, topRight, bottomRight, impact, aspect);
                            AddShard(shard++, topLeft, bottomRight, bottomLeft, impact, aspect);
                        }
                        else
                        {
                            AddShard(shard++, topLeft, topRight, bottomLeft, impact, aspect);
                            AddShard(shard++, topRight, bottomRight, bottomLeft, impact, aspect);
                        }
                    }
                }
            }

            private void AddShard(int shard, Vector2 a, Vector2 b, Vector2 c, Vector2 impact, float aspect)
            {
                Vector2 centre = (a + b + c) / 3f;
                Vector2 away   = new Vector2((centre.x - impact.x) * aspect, impact.y - centre.y);
                float   reach  = away.magnitude;

                m_Texels[shard * 3]     = new Vector2(a.x, 1f - a.y);
                m_Texels[shard * 3 + 1] = new Vector2(b.x, 1f - b.y);
                m_Texels[shard * 3 + 2] = new Vector2(c.x, 1f - c.y);
                m_Corners[shard * 3]     = OnPicture(a, aspect);
                m_Corners[shard * 3 + 1] = OnPicture(b, aspect);
                m_Corners[shard * 3 + 2] = OnPicture(c, aspect);

                m_Centres[shard]    = OnPicture(centre, aspect);
                m_BreakAt[shard]    = 0.05f + reach * 0.45f;
                m_Velocities[shard] = new Vector3(away.x, away.y, 0f).normalized * Random.Range(2f, 5f) + new Vector3(0f, Random.Range(0f, 2f), -Random.Range(6f, 11f));
                m_Axes[shard]       = Random.onUnitSphere;
                m_Spins[shard]      = Random.Range(180f, 540f);
            }

            private void DrawShard(int shard, float time, float fade)
            {
                float      flying   = Mathf.Max(0f, time - m_BreakAt[shard]);
                float      cracking = Mathf.Clamp01((time - m_BreakAt[shard] + CRACK_SECONDS) / CRACK_SECONDS);
                Quaternion turn     = Quaternion.AngleAxis(m_Spins[shard] * flying, m_Axes[shard]);
                Vector3    centre   = Place(shard, m_Centres[shard], time);

                if (centre.z < NEAR * 4f)
                {
                    return;
                }

                // Lit from above left in front, either face, so a shard flashes as it turns towards the light.
                Vector3 normal  = turn * Vector3.back;
                float   shade   = (0.55f + 0.6f * Mathf.Abs(Vector3.Dot(normal, m_Light))) * fade;
                Color   colour  = new Color(shade, shade, shade, 1f);
                float   inset   = 1f - CRACK_INSET * cracking;

                for (int corner = 0; corner < 3; corner++)
                {
                    Vector3 local = (m_Corners[shard * 3 + corner] - m_Centres[shard]) * inset;

                    GL.Color(colour);
                    GL.TexCoord(m_Texels[shard * 3 + corner]);
                    GL.Vertex(centre + turn * local);
                }
            }

            private Vector3 Place(int shard, Vector3 start, float time)
            {
                float   flying = Mathf.Max(0f, time - m_BreakAt[shard]);
                Vector3 placed = start + m_Velocities[shard] * flying + Vector3.down * (0.5f * GRAVITY * flying * flying);

                return placed;
            }

            // A point on the picture, 0 to 1 from the top left, on a plane far enough away to fill the view.
            private static Vector3 OnPicture(Vector2 point, float aspect)
            {
                float   halfHeight = DISTANCE * Mathf.Tan(FIELD_OF_VIEW * 0.5f * Mathf.Deg2Rad);
                float   halfWidth  = halfHeight * aspect;
                Vector3 placed     = new Vector3((point.x * 2f - 1f) * halfWidth, (1f - point.y * 2f) * halfHeight, DISTANCE);

                return placed;
            }
        }
    }
}