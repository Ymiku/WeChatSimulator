// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "UI/SequenceAni"{
	Properties	{		
		_MainTex ("Sequence Frame Image", 2D) = "white" {}
		_Color("Color Tint", Color) = (1, 1, 1, 1) 

		_HorizontalAmount("Horizontal Amount", float) = 4 		
		_VerticalAmount("Vertical Amount", float) = 4 		
		_Speed("Speed", Range(1, 100)) = 30	
	}


	SubShader
	{
		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
		}
		LOD 100
		Pass
		{
			Tags{"LightMode"="ForwardBase"}
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			#include "UnityUI.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float2 uv_1 : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float4 worldPosition : TEXCOORD1;

			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed4 _Color;
			float _HorizontalAmount;
			float _VerticalAmount;
			float _Speed;
			float4 _ClipRect;

			v2f vert (appdata v)
			{
				v2f o;
				o.worldPosition = v.vertex;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.uv_1 = v.uv % 1000;
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				float time = floor(_Time.y * _Speed);
				float row = floor(time / _HorizontalAmount);
				float column = time - row * _HorizontalAmount;
				half2 uv = float2(i.uv.x /_HorizontalAmount, i.uv.y / _VerticalAmount);
				uv.x += column / _HorizontalAmount;
				uv.y -= row / _VerticalAmount; 
				fixed4 col = tex2D(_MainTex, uv);
				col.rgb *= _Color.rgb;
				col *= UnityGet2DClipping(i.worldPosition.xy, _ClipRect);
				return col;
			}
			ENDCG
		}
	}
}
