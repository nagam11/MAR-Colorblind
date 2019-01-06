Shader "Hidden/MatrixShader" {
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


	float4 frag(v2f_img i) : COLOR{
		float4 c = tex2D(_MainTex, i.uv);
		float r = c.r * _rr + c.g * _gr + c.b * _br;
		float g = c.r * _rg + c.g * _gg + c.b * _bg;
		float b = c.r * _rb + c.g * _gb + c.b * _bb;
		float3 shaded = float3(r, g, b);

		float4 result = c;
		result.rgb = shaded;
		return result;
	}
		ENDCG
	}
	}
}