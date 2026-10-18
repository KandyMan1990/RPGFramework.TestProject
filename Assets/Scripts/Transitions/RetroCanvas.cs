using UnityEngine;
using UnityEngine.Rendering;

namespace Test.Transitions
{
    /// <summary>How a quad is combined with what is already drawn, as old console hardware offered.</summary>
    internal enum RetroBlend
    {
        Opaque,
        Half,
        Add,
        Subtract,
        AddQuarter
    }

    /// <summary>
    /// A 320 by 224 screen drawn as old console hardware drew it, in the real screen's resolution: quads in screen
    /// pixels, top left the origin, each texel modulated by its vertex colour and blended in gamma space, saturating.
    /// It holds the picture a transition started from, which a transition may draw back over to build on its last frame,
    /// and shows what it draws in <see cref="Display" />.
    /// </summary>
    internal sealed class RetroCanvas
    {
        internal const float WIDTH  = 320f;
        internal const float HEIGHT = 224f;

        private static readonly int m_MainTexId        = Shader.PropertyToID("_MainTex");
        private static readonly int m_SrcBlendId       = Shader.PropertyToID("_SrcBlend");
        private static readonly int m_DstBlendId       = Shader.PropertyToID("_DstBlend");
        private static readonly int m_BlendOpId        = Shader.PropertyToID("_BlendOp");
        private static readonly int m_StrengthId       = Shader.PropertyToID("_Strength");
        private static readonly int m_AlphaId          = Shader.PropertyToID("_Alpha");
        private static readonly int m_SourceToGammaId  = Shader.PropertyToID("_SourceToGamma");
        private static readonly int m_OutputToLinearId = Shader.PropertyToID("_OutputToLinear");

        private readonly Material[]    m_Materials;
        private readonly RenderTexture m_Frame;
        private readonly RenderTexture m_Picture;
        private readonly RenderTexture m_Scratch;
        private readonly RenderTexture m_Display;
        private readonly bool          m_Linear;

        private RenderTexture m_Previous;

        internal RetroCanvas(Shader shader, Texture capturedFrame)
        {
            m_Linear    = QualitySettings.activeColorSpace == ColorSpace.Linear;
            m_Materials = new Material[5];
            m_Frame     = CreateTarget(capturedFrame, RenderTextureReadWrite.Linear);
            m_Picture   = CreateTarget(capturedFrame, RenderTextureReadWrite.Linear);
            m_Scratch   = CreateTarget(capturedFrame, RenderTextureReadWrite.Linear);
            m_Display   = CreateTarget(capturedFrame, RenderTextureReadWrite.sRGB);
            m_Previous  = null;

            m_Materials[(int)RetroBlend.Opaque]     = CreateMaterial(shader, BlendMode.One,      BlendMode.Zero,             BlendOp.Add,             1f,    1f);
            m_Materials[(int)RetroBlend.Half]       = CreateMaterial(shader, BlendMode.SrcAlpha, BlendMode.OneMinusSrcAlpha, BlendOp.Add,             1f,    0.5f);
            m_Materials[(int)RetroBlend.Add]        = CreateMaterial(shader, BlendMode.One,      BlendMode.One,              BlendOp.Add,             1f,    1f);
            m_Materials[(int)RetroBlend.Subtract]   = CreateMaterial(shader, BlendMode.One,      BlendMode.One,              BlendOp.ReverseSubtract, 1f,    1f);
            m_Materials[(int)RetroBlend.AddQuarter] = CreateMaterial(shader, BlendMode.One,      BlendMode.One,              BlendOp.Add,             0.25f, 1f);

            Begin(m_Picture);
            Draw(RetroBlend.Opaque, capturedFrame, m_Linear && capturedFrame.isDataSRGB);
            Rectangle(0f, 0f, WIDTH, HEIGHT, 0f, 0f, WIDTH, HEIGHT, Textured(128));
            EndDraw();
            End();

            Present(m_Picture);
        }

        /// <summary>What the canvas shows: its last frame, in the real screen's colour space.</summary>
        internal Texture Display => m_Display;

        /// <summary>The picture as it stands, a frame's own texture to draw from.</summary>
        internal Texture Picture => m_Picture;

        /// <summary>The frame drawn so far, copied by <see cref="CopyFrameToScratch" /> to draw from.</summary>
        internal Texture Scratch => m_Scratch;

        /// <summary>A vertex colour for a textured quad: 128 draws the texel as it is, 255 nearly doubles it.</summary>
        internal static Color Textured(int value)
        {
            float half   = Mathf.Clamp(value, 0, 255) / 256f;
            Color colour = new Color(half, half, half, 1f);

            return colour;
        }

        /// <summary>A vertex colour for a quad with no texture, each channel 0 to 255 as it is drawn.</summary>
        internal static Color Flat(int red, int green, int blue)
        {
            Color colour = new Color(Mathf.Clamp(red, 0, 255) / 510f, Mathf.Clamp(green, 0, 255) / 510f, Mathf.Clamp(blue, 0, 255) / 510f, 1f);

            return colour;
        }

        /// <summary>Starts drawing a frame over a clear or over the picture, as the caller draws first.</summary>
        internal void BeginFrame()
        {
            Begin(m_Frame);
        }

        internal void Clear()
        {
            GL.Clear(false, true, Color.black);
        }

        /// <summary>
        /// Starts a batch of quads drawn with <paramref name="blend" /> from <paramref name="texture" />, white for none.
        /// </summary>
        internal void Draw(RetroBlend blend, Texture texture)
        {
            Draw(blend, texture, false);
        }

