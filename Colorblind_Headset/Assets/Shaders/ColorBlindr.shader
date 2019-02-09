Shader "Hidden/ColorBlindr"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Strength ("Blindness Strength (Float)", Float) = 0.8
	}

	CGINCLUDE

		#include "UnityCG.cginc"

		sampler2D _MainTex;
		float _Strength;

		// Adapted from http://colororacle.org/ algorithms
		// Highly unoptimized shaders but they're intended to be used in the editor only so who cares
		
		//http://www.brucelindbloom.com/index.html?Eqn_RGB_XYZ_Matrix.html


		float3 rgb2lin(float3 c) { 
			// return (0.992052 * GammaToLinearSpace(c) + 0.003974) * 128.498039; 
			return(GammaToLinearSpace(c));
		}
		float3 lin2rgb(float3 c) { return LinearToGammaSpace(c); }

		float mymax(float a, float b) { return (a > b ? a : b); }
		float mymin(float a, float b) { return (a < b ? a : b); }
		float3 rgFilter(float3 color, float k1, float k2, float k3)
		{
			/*
			color = saturate(color);
			float3 c_lin = rgb2lin(color);
					
			float r_blind = (k1 * c_lin.r + k2 * c_lin.g) / 16448.25098;
			float b_blind = (k3 * c_lin.r - k3 * c_lin.g + 128.498039 * c_lin.b) / 16448.25098;
			r_blind = saturate(r_blind);
			b_blind = saturate(b_blind);

			return lerp(color, lin2rgb(float3(r_blind, r_blind, b_blind)), _Strength);
			*/

			/*
			proto
			0.1139    0.8990    0.0066   r
			0.1064    0.8400   -0.0050 x g
			0.0119    0.0554    1.0066   b
			*/
			/*
			proto2
			0.1077    0.8499    0.0021   r
			0.1005    0.7933   -0.0093 x g
			0.0207    0.1242    1.0143   b
			*/
			/*
			RGBtoProt =

				0.1127    0.8891    0.0196
				0.1124    0.8878    0.0001
			   -0.0166   -0.1311   -0.0114


			RGBtodeut =

				0.3030    0.7336    0.0222
				0.2887    0.6974   -0.0008
			   -0.0427   -0.1040   -0.0113


			RGBtotrit =

				0.4238    0.5420    0.0175
				0.5020    0.5304    0.0024
			   -0.0389   -0.1100   -0.0115
			*/

			color = saturate(color);
			float3 c_lin = rgb2lin(color);
			
			float r_blind = 0.1139 * c_lin.r + 0.8990 * c_lin.g + 0.0066 * c_lin.b;
			float g_blind = 0.1064 * c_lin.r + 0.8400 * c_lin.g + -0.0050 * c_lin.b;
			float b_blind = 0.0119 * c_lin.r + 0.0554 * c_lin.g + 1.0066 * c_lin.b;
			/*
			float r_blind = 0.1077 * c_lin.r + 0.8499 * c_lin.g + 0.0021 * c_lin.b;
			float g_blind = 0.1005 * c_lin.r + 0.7933 * c_lin.g + -0.0093 * c_lin.b;
			float b_blind = 0.0207 * c_lin.r + 0.1242 * c_lin.g + 1.0143 * c_lin.b;
			*/
			float err_r = c_lin.r - r_blind;
			float err_g = c_lin.g - g_blind;
			float err_b = c_lin.b - b_blind;


			float g_shift = 0.7 * err_r + err_g;
			float b_shift = 0.7 * err_r + err_b;

			//r_blind = c_lin.r;
			g_blind = c_lin.g + g_shift;
			b_blind = c_lin.b + b_shift;

			/*
			r_blind = saturate(r_blind);
			g_blind = saturate(g_blind);
			b_blind = saturate(b_blind);
			*/
			
			float3 color2 = lin2rgb(float3(r_blind, g_blind, b_blind));
			
			// float err_r = mymax(c_lin.r, r_blind) - mymin(c_lin.r, r_blind);
			// float err_g = mymax(c_lin.g, g_blind) - mymin(c_lin.g, g_blind);
			// float err_b = mymax(c_lin.b,b_blind) - mymin(c_lin.b, b_blind);
			/*
			float err_r = c_lin.r - r_blind;
			float err_g = c_lin.g - g_blind;
			float err_b = c_lin.b - b_blind;

			
			float g_shift = 0.7 * err_r + err_g;
			float b_shift = 0.7 * err_r + err_b;

			r_blind = c_lin.r;
			g_blind = c_lin.g + g_shift;
			b_blind = c_lin.b + b_shift;
			*/
			
			return lerp(color, color2, _Strength);

		}

		float3 tritanFilter(float3 color)
		{
			color = saturate(color);

			float anchor_e0 = 0.05059983 + 0.08585369 + 0.00952420;
			float anchor_e1 = 0.01893033 + 0.08925308 + 0.01370054;
			float anchor_e2 = 0.00292202 + 0.00975732 + 0.07145979;
			float inflection = anchor_e1 / anchor_e0;

			float a1 = -anchor_e2 * 0.007009;
			float b1 = anchor_e2 * 0.0914;
			float c1 = anchor_e0 * 0.007009 - anchor_e1 * 0.0914;
			float a2 = anchor_e1 * 0.3636 - anchor_e2 * 0.2237;
			float b2 = anchor_e2 * 0.1284 - anchor_e0 * 0.3636;
			float c2 = anchor_e0 * 0.2237 - anchor_e1 * 0.1284;

			float3 c_lin = rgb2lin(color);

			float L = (c_lin.r * 0.05059983 + c_lin.g * 0.08585369 + c_lin.b * 0.00952420) / 128.498039;
			float M = (c_lin.r * 0.01893033 + c_lin.g * 0.08925308 + c_lin.b * 0.01370054) / 128.498039;
			float S = (c_lin.r * 0.00292202 + c_lin.g * 0.00975732 + c_lin.b * 0.07145979) / 128.498039;

			float tmp = M / L;

			if (tmp < inflection) S = -(a1 * L + b1 * M) / c1;
			else S = -(a2 * L + b2 * M) / c2;

			float r = L * 30.830854 - M * 29.832659 + S * 1.610474;
			float g = -L * 6.481468 + M * 17.715578 - S * 2.532642;
			float b = -L * 0.375690 - M * 1.199062 + S * 14.273846;

			return lerp(color, lin2rgb(saturate(float3(r, g, b))), _Strength);
		}

	ENDCG

	SubShader
	{
		ZTest Always Cull Off ZWrite Off
		Fog { Mode off }

		// (0) Deuteranopia
		Pass
		{
			CGPROGRAM

				#pragma vertex vert_img
				#pragma fragment frag
				#pragma fragmentoption ARB_precision_hint_fastest

				float4 frag(v2f_img i) : SV_Target
				{
					float3 result = rgFilter(tex2D(_MainTex, i.uv).rgb, 37.611765, 90.87451, -2.862745);
					return float4(result, 1.0);
				}
			ENDCG
		}

		// (1) Protanopia
		Pass
		{
			CGPROGRAM

				#pragma vertex vert_img
				#pragma fragment frag
				#pragma fragmentoption ARB_precision_hint_fastest

				float4 frag(v2f_img i) : SV_Target
				{
					float3 result = rgFilter(tex2D(_MainTex, i.uv).rgb, 14.443137, 114.054902, 0.513725);
					return float4(result, 1.0);
				}
			ENDCG
		}

		// (2) Tritanopia
		Pass
		{
			CGPROGRAM
				#pragma vertex vert_img
				#pragma fragment frag
				#pragma fragmentoption ARB_precision_hint_fastest

				float4 frag(v2f_img i) : SV_Target
				{
					float3 result = tritanFilter(tex2D(_MainTex, i.uv).rgb);
					return float4(result, 1.0);
				}

			ENDCG
		}

		// (3) Deuteranopia - Linear
		Pass
		{
			CGPROGRAM

				#pragma vertex vert_img
				#pragma fragment frag
				#pragma fragmentoption ARB_precision_hint_fastest

				float4 frag(v2f_img i) : SV_Target
				{
					float3 color = LinearToGammaSpace(tex2D(_MainTex, i.uv).rgb);
					float3 result = rgFilter(color, 37.611765, 90.87451, -2.862745);
					return float4(GammaToLinearSpace(result), 1.0);
				}
			ENDCG
		}

		// (4) Protanopia - Linear
		Pass
		{
			CGPROGRAM

				#pragma vertex vert_img
				#pragma fragment frag
				#pragma fragmentoption ARB_precision_hint_fastest

				float4 frag(v2f_img i) : SV_Target
				{
					float3 color = LinearToGammaSpace(tex2D(_MainTex, i.uv).rgb);
					float3 result = rgFilter(color, 14.443137, 114.054902, 0.513725);
					return float4(GammaToLinearSpace(result), 1.0);
				}
			ENDCG
		}

		// (5) Tritanopia - Linear
		Pass
		{
			CGPROGRAM
				#pragma vertex vert_img
				#pragma fragment frag
				#pragma fragmentoption ARB_precision_hint_fastest

				float4 frag(v2f_img i) : SV_Target
				{
					float3 color = LinearToGammaSpace(tex2D(_MainTex, i.uv).rgb);
					float3 result = tritanFilter(color);
					return float4(GammaToLinearSpace(result), 1.0);
				}

			ENDCG
		}
	}

	FallBack off
}
