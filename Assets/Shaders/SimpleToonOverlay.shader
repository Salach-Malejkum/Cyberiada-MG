Shader "Custom/SimpleToonOverlay"
{
    Properties
    {
        _ToonIntensity ("Toon Intensity", Range(0,1)) = 0.7
        _HighlightColor ("Highlight Color (White)", Color) = (1.3,1.3,1.3,1)
        _HighlightThreshold ("Highlight Threshold", Range(0.6,1.0)) = 0.85
        _MidColor ("Mid Color (Normal)", Color) = (1.0,1.0,1.0,1)
        _ShadowColor ("Shadow Color (Dark)", Color) = (0.2,0.2,0.2,1)
        _ShadowThreshold ("Shadow Threshold", Range(0.0,0.6)) = 0.4
        _MatteIntensity ("Matte Intensity", Range(0,1)) = 0.8
    }
    
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        
        Pass
        {
            Blend DstColor SrcColor
            ZWrite Off
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            
            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
            };
            
            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float3 normalWS : TEXCOORD0;
            };
            
            float _ToonIntensity;
            float4 _HighlightColor;
            float _HighlightThreshold;
            float4 _MidColor;
            float4 _ShadowColor;
            float _ShadowThreshold;
            float _MatteIntensity;
            
            Varyings vert(Attributes input)
            {
                Varyings output;
                output.positionHCS = TransformObjectToHClip(input.positionOS.xyz);
                output.normalWS = TransformObjectToWorldNormal(input.normalOS);
                return output;
            }
            
            float4 frag(Varyings input) : SV_Target
            {
                Light mainLight = GetMainLight();
                float3 lightDir = normalize(mainLight.direction);
                float3 normalWS = normalize(input.normalWS);
                float NdotL = saturate(dot(normalWS, lightDir));
                
                // 3-Step Toon Shading - multiplicative approach for true black shadows
                float3 toonMultiplier;
                
                // Step 1: Shadow - can be black but only with high intensity
                if (NdotL <= _ShadowThreshold)
                {
                    toonMultiplier = _ShadowColor.rgb;
                }
                // Step 2: Mid-tone - between shadow and highlight
                else if (NdotL <= _HighlightThreshold)
                {
                    toonMultiplier = _MidColor.rgb;
                }
                // Step 3: Highlight - top level step - the white highlight
                else
                {
                    // Make it matte
                    toonMultiplier = lerp(_MidColor.rgb, _HighlightColor.rgb, _MatteIntensity);
                }
                
                toonMultiplier = lerp(float3(1,1,1), toonMultiplier, _ToonIntensity);
                
                return float4(toonMultiplier, 1.0);
            }
            ENDHLSL
        }
    }
}
