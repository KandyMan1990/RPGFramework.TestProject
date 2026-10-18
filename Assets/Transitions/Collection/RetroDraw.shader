// Draws a quad as old console hardware did: a texel modulated by the vertex colour, 0.5 leaving it as it is, then
// blended by the material's blend state. The caller works in gamma space, so a source sampled as linear is turned back
// to gamma, and a result written into an sRGB target is turned to linear for the hardware to encode.
Shader "Hidden/Test/Retro Draw"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _SrcBlend ("Source blend", Float) = 1
        _DstBlend ("Destination blend", Float) = 0
        _BlendOp ("Blend operation", Float) = 0
        _Strength ("Strength", Float) = 1
        _Alpha ("Alpha", Float) = 1
        _SourceToGamma ("Source to gamma", Float) = 0
        _OutputToLinear ("Output to linear", Float) = 0
    }

    SubShader
    {
        Cull Off
        ZWrite Off
        ZTest Always
        Blend [_SrcBlend] [_DstBlend]
        BlendOp [_BlendOp]

        Pass
        {
            CGPROGRAM
            #pragma vertex Vertex
            #pragma fragment Fragment

            #include "UnityCG.cginc"

            struct Attributes
            {
                float4 vertex : POSITION;
                float4 colour : COLOR;
                float2 uv     : TEXCOORD0;
            };

            struct Varyings
            {
                float4 position : SV_POSITION;
                float4 colour   : COLOR;
                float2 uv       : TEXCOORD0;
            };

            sampler2D _MainTex;
            float     _Strength;
            float     _Alpha;
            float     _SourceToGamma;
            float     _OutputToLinear;

            Varyings Vertex(Attributes input)
            {
                Varyings output;
                output.position = UnityObjectToClipPos(input.vertex);
                output.colour   = input.colour;
                output.uv       = input.uv;
                return output;
            }

            half4 Fragment(Varyings input) : SV_Target
            {
                float3 texel = tex2D(_MainTex, input.uv).rgb;

                if (_SourceToGamma > 0.5)
                {
                    texel = float3(LinearToGammaSpaceExact(texel.r), LinearToGammaSpaceExact(texel.g), LinearToGammaSpaceExact(texel.b));
                }

                float3 colour = saturate(texel * input.colour.rgb * 2.0 * _Strength);

                if (_OutputToLinear > 0.5)
                {
                    colour = float3(GammaToLinearSpaceExact(colour.r), GammaToLinearSpaceExact(colour.g), GammaToLinearSpaceExact(colour.b));
                }

                return half4(colour, _Alpha);
            }
            ENDCG
        }
    }
}