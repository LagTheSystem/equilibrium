// Upgrade NOTE: replaced '_LightMatrix0' with 'unity_WorldToLight'

Shader "Custom/Cel"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        [MaterialToggle] _UseShadeTex ("Use Shade Texture", Float) = 0
        _ShadeTex ("Shade Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _DarknessColor ("Darkness Color", Color) = (0.5,0.5,0.5,1)
        _DarknessMidpoint ("Darkness Midpoint", Range(0, 1)) = 0.5
        _ShadowThreshold ("Shadow Threshold", Range(0, 1)) = 0.5
        _ShadeBitDepth ("Shade Bit Depth", Range(0, 15)) = 5
        [Toggle] _FogEnabled ("Fog Enabled", Float) = 0
        _FogColor ("Fog Color", Color) = (0.7, 0.7, 0.8, 1)
        _FogHeight ("Fog Height", Float) = 0.0
        _FogDensity ("Fog Density", Float) = 0.1
    }
    SubShader
    {
        Tags
        {
            "RenderType" = "Opaque"
        }
        
        Pass
        {
            Tags { "LightMode" = "ForwardBase" }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            #include "Cel.hlsl"
            ENDCG
        }

        // https://en.wikibooks.org/wiki/Cg_Programming/Unity/Cookies
        Pass {    
            Tags { "LightMode" = "ForwardAdd" } 

            CGPROGRAM
            #pragma multi_compile_lightpass
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdbase
            #include "Cel.hlsl"
            ENDCG
        }

        UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
    }
}
