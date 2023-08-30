// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Silver Tau/Standard/Shadow Receiver"
{
    Properties
    {
        [Header(Main settings)]
        [Space(10)]
        _Color ("Color", Color) = (0,0,0,1)
        _ShadowIntensity ("Shadow Intensity", Range (0, 1)) = 0.6
        [Header(Advanced settings)]
        [Space(10)]
        _Hue ("Hue", Range(-360, 360)) = 0.
        _Brightness ("Brightness", Range(-1, 1)) = 0.
        _Contrast("Contrast", Range(0, 2)) = 1
        _Saturation("Saturation", Range(0, 2)) = 1
    }
 
    SubShader
    {
 
        Tags {"Queue"="Geometry" }
 
        Pass
        {
            Tags {"LightMode" = "ForwardBase" }
            Cull Back
            Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdbase
 
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            uniform fixed4  _Color;
            uniform float _ShadowIntensity;
            float _Hue;
            float _Brightness;
            float _Contrast;
            float _Saturation;
 
            struct v2f
            {
                float4 pos : SV_POSITION;
                LIGHTING_COORDS(0,1)
            };
            v2f vert(appdata_base v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos (v.vertex);
                TRANSFER_VERTEX_TO_FRAGMENT(o);
             
                return o;
            }
            inline float3 applyHue(float3 aColor, float aHue)
            {
                float angle = radians(aHue);
                float3 k = float3(0.57735, 0.57735, 0.57735);
                float cosAngle = cos(angle);
                //Rodrigues' rotation formula
                return aColor * cosAngle + cross(k, aColor) * sin(angle) + k * dot(k, aColor) * (1 - cosAngle);
            }
            inline float4 applyHSBEffect(float4 startColor)
            {
                float4 outputColor = startColor;
                outputColor.rgb = applyHue(outputColor.rgb, _Hue);
                outputColor.rgb = (outputColor.rgb - 0.5f) * (_Contrast)+0.5f;
                outputColor.rgb = outputColor.rgb + _Brightness;
                float3 intensity = dot(outputColor.rgb, float3(0.299, 0.587, 0.114));
                outputColor.rgb = lerp(intensity, outputColor.rgb, _Saturation);
                return outputColor;
            }
            
            fixed4 frag(v2f i) : COLOR
            {
                float attenuation = LIGHT_ATTENUATION(i);
                fixed4 color = fixed4(1,1,1,(1-attenuation)*_ShadowIntensity) * _Color;
                float4 hsbColor = applyHSBEffect(color);
                return hsbColor;
            }
            ENDCG
        }
 
    }
    Fallback "VertexLit"
}