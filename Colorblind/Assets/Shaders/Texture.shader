// Shader for the glass part of the magnifying glass.
Shader "MagnifyingGlass"
{
Properties{
        _MainTex("Base (RGB)", 2D) = "white" {}
        _rg("Red -> Green", Range(0, 1)) = 0
        _rb("Red -> Blue", Range(0, 1)) = 0
        _gr("Green -> Red", Range(0, 1)) = 0
        _gg("Green -> Green", Range(0, 1)) = 0
        _gb("Green -> Blue", Range(0, 1)) = 0
        _br("Blue -> Red", Range(0, 1)) = 0
        _bg("Blue -> Green", Range(0, 1)) = 0
        _bb("Blue -> Blue", Range(0, 1)) = 0

		_erg("Red -> Green", Range(0, 1)) = 0
		_erb("Red -> Blue", Range(0, 1)) = 0
		_egr("Green -> Red", Range(0, 1)) = 0
		_egg("Green -> Green", Range(0, 1)) = 0
		_egb("Green -> Blue", Range(0, 1)) = 0
		_ebr("Blue -> Red", Range(0, 1)) = 0
		_ebg("Blue -> Green", Range(0, 1)) = 0
		_ebb("Blue -> Blue", Range(0, 1)) = 0
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
			uniform float _err;
			uniform float _egr;
			uniform float _ebr;
			uniform float _erg;
			uniform float _egg;
			uniform float _ebg;
			uniform float _erb;
			uniform float _egb;
			uniform float _ebb;
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
            
            // Taken from here: http://lolengine.net/blog/2013/07/27/rgb-to-hsv-in-glsl
            vec3 rgb2hsv(vec3 c)
            {
                vec4 K = vec4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
                vec4 p = mix(vec4(c.bg, K.wz), vec4(c.gb, K.xy), step(c.b, c.g));
                vec4 q = mix(vec4(p.xyw, c.r), vec4(c.r, p.yzx), step(p.x, c.r));

                float d = q.x - min(q.w, q.y);
                float e = 1.0e-10;
                return vec3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
            }
            
            vec3 hsv2rgb(vec3 c)
            {
                vec4 K = vec4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
                vec3 p = abs(fract(c.xxx + K.xyz) * 6.0 - K.www);
                return c.z * mix(K.xxx, clamp(p - K.xxx, 0.0, 1.0), c.y);
            }
            
            // Use captured background texture from camera.
            sampler2D _BackgroundTexture;

            half4 frag(v2f i) : SV_Target
            {
                half4 c = tex2Dproj(_BackgroundTexture, i.grabPos);
                float3 color = c.rgb;
                color = saturate(color);

				color = GammaToLinearSpace(color);

				//TODO: doesn't work with normal vision. it needs parameters of which colorblind has to simulate
				
				r = color.r * _rr + color.g * _gr + color.b * _br;
				g = color.r * _rg + color.g * _gg + color.b * _bg;
				b = color.r * _rb + color.g * _gb + color.b * _bb;

				float err_r = color.r - r;
				float err_g = color.g - g;
				float err_b = color.b - b;

				float err_rate = 3;

				err_r *= err_rate;
				err_g *= err_rate;
				err_b *= err_rate;


				float r_shift = err_r * _err + err_g * _egr + err_b * _ebr;
				float g_shift = err_r * _erg + err_g * _egg + err_b * _ebg;
				float b_shift = err_r * _erb + err_g * _egb + err_b * _ebb;

				r = color.r + r_shift;
				g = color.g + g_shift;
				b = color.b + b_shift;

				//c.rgb = (saturate(float3(err_r, err_g, err_b)));
				c.rgb = LinearToGammaSpace(saturate(float3(r, g, b)));
                
                //TODO: render new texture for specific pixels here. Use HSV for finding specific colors.
                if (color.r > 0.388 & color.g < 0.079 & color.b < 0.079 ){
                    r = 0.0;
                    g = 0.0;
                    b = 1.0;
                    c.rgb = float3(r, g, b);
                }
                
                return c;
            }
            ENDCG
        }
    }
}
