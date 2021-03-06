// Upgrade NOTE: upgraded instancing buffer 'MyProperties' to new syntax.

Shader "Tiled2Unity/Depth (Instanced)"
{
    Properties
    {
        _MainTex("Tiled Texture", 2D) = "white" {}
        _Color("Tint", Color) = (1,1,1,1)
        _CutOff("Cut off", float) = 0.1
        [MaterialToggle] PixelSnap("Pixel snap", Float) = 0
    }

    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType" = "TransparentCutout"
            "PreviewType" = "Plane"
        }

        Cull Off
        Lighting Off
        ZWrite On
        ZTest LEqual
        Fog{ Mode Off }
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile DUMMY PIXELSNAP_ON
            #include "UnityCG.cginc"
            #include "Tiled2Unity.cginc"

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color : COLOR;
                half2 texcoord  : TEXCOORD0;
            };


            UNITY_INSTANCING_BUFFER_START(MyProperties)
                UNITY_DEFINE_INSTANCED_PROP(float4, _Color)
#define _Color_arr MyProperties
            UNITY_INSTANCING_BUFFER_END(MyProperties)

            v2f vert(appdata_t IN)
            {
                UNITY_SETUP_INSTANCE_ID(IN);
                v2f OUT;
                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.texcoord = IN.texcoord;
                OUT.color = IN.color * UNITY_ACCESS_INSTANCED_PROP(_Color_arr, _Color);

                #ifdef PIXELSNAP_ON
                OUT.vertex = UnityPixelSnap(OUT.vertex);
                #endif

                return OUT;
            }

            sampler2D _MainTex;
            float _CutOff;

            fixed4 frag(v2f IN) : COLOR
            {
                half4 texcol = tex2D(_MainTex, IN.texcoord);
                texcol = texcol * IN.color;

                if (texcol.a < _CutOff)
                    discard;

                return texcol;
            }
            ENDCG
        }
    }

        Fallback "Sprites/Default"
}