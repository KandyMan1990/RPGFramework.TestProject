// A shard of the captured frame: its texel times the vertex colour, drawn as it comes, nearest last.
Shader "Hidden/Test/Shard"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }

    SubShader
    {
        Cull Off
        ZWrite Off
        ZTest Always

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
                half3 colour = tex2D(_MainTex, input.uv).rgb * input.colour.rgb;

                return half4(colour, 1.0);
            }
            ENDCG
        }
    }
}