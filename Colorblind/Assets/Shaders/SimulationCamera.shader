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
				//color = GammaToLinearSpace(color);
				
				/*
				float r2 = color.r * 17.8824 + color.g * 43.5161 + color.b * 4.11935;
				float g2 = color.r * 3.45565 + color.g * 27.1554 + color.b * 3.86714;
				float b2 = color.r * 0.0299566 + color.g * 0.184309 + color.b * 1.46709;
				float3 v = float3(r2, g2, b2);
				v = saturate(v);
				*/

				r = color.r * _rr + color.g * _gr + color.b * _br;
				g = color.r * _rg + color.g * _gg + color.b * _bg;
				b = color.r * _rb + color.g * _gb + color.b * _bb;

				// Put textures for the whole camera view only on fullScreen mode.
				if(fullScreen == 1) {
					/*
					//TODO: render new texture for specific pixels here. Use HSV for finding specific colors.
					if (color.r > 0.388 & color.g < 0.079 & color.b < 0.079 ){
                        r = 0.0;
                        g = 0.0;
                        b = 1.0;
                    }
                	*/
					float err_r = color.r - r;
					float err_g = color.g - g;
					float err_b = color.b - b;


					float g_shift = 0.7 * err_r + err_g;
					float b_shift = 0.7 * err_r + err_b;

					//r_blind = c_lin.r;
					g = color.g + g_shift;
					b = color.b + b_shift;


                } 
				c.rgb = (saturate(float3(r, g, b)));
				//c.rgb = (LinearToGammaSpace(float3(r, g, b)));

                return c;
            }
            ENDCG
        }
    }
}