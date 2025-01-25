Shader "Custom/PlayerColor"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1, 1, 1, 1)
    }
    SubShader
    {
        Tags {"Queue"="Transparent" "RenderType"="Transparent"}
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4  _Color;
            struct a2v
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 texcoord : TEXCOORD0;
            };

            v2f vert (a2v v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.texcoord;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 currentColor = tex2D(_MainTex, i.texcoord);

                float3 targetColor = float3(0.06836f, 0.06641f, 0.14746f);
                float epsilon = 0.05f;

                if (abs(currentColor.r - targetColor.r) < epsilon &&
                    abs(currentColor.g - targetColor.g) < epsilon &&
                    abs(currentColor.b - targetColor.b) < epsilon)
                {
                    return _Color;
                }

                return currentColor;
            }

            ENDCG
        }
    }
}
