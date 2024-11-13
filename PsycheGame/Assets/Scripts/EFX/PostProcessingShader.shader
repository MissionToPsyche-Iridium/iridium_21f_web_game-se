// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "EFX/PostProcessingShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        // No culling or depth
        ZWrite Off 
        ZTest Always
        Lighting Off
        Pass

        {
            CGPROGRAM
            #pragma vertex VShader
            #pragma fragment FShader

            #include "UnityCG.cginc"

           struct VertexToFragment
            {
                float4 pos : SV_POSITION; 
            };

            VertexToFragment VShader(VertexToFragment i) 
            {
                VertexToFragment o;
                o.pos = UnityObjectToClipPos(i.pos);
                return o;
            }

            half4 FShader () : COLOR
            {
                return half4(1, 1, 1, 1);
            }

            ENDCG
        }
    }
}
