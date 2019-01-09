// Shader for simulating colorblindness for the whole camera view. 
Shader "SimulationCamera" {
    Properties{
        _MainTex("Base (RGB)", 2D) = "white" {}
        _rg("Red -> Green", Range(-1, 1)) = 0
        _rb("Red -> Blue", Range(-1, 1)) = 0
        _gr("Green -> Red", Range(-1, 1)) = 0
        _gg("Green -> Green", Range(-1, 1)) = 0
        _gb("Green -> Blue", Range(-1, 1)) = 0
        _br("Blue -> Red", Range(-1, 1)) = 0
        _bg("Blue -> Green", Range(-1, 1)) = 0
        _bb("Blue -> Blue", Range(-1, 1)) = 0
    }
    SubShader{
        // Set ZWrite Off for transparent effects
        ZTest Always Cull Off ZWrite Off
        Pass{
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag
            #include "UnityCG.cginc"

            uniform sampler2D _MainTex;
            uniform float _rr;
            uniform float _gr;
            uniform float _br;
            uniform float _rg;
            uniform float _gg;
            uniform float _bg;
            uniform float _rb;
            uniform float _gb;
            uniform float _bb;
            int fullScreen;
            float r;
            float g;
            float b;
            
            float4 frag(v2f_img i) : COLOR{
                float4 c = tex2D(_MainTex, i.uv);
                float3 color = tex2D(_MainTex, i.uv).rgb;
                color = saturate(color);
                
                r = c.r * _rr + c.g * _gr + c.b * _br;
                g = c.r * _rg + c.g * _gg + c.b * _bg;
                b = c.r * _rb + c.g * _gb + c.b * _bb;
                // Put textures for the whole camera view only on fullScreen mode.
                if(fullScreen == 1) {
                    //TODO: render new texture for specific pixels here. Use HSV for finding specific colors.
                    if (color.r > 0.388 & color.g < 0.079 & color.b < 0.079 ){
                        r = 0.0;
                        g = 0.0;
                        b = 1.0;
                    }
                } 
                c.rgb = float3(r, g, b);
                return c;
            }
            ENDCG
        }
    }
}