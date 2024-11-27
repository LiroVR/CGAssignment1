Shader "Custom/Distortion" {
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _DistortionStrength ("Distortion Strength", Range (0, 10)) = 1.0
        _DistortionSpeed ("Distortion Speed", Range (0, 1)) = 0.5
    }
    SubShader {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" }
        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float _DistortionStrength;
            float _DistortionSpeed;

            float noise(float2 pos) {
                return frac(sin(dot(pos, float2(12.9898, 78.233))) * 43758.5453);
            }

            v2f vert(appdata v) {
                v2f o;
                float3 worldNormal = normalize(mul((float3x3)unity_ObjectToWorld, v.normal));
                float noiseValue = noise(v.vertex.xy * _DistortionSpeed + _Time.y);
                float3 distortion = worldNormal * noiseValue * _DistortionStrength;
                v.vertex.xyz += distortion;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            half4 frag(v2f i) : SV_Target {
                return tex2D(_MainTex, i.uv);
            }
            ENDCG
        }
    }
}
