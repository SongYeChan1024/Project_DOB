// 화구 판 위 냄비 둘레에 그리는 만화풍 불빛 고리. 노랑 → 주황 → 어두운 빨강 세 단계 띠로 또렷하게 끊고,
// 바깥 테두리는 혀처럼 일렁이며, 띠 무늬는 시간이 지날수록 냄비 쪽으로 빨려 들어간다.
// 세기(_Intensity)와 불 시간(_FireTime)은 StoveFire가 머티리얼 프로퍼티 블록으로 넘긴다(조리 시계와 같이 흐름).
// 판은 위를 보는 Quad, UV 가운데(0.5, 0.5)가 냄비 중심이다.
Shader "DOB/ToonFireRing"
{
    Properties
    {
        _ColorCore ("안쪽 띠 색", Color) = (1, 0.92, 0.45, 1)
        _ColorMid ("가운데 띠 색", Color) = (1, 0.55, 0.12, 1)
        _ColorOuter ("바깥 띠 색", Color) = (0.75, 0.18, 0.05, 1)
        _Inner ("안쪽 반지름(UV, 냄비 둘레)", Range(0, 0.5)) = 0.29
        _Outer ("최대 바깥 반지름(UV)", Range(0, 0.5)) = 0.44
        _Band1 ("안쪽/가운데 경계(0~1)", Range(0, 1)) = 0.32
        _Band2 ("가운데/바깥 경계(0~1)", Range(0, 1)) = 0.66
        _Alpha ("띠별 불투명도(안쪽,가운데,바깥)", Vector) = (0.95, 0.85, 0.7, 0)
        _Wobble ("테두리 일렁임 폭(UV)", Range(0, 0.15)) = 0.05
        _Tongues ("테두리 혀 개수", Range(1, 24)) = 9
        _FlowSpeed ("빨려 드는 속도", Range(0, 5)) = 1.3
        _FlowScale ("띠 흔들림 촘촘함", Range(0, 30)) = 12
        _FlowAmount ("띠 경계 흔들림 폭(0~1)", Range(0, 0.5)) = 0.12
        _Soft ("경계 부드러움", Range(0.001, 0.1)) = 0.012
        _Intensity ("세기(스크립트가 넣음)", Float) = 0
        _FireTime ("불 시간(스크립트가 넣음)", Float) = 0
    }
    SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent" "RenderPipeline" = "UniversalPipeline" "IgnoreProjector" = "True" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            Name "ToonFireRing"
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            CBUFFER_START(UnityPerMaterial)
                half4 _ColorCore, _ColorMid, _ColorOuter;
                float _Inner, _Outer, _Band1, _Band2;
                float4 _Alpha;
                float _Wobble, _Tongues, _FlowSpeed, _FlowScale, _FlowAmount, _Soft;
                float _Intensity, _FireTime;
            CBUFFER_END

            struct Attributes { float4 positionOS : POSITION; float2 uv : TEXCOORD0; };
            struct Varyings { float4 positionCS : SV_POSITION; float2 uv : TEXCOORD0; };

            Varyings vert(Attributes v)
            {
                Varyings o;
                o.positionCS = TransformObjectToHClip(v.positionOS.xyz);
                o.uv = v.uv;
                return o;
            }

            // 값 노이즈: 격자 꼭짓점 난수를 부드럽게 보간한다.
            float Hash(float2 p) { return frac(sin(dot(p, float2(127.1, 311.7))) * 43758.5453); }
            float Noise(float2 p)
            {
                float2 i = floor(p), f = frac(p);
                float2 u = f * f * (3.0 - 2.0 * f);
                return lerp(lerp(Hash(i), Hash(i + float2(1, 0)), u.x), lerp(Hash(i + float2(0, 1)), Hash(i + float2(1, 1)), u.x), u.y);
            }
            // 각도 방향은 한 바퀴 돌아 이어져야 하므로 원 위의 점(cos, sin)으로 노이즈를 읽는다.
            float RingNoise(float ang, float scale, float t)
            {
                float2 c = float2(cos(ang), sin(ang)) * scale;
                return Noise(c + float2(t, -t * 0.7));
            }

            half4 frag(Varyings i) : SV_Target
            {
                float intensity = saturate(_Intensity);
                if (intensity <= 0.001) return 0;

                float2 p = i.uv - 0.5;
                float r = length(p);
                float ang = atan2(p.y, p.x);

                // 세기가 낮으면 고리가 얇아지고 높으면 넓어진다.
                float reach = lerp(_Inner + 0.02, _Outer, intensity);
                // 혀처럼 날름이는 바깥 테두리(두 겹 노이즈로 크고 작은 혀를 섞는다).
                float tongue = RingNoise(ang, _Tongues * 0.35, _FireTime * 1.7) * 0.7 + RingNoise(ang, _Tongues, _FireTime * 3.1) * 0.3;
                float edge = reach + (tongue - 0.5) * 2.0 * _Wobble * (0.4 + 0.6 * intensity);

                float t = (r - _Inner) / max(edge - _Inner, 0.001);   // 0 = 냄비 쪽, 1 = 바깥 테두리
                // 같은 무늬가 시간이 지날수록 r이 줄어드는 쪽(냄비 쪽)으로 움직인다.
                float flow = Noise(float2(r * _FlowScale + _FireTime * _FlowSpeed, ang * 2.0));
                t += (flow - 0.5) * 2.0 * _FlowAmount;

                float inside = smoothstep(1.0 + _Soft * 8, 1.0 - _Soft * 8, t) * smoothstep(-0.05, 0.0, (r - _Inner) / max(_Outer, 0.001));
                float b1 = smoothstep(_Band1 - _Soft * 8, _Band1 + _Soft * 8, t);
                float b2 = smoothstep(_Band2 - _Soft * 8, _Band2 + _Soft * 8, t);
                half3 col = lerp(lerp(_ColorCore.rgb, _ColorMid.rgb, b1), _ColorOuter.rgb, b2);
                float a = lerp(lerp(_Alpha.x, _Alpha.y, b1), _Alpha.z, b2) * inside * saturate(intensity * 1.6);
                return half4(col, a);
            }
            ENDHLSL
        }
    }
}
