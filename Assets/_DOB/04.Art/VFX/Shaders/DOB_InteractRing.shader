// 상호작용할 수 있는 자리(화구 앞 등) 바닥에 까는 둥근 표시. 따뜻한 크림·금빛 고리가 천천히 숨 쉬듯 밝아졌다 어두워지고,
// 안쪽으로 옅은 빛이 번진다. 켜고 끄기는 게임 쪽(가까이 왔을 때)이 이 오브젝트를 켜는 것으로 한다.
// 판은 위를 보는 Quad, UV 가운데(0.5, 0.5)가 고리 중심이다. 시간은 _Time을 쓰므로 스크립트가 없어도 움직인다.
//
// 의사코드
//   입력: UV, 시간
//   1. 중심에서의 거리 r을 구한다.
//   2. 고리: r이 _Radius 근처(두께 _Width)일 때 1, 경계는 _Soft로 부드럽게.
//   3. 안쪽 빛: r이 0~_Radius 사이에서 바깥으로 갈수록 진해지는 옅은 채움(_FillAlpha).
//   4. 숨쉬기: sin(시간 * _PulseSpeed)로 0~1을 만들어 고리 밝기와 반지름을 조금씩 바꾼다.
//   5. 고리 바깥(_Radius + _Width 이상)은 0. 최종 색 = _Color, 알파 = (고리 + 안쪽 빛) * 숨쉬기 * _Alpha.
Shader "DOB/InteractRing"
{
    Properties
    {
        _Color ("고리 색", Color) = (1, 0.86, 0.55, 1)
        _Radius ("고리 반지름(UV)", Range(0.05, 0.5)) = 0.4
        _Width ("고리 두께(UV)", Range(0.005, 0.2)) = 0.045
        _Soft ("경계 부드러움", Range(0.001, 0.1)) = 0.02
        _FillAlpha ("안쪽 빛 세기(0~1)", Range(0, 1)) = 0.22
        _Alpha ("전체 불투명도(0~1)", Range(0, 1)) = 0.9
        _PulseSpeed ("숨쉬기 속도(라디안/초)", Range(0, 10)) = 3
        _PulseAmount ("숨쉬기 밝기 폭(0~1)", Range(0, 1)) = 0.35
        _PulseGrow ("숨쉬기 반지름 폭(UV)", Range(0, 0.1)) = 0.02
    }
    SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent" "RenderPipeline" = "UniversalPipeline" "IgnoreProjector" = "True" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            Name "InteractRing"
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            CBUFFER_START(UnityPerMaterial)
                half4 _Color;
                float _Radius, _Width, _Soft, _FillAlpha, _Alpha, _PulseSpeed, _PulseAmount, _PulseGrow;
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

            half4 frag(Varyings i) : SV_Target
            {
                float r = length(i.uv - 0.5);
                // 0~1로 오가는 숨쉬기
                float pulse = 0.5 + 0.5 * sin(_Time.y * _PulseSpeed);
                float radius = _Radius + _PulseGrow * pulse;
                float halfW = _Width * 0.5;
                float ring = smoothstep(halfW + _Soft, halfW, abs(r - radius));
                // 안쪽 빛은 가장자리 쪽으로 갈수록 진해져 바닥에 고리가 '깔린' 느낌을 준다
                float fill = _FillAlpha * smoothstep(0.0, radius, r) * step(r, radius);
                float brightness = 1.0 - _PulseAmount + _PulseAmount * pulse;
                float a = saturate(ring + fill) * brightness * _Alpha;
                return half4(_Color.rgb, a);
            }
            ENDHLSL
        }
    }
}
