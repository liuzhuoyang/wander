Shader "Custom/IslandShoreFoam_Wavy"
{
    Properties
    {
        _MainTex ("Island Sprite", 2D) = "white" {}
        _Color   ("Foam Base Color", Color) = (1,1,1,0.7)

        _BaseExpand ("Base Expand", Range(-0.2,0.2)) = 0.02
        _ExpandAmp  ("Expand Amplitude", Range(0,0.2)) = 0.01
        _ExpandSpeed("Expand Speed", Range(0,5)) = 1.2

        _BandWidth  ("Foam Band Width", Range(0.001,0.2)) = 0.04
        _Softness   ("Edge Softness", Range(0,1)) = 0.6

        _NoiseTex   ("Noise (optional)", 2D) = "gray" {}
        _NoiseScale ("Noise Scale", Range(0,10)) = 3
        _NoiseStrength ("Noise Strength", Range(0,1)) = 0.25
        _NoiseSpeed ("Noise Scroll Speed", Range(0,5)) = 0.6

        _WaveFreq   ("Wave Frequency", Range(0,20)) = 8
        _WaveAmp    ("Wave Amplitude", Range(0,0.05)) = 0.01
        _WaveSpeed  ("Wave Speed", Range(0,5)) = 1.5
    }

    SubShader
    {
        Tags{
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "CanUseSpriteAtlas"="True"
        }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex   vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex; float4 _MainTex_ST;
            fixed4 _Color;

            float _BaseExpand, _ExpandAmp, _ExpandSpeed;
            float _BandWidth, _Softness;
            sampler2D _NoiseTex; float4 _NoiseTex_ST;
            float _NoiseScale, _NoiseStrength, _NoiseSpeed;
            float _WaveFreq, _WaveAmp, _WaveSpeed;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv     : TEXCOORD0;
                fixed4 color  : COLOR;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv  : TEXCOORD0;
                fixed4 col : COLOR;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv  = TRANSFORM_TEX(v.uv, _MainTex);
                o.col = v.color * _Color;
                return o;
            }

            float2 ScaleUV(float2 uv, float expand)
            {
                float2 c = float2(0.5, 0.5);
                return (uv - c) * (1.0 - expand) + c;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // 基础 alpha
                float a0 = tex2D(_MainTex, i.uv).a;

                // 全局呼吸扩张
                float baseExpand = _BaseExpand + sin(_Time.y * _ExpandSpeed) * _ExpandAmp;

                // ✨ 局部波动扰动：沿 uv.x 方向正弦起伏
                float localWave = sin(i.uv.x * _WaveFreq + _Time.y * _WaveSpeed) * _WaveAmp;

                // 总扩张量 = 基础扩张 + 局部波动
                float expand = baseExpand + localWave;

                // 取外、内边界
                float aOuter = tex2D(_MainTex, ScaleUV(i.uv, expand)).a;
                float aInner = tex2D(_MainTex, ScaleUV(i.uv, expand - _BandWidth)).a;

                // 环带
                float ring = saturate(aOuter - aInner);
                ring = pow(ring, 1.0 - _Softness);

                // 噪声扰动
                float2 nUV = i.uv * _NoiseScale + float2(_Time.y * _NoiseSpeed, 0);
                float n = tex2D(_NoiseTex, nUV).r;
                ring *= (1.0 - _NoiseStrength) + n * _NoiseStrength;

                fixed4 col = i.col;
                col.a *= ring;
                return col;
            }
            ENDCG
        }
    }
}
