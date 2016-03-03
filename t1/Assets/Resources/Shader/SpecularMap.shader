Shader "Custom/SpecularMap" {
	Properties {
		_MainTex ("Main Texture", 2D) = "white" {}
		_Cube("Reflection Map", Cube) = "" {}
        _Reflectivity("reflectivity",Range (0,1.0)) = 0.5
	}
	SubShader {
	Pass {
	 Lighting Off
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#include "UnityCG.cginc"
		struct appdata_t {
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				float3 normal : NORMAL;
			};

			struct v2f {
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				float3 reflectDir : TEXCOORD1;
			};

		    uniform samplerCUBE _Cube;   
            uniform sampler2D _MainTex;   
            uniform float _Reflectivity;
			v2f vert (appdata_t v)
			{
				v2f o;
                float4x4 modelMatrix = _Object2World;  
                float4x4 modelMatrixInverse = _World2Object;
                float3 I = normalize(_WorldSpaceCameraPos - float3(mul(modelMatrix, v.vertex).xyz) );
                float3 N = normalize(float3(mul(float4(v.normal, 0.0), modelMatrixInverse).xyz));
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.texcoord = v.texcoord;
				o.reflectDir = reflect(I, N);
				return o;
			}
			
			half4 frag (v2f i) : COLOR
			{
				float4 col = saturate(tex2D(_MainTex, i.texcoord) + texCUBE(_Cube, i.reflectDir.xyz) * _Reflectivity);
				return col;
			}
		ENDCG
		}
	}
}