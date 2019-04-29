// 两个需求
// 一 每个序列帧播放不一样
// 二 填充另一个原sprite uv
Shader "UI/SequenceAni"{
	Properties	{		
		_MainTex ("Sequence Frame Image", 2D) = "white" {}
		_Color("Color Tint", Color) = (1, 1, 1, 1) 
		_HorizontalAmount("Horizontal Amount", float) = 4 		
		_VerticalAmount("Vertical Amount", float) = 4 		
		_Speed("Speed", Range(1, 100)) = 30	
		_ImgTex ("Sequence Frame Image", 2D) = "white" {}
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
			sampler2D _ImgTex;
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
				float weiget = i.worldPosition.x + 0.1 * i.worldPosition.y;  // todo
				float time = floor((_Time.y) * _Speed);
				float row = floor(time / _HorizontalAmount);
				float column = time - row * _HorizontalAmount;
				half2 uv = float2(i.uv.x /_HorizontalAmount, i.uv.y / _VerticalAmount);
				uv.x += column / _HorizontalAmount;
				uv.y -= row / _VerticalAmount; 
				fixed4 col = tex2D(_MainTex, uv);
				col.rgb *= _Color.rgb;
				fixed4 col_1 = tex2D(_ImgTex, i.uv);
				col_1.rgb *= _Color.rgb;
				float step_1 = step(col.a, col_1.a);
				float step_0 = 1 - step_1;
				return fixed4(col.r * step_0 + col_1.r * step_1, col.g * step_0 + col_1.g * step_1, col.b * step_0 + col_1.b * step_1, step_0 * col.a + step_1 * col_1.a);
			}
			ENDCG
		}
	}
}
