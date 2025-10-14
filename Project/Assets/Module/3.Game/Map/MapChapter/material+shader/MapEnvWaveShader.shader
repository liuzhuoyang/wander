Shader "Custom/UnlitWaveScroll"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _Speed ("Scroll Speed (XY)", Vector) = (0.05, 0, 0, 0)
        _WaveAmp ("Wave Amplitude", Range(0,0.05)) = 0.015
        _WaveFreq ("Wave Frequency", Range(0,20)) = 8
        _WaveSpeed ("Wave Speed", Range(0,5)) = 1
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" "CanUseSpriteAtlas"="True" }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off ZWrite Off
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;
            float2 _Speed;
            float _WaveAmp;
            float _WaveFreq;
            float _WaveSpeed;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv  : TEXCOORD0;
                fixed4 col : COLOR;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);

                // 基础滚动
                float2 uv = TRANSFORM_TEX(v.uv, _MainTex);
                uv += _Time.y * _Speed;

                // 加上正弦形起伏
                float wave = sin(uv.x * _WaveFreq + _Time.y * _WaveSpeed) * _WaveAmp;
                uv.y += wave;

                o.uv = uv;
                o.col = v.color * _Color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 c = tex2D(_MainTex, i.uv) * i.col;
                return c;
            }
            ENDCG
        }
    }
}


