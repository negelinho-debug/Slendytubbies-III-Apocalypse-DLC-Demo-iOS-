Shader "RIOT/Grass/GrassShader (Low)" {
	Properties {
		[Header(Textures)] _MainTex ("Fur Texture (RGB)", 2D) = "white" {}
		[NoScaleOffset] _DirtTex ("Dirt Texture (RGB)", 2D) = "white" {}
		[NoScaleOffset] _HeightMap ("Height Map (Gray) ", 2D) = "white" {}
		[Header(Grass Color Properties)] _GrassColor ("Grass Color", Vector) = (0.27,0.949,0.121,1)
		_GrassBrightness ("Grass Brightness", Range(0, 1)) = 0.15
		_HeightMapBrightness ("Height Map Brightness", Range(0, 1)) = 1
		_GrassTransparency ("Grass Transparency", Range(0, 1)) = 1
		[Header(Dirt Color Properties)] [Toggle] _EnableDirt ("Enable Dirt Layer", Float) = 0
		_DirtColor ("Dirt Color", Vector) = (1,1,1,1)
		_DirtBrightness ("Dirt Brightness", Range(0, 1)) = 0.25
		_DirtTransparency ("Dirt Transparency", Range(0, 1)) = 1
		[Header(Grass Properties)] _GrassLength ("Grass Length", Range(0, 1)) = 0.075
		_GrassStiff ("Grass Stiffness", Range(0, 1)) = 0.1
		_Gravity ("Gravity Direction", Vector) = (0,0.25,0,0)
		[Header(Depth Shadow Properties)] [Toggle] _Shadows ("Depth Shadows", Float) = 0
		_ShadowStrength ("Depth Shadow Strength", Range(0, 1)) = 0.5
		[Header(Randomized Wind Properties)] _WindSpeed ("Wind Speed", Range(0, 1)) = 0.245
		_WindStrength ("Wind Strength", Range(0, 1)) = 0.15
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

			struct Fragment_Stage_Input
			{
				float2 uv : TEXCOORD0;
			};

			float4 frag(Fragment_Stage_Input input) : SV_TARGET
			{
				return _MainTex.Sample(sampler_MainTex, input.uv.xy);
			}

			ENDHLSL
		}
	}
}