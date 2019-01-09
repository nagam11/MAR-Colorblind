// Shader for the glass part of the magnifying glass.
Shader "MagnifyingGlass"
{
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
    SubShader
    {
        // ShaderLab: SubShader tags for rendering as transparent
        Tags { "Queue" = "Transparent" }

        // ShaderLab: Grab the screen behind the object, in this case the camera screen
        GrabPass
        {
            "_BackgroundTexture"
        }
     
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            uniform float _rr;
            uniform float _gr;
            uniform float _br;
            uniform float _rg;
            uniform float _gg;
            uniform float _bg;
            uniform float _rb;
            uniform float _gb;
            uniform float _bb;
            float r;
            float g;
            float b;

            struct v2f
            {
                float4 grabPos : TEXCOORD0;
                float4 pos : SV_POSITION;
            };

            v2f vert(appdata_base v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.grabPos = ComputeGrabScreenPos(o.pos);
                return o;
            }
            
            // Use captured background texture from camera.
            sampler2D _BackgroundTexture;

            half4 frag(v2f i) : SV_Target
            {
                half4 c = tex2Dproj(_BackgroundTexture, i.grabPos);
                float3 color = c.rgb;
                color = saturate(color);
                //TODO: render new texture for specific pixels here. Use HSV for finding specific colors.
                if (color.r > 0.388 & color.g < 0.079 & color.b < 0.079 ){
                    r = 0.0;
                    g = 0.0;
                    b = 1.0;
                } else {
                    r = c.r * _rr + c.g * _gr + c.b * _br;
                    g = c.r * _rg + c.g * _gg + c.b * _bg;
                    b = c.r * _rb + c.g * _gb + c.b * _bb;
                }
                float3 shaded = float3(r, g, b);
                float4 result = c;
                result.rgb = shaded;
                return result;
            }
            ENDCG
        }
    }
}