        /// <summary>A quad's corner in screen pixels, its texel in the texture's own pixels at the 320 by 224 scale.</summary>
        internal void Vertex(Vector2 position, Vector2 texel, Color colour)
        {
            GL.Color(colour);
            GL.TexCoord2(texel.x / WIDTH, 1f - texel.y / HEIGHT);
            GL.Vertex3(position.x, position.y, 0f);
        }

        /// <summary>
        /// A quad given as the hardware took them — top left, top right, bottom left, bottom right — in one colour.
        /// </summary>
        internal void Quad(Vector2 topLeft, Vector2 topRight, Vector2 bottomLeft, Vector2 bottomRight,
                           Vector2 texelTopLeft, Vector2 texelTopRight, Vector2 texelBottomLeft, Vector2 texelBottomRight, Color colour)
        {
            Quad(topLeft, topRight, bottomLeft, bottomRight, texelTopLeft, texelTopRight, texelBottomLeft, texelBottomRight, colour, colour, colour, colour);
        }

        internal void Quad(Vector2 topLeft, Vector2 topRight, Vector2 bottomLeft, Vector2 bottomRight,
                           Vector2 texelTopLeft, Vector2 texelTopRight, Vector2 texelBottomLeft, Vector2 texelBottomRight,
                           Color colourTopLeft, Color colourTopRight, Color colourBottomLeft, Color colourBottomRight)
        {
            Vertex(topLeft, texelTopLeft, colourTopLeft);
            Vertex(topRight, texelTopRight, colourTopRight);
            Vertex(bottomRight, texelBottomRight, colourBottomRight);
            Vertex(bottomLeft, texelBottomLeft, colourBottomLeft);
        }

        /// <summary>An upright rectangle in screen pixels showing a rectangle of texels, in one colour.</summary>
        internal void Rectangle(float left, float top, float right, float bottom,
                                float texelLeft, float texelTop, float texelRight, float texelBottom, Color colour)
        {
            Quad(new Vector2(left, top), new Vector2(right, top), new Vector2(left, bottom), new Vector2(right, bottom),
                 new Vector2(texelLeft, texelTop), new Vector2(texelRight, texelTop), new Vector2(texelLeft, texelBottom), new Vector2(texelRight, texelBottom),
                 colour);
        }

        /// <summary>Ends a batch of quads.</summary>
        internal void EndDraw()
        {
            GL.End();
        }

        /// <summary>Ends the frame and shows it.</summary>
        internal void EndFrame()
        {
            End();
            Present(m_Frame);
        }

        /// <summary>Makes the frame just drawn the picture the next frame builds on.</summary>
        internal void CopyFrameToPicture()
        {
            Graphics.CopyTexture(m_Frame, m_Picture);
        }

        /// <summary>Copies the frame drawn so far, so it can be drawn from while it is drawn into.</summary>
        internal void CopyFrameToScratch()
        {
            End();
            Graphics.CopyTexture(m_Frame, m_Scratch);
            Begin(m_Frame);
        }

        internal void Release()
        {
            for (int i = 0; i < m_Materials.Length; i++)
            {
                Object.Destroy(m_Materials[i]);
            }

            Release(m_Frame);
            Release(m_Picture);
            Release(m_Scratch);
            Release(m_Display);
        }

        private void Present(Texture source)
        {
            Begin(m_Display);
            m_Materials[(int)RetroBlend.Opaque].SetFloat(m_OutputToLinearId, m_Linear ? 1f : 0f);
            Draw(RetroBlend.Opaque, source, false);
            Rectangle(0f, 0f, WIDTH, HEIGHT, 0f, 0f, WIDTH, HEIGHT, Textured(128));
            EndDraw();
            End();
            m_Materials[(int)RetroBlend.Opaque].SetFloat(m_OutputToLinearId, 0f);
        }

        private void Draw(RetroBlend blend, Texture texture, bool sourceToGamma)
        {
            Material material = m_Materials[(int)blend];

            material.SetTexture(m_MainTexId, texture != null ? texture : Texture2D.whiteTexture);
            material.SetFloat(m_SourceToGammaId, sourceToGamma ? 1f : 0f);
            material.SetPass(0);

            GL.Begin(GL.QUADS);
        }

        private void Begin(RenderTexture target)
        {
            m_Previous           = RenderTexture.active;
            RenderTexture.active = target;

            GL.PushMatrix();
            GL.LoadPixelMatrix(0f, WIDTH, HEIGHT, 0f);
        }

        private void End()
        {
            GL.PopMatrix();

            RenderTexture.active = m_Previous;
        }

        private static RenderTexture CreateTarget(Texture size, RenderTextureReadWrite readWrite)
        {
            RenderTexture target = new RenderTexture(size.width, size.height, 0, RenderTextureFormat.ARGB32, readWrite);

            target.Create();

            return target;
        }

        private static Material CreateMaterial(Shader shader, BlendMode source, BlendMode destination, BlendOp operation, float strength, float alpha)
        {
            Material material = new Material(shader);

            material.SetFloat(m_SrcBlendId, (float)source);
            material.SetFloat(m_DstBlendId, (float)destination);
            material.SetFloat(m_BlendOpId, (float)operation);
            material.SetFloat(m_StrengthId, strength);
            material.SetFloat(m_AlphaId, alpha);

            return material;
        }

        private static void Release(RenderTexture target)
        {
            target.Release();
            Object.Destroy(target);
        }
    }
}