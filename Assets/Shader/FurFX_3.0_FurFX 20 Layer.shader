Shader "FurFX/3.0/FurFX 20 Layer" {
	Properties {
		_Color ("Color (RGB)", Vector) = (1,1,1,1)
		_SpecColor ("Specular Material Color (RGB)", Vector) = (1,1,1,1)
		_BonusAmbient ("Bonus Ambient", Vector) = (0,0,0,1)
		_Shininess ("Shininess", Range(0.01, 10)) = 8
		_Gloss ("Gloss", Range(0, 3)) = 1
		_RimColor ("Rim Color", Vector) = (0,0,0,0)
		_RimPower ("Rim Power", Range(0.5, 8)) = 4
		_FurLength ("Fur Length", Range(0.0002, 0.2)) = 0.05
		_MainTex ("Base (RGB) Mask(A)", 2D) = "white" {}
		_BumpMap ("Normalmap", 2D) = "bump" {}
		_NoiseTex ("Noise (RGB)", 2D) = "white" {}
		_Cube ("Reflection Map", Cube) = "" {}
		_ReflColor ("Reflection Color (RGB)", Vector) = (1,1,1,1)
		_Reflection ("Reflection Power", Range(0, 1)) = 0
		_ReflMinLevel ("Reflection Minimal Length", Range(0, 1)) = 0
		_Cutoff ("Alpha Cutoff", Range(0, 0.5)) = 0.2
		_HairHardness ("Fur Hardness", Range(0.1, 1)) = 1
		_HairThinness ("Fur Thinness", Range(0.01, 5)) = 2
		_HairShading ("Fur Shading", Range(0, 0.5)) = 0.25
		_HairColoring ("Fur Coloring", Range(0, 1)) = 0.1
		_SkinAlpha ("Mask Alpha Factor", Range(0, 1)) = 0.5
		_ForceGlobal ("Force Global", Vector) = (0,0,0,0)
		_ForceLocal ("Force Local", Vector) = (0,0,0,0)
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
	Fallback "VertexLit"
}