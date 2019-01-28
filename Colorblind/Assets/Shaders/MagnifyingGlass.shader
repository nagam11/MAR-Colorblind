// Shader for the glass part of the magnifying glass.
Shader "MagnifyingGlass"
{
Properties{
        _MainTex("Base (RGB)", 2D) = "white" {}
        // Grayscale level
        _bwBlend ("Black & White blend", Range (0, 1)) = 0.8 
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
            uniform float _bwBlend;
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
            // 0: Magnifying glass mode 1: Full screen mode
            int fullScreen;
            // 0: Daltonization 1: ColorPopper 2: Texture
            int correctionMethod;
            // 0: red 1: green 2: blue 3: yellow
            int selectedColor;

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
            
            /* 
                RGB to HSV and vice-versa in HLSL can be found here:
               http://lolengine.net/blog/2013/07/27/rgb-to-hsv-in-glsl
               All components are in the range [0…1], including hue.
            */
            float3 rgb2hsv(float3 c)
            {
                float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
                float4 p = lerp(float4(c.bg, K.wz), float4(c.gb, K.xy), step(c.b, c.g));
                float4 q = lerp(float4(p.xyw, c.r), float4(c.r, p.yzx), step(p.x, c.r));

                float d = q.x - min(q.w, q.y); 
                float e = 1.0e-10;
                return float3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
            }
            
            float3 hsv2rgb(float3 c)
            {
                float4 K = float4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
                float3 p = abs(frac(c.xxx + K.xyz) * 6.0 - K.www);
                return c.z * lerp(K.xxx, saturate(p - K.xxx), c.y);
            }
             
           float4 dim(float3 color, float4 c){
                float luminosity = color.r*.3 + color.g*.59 + color.b*.11;
                float3 bw = float3( luminosity, luminosity, luminosity); 
                c.rgb = lerp(c.rgb, bw, _bwBlend);
                return c;
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
                
                if(fullScreen == 0) {
                 /* DALTONIZATION CORRECTION */
                    if (correctionMethod == 0) {
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
                        c.rgb = (LinearToGammaSpace(saturate(float3(r, g, b))));
                    }
                    
                     /* COLORPOPPER CORRECTION */
                    else if (correctionMethod == 1) {
                        // Convert to HSV Color Space and pop certain user-specified colors.
                        float3 hsv_color = rgb2hsv(color);
                             
                        switch(selectedColor) {
                             /* RED */
                            case 0:
                                if ((hsv_color.x < 0.034 || hsv_color.x > 0.971) && (hsv_color.y > 0.78) && (hsv_color.z > 0.30)) {
                                    // Keep the pixels of that color
                                    c.rgb = (LinearToGammaSpace(saturate(float3(r, g, b))));
                                } else {
                                    c = dim(color,c);
                                }
                                break;
                             /* GREEN */
                            case 1:
                                 if ((hsv_color.x < 0.4305 && hsv_color.x > 0.27) && (hsv_color.y > 0.75) && (hsv_color.z > 0.10)) {
                                    // Keep the pixels of that color
                                    c.rgb = (LinearToGammaSpace(saturate(float3(r, g, b))));
                                 } else {
                                    c = dim(color,c);
                                 }
                                break;
                             /* BLUE */
                            case 2:
                                 if ((0.527 < hsv_color.x && hsv_color.x  < 0.694) && (hsv_color.y > 0.75) && (hsv_color.z > 0.3)) {
                                    // Keep the pixels of that color
                                    c.rgb = (LinearToGammaSpace(saturate(float3(r, g, b))));
                                 } else {
                                    c = dim(color,c);
                                 }
                                break;
                             /* YELLOW */
                            case 3:
                                 if ((0.115 < hsv_color.x && hsv_color.x < 0.183) && (hsv_color.y > 0.50) && (hsv_color.z > 0.2)) {
                                    // Keep the pixels of that color
                                    c.rgb = (LinearToGammaSpace(saturate(float3(r, g, b))));
                                 } else {
                                    c = dim(color,c);
                                 }
                                break;
                            default:
                                break;
                        }
                    }
                     /* TEXTURE CORRECTION */
                    else if (correctionMethod == 2){
                        // Convert to HSV Color Space and put a texture on a user-specified color.
                        float3 hsv_color = rgb2hsv(color);
                        switch(selectedColor) {
                            /* RED */
                            case 0:
                                if ((hsv_color.x < 0.034 || hsv_color.x > 0.971) && (hsv_color.y > 0.78) && (hsv_color.z > 0.30)) {
                                    // TODO: Put texture...
                                    r = 0.0;
                                    g = 0.0;
                                    b = 1.0;
                                    c.rgb = float3(r, g, b);
                                }
                                break;
                            /* GREEN */  
                            case 1:
                                if ((hsv_color.x < 0.4305 && hsv_color.x > 0.27) && (hsv_color.y > 0.75) && (hsv_color.z > 0.10)) {
                                    // TODO: Put texture...
                                    r = 0.0;
                                    g = 0.0;
                                    b = 1.0;
                                    c.rgb = float3(r, g, b);
                                 }
                                break;
                             /* BLUE */
                            case 2:
                                if ((0.527 < hsv_color.x && hsv_color.x  < 0.694) && (hsv_color.y > 0.75) && (hsv_color.z > 0.3)) {
                                    // TODO: Put texture...
                                    r = 0.0;
                                    g = 0.0;
                                    b = 1.0;
                                    c.rgb = float3(r, g, b);
                                 }
                                break;
                             /* YELLOW */
                            case 3:
                                if ((0.115 < hsv_color.x && hsv_color.x < 0.183) && (hsv_color.y > 0.50) && (hsv_color.z > 0.2)) {
                                    // TODO: Put texture...
                                    r = 0.0;
                                    g = 0.0;
                                    b = 1.0;
                                    c.rgb = float3(r, g, b);
                                 }
                                break;
                            default:
                                break;
                            }
                         } 
                  }
                return c;
            }
            ENDCG
        }
    }
}
