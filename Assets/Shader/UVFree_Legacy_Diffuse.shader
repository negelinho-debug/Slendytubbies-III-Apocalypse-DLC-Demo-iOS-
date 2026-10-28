Shader "UVFree/Legacy/Diffuse" {
	Properties {
		[Toggle(_UVFREE_LOCAL)] _UVFreeLocal ("Use Local Space (instead of Global)", Float) = 0
		[Toggle(_UVFREE_VERTEX_COLOR)] _UVFreeVertexColor ("Use Vertex Color", Float) = 0
		[Toggle(_UVFREE_BUMPED)] _UVFreeBumped ("Use Normal Mapping", Float) = 1
		[Toggle(_UVFREE_RIM)] _UVFreeRim ("Use Rim Lighting", Float) = 0
		[Toggle(_EMISSION)] _Emission ("Use Emission", Float) = 0
		_TexPower ("Texture Power", Range(0, 20)) = 10
		_Color ("Tint", Vector) = (1,1,1,1)
		_MainTex ("Base Color (RGB) Gloss (A)", 2D) = "white" {}
		_VertexColorStrength ("Vertex Color Strength", Range(0, 1)) = 0
		_BumpScale ("Bump Scale", Range(-2, 2)) = 1
		_BumpMap ("Normal Map", 2D) = "bump" {}
		_RimMultiplier ("Rim Multiplier", Range(0, 8)) = 0
		_RimColor ("Rim Color (RGB) Strength (A)", Vector) = (1,1,1,1)
		_RimPower ("Rim Power", Range(0, 16)) = 5
		_EmissionMultiplier ("Emission Multiplier", Float) = 0
		_EmissionColor ("Emission Color", Vector) = (0,0,0,1)
		_EmissionMap ("Emission", 2D) = "white" {}
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200

		Pass
		{
			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			float4x4 unity_ObjectToWorld;
			float4x4 unity_MatrixVP;
			float4 _MainTex_ST;

			struct Vertex_Stage_Input
			{
				float4 pos : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct Vertex_Stage_Output
			{
				float2 uv : TEXCOORD0;
				float4 pos : SV_POSITION;
			};

			Vertex_Stage_Output vert(Vertex_Stage_Input input)
			{
				Vertex_Stage_Output output;
				output.uv = (input.uv.xy * _MainTex_ST.xy) + _MainTex_ST.zw;
				output.pos = mul(unity_MatrixVP, mul(unity_ObjectToWorld, input.pos));
				return output;
			}

			Texture2D<float4> _MainTex;
			SamplerState sampler_MainTex;
			float4 _Color;

			struct Fragment_Stage_Input
			{
				float2 uv : TEXCOORD0;
			};

			float4 frag(Fragment_Stage_Input input) : SV_TARGET
			{
				return _MainTex.Sample(sampler_MainTex, input.uv.xy) * _Color;
			}

			ENDHLSL
		}
	}
	Fallback "Diffuse"
	//CustomEditor "UVFreeLegacyTopBottomGUI"
}