Shader "CrossGate/Cartoon/BattleUV_RGB" 
{
Properties 
	{	
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_UVTex("UV贴图红绿蓝色通道(RGB)",2D) = "white"{}	
		_UVTexColorR("角色换装颜色绿色通道(R),亮度(A)",Color) = (1,1,1,0.5)
		_ColorRSaturation("饱和度",Float) = 1.0
        _RAlpha("R通道ALPHA值", Range(0, 1)) = 1.0
		_UVTexColorG("角色换装颜色绿色通道(G),亮度(A)",Color) = (1,1,1,0.5)
		_ColorGSaturation("饱和度",Float) = 1.0
        _GAlpha("G通道ALPHA值", Range(0, 1)) = 1.0
		_UVTexColorB("角色换装颜色蓝色通道(B),亮度(A)",Color) = (1,1,1,0.5)
		_ColorBSaturation("饱和度",Float) = 1.0
        _BAlpha("B通道ALPHA值", Range(0, 1)) = 1.0
		_MaskColor("角色颜色(RGB),角色遮罩Alpha(A)",Color) = (1,1,1,1)
//		_ContrastAmount("对比度",Range(0,2.0)) = 1.0
//		_BrightnessAmount("亮度",Range(0,2.0)) = 1.0
		_RimColor("被遮挡后颜色(RGB),遮挡Alpha(A)",Color) = (0.5,0.5,0.5,0.5)
        
	
	}
	//角色非战斗状态shader
	SubShader 
	{	
		Tags {  "Queue" = "Transparent" "RenderType"="Transparent"  }			
		//Tags {  "Queue" = "Geometry" "RenderType"="Opaque"  }			
		Pass
			{
				Cull Off
				Blend SrcAlpha  OneMinusSrcAlpha
          	 	Lighting off
				
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#include "UnityCG.cginc"
				
				sampler2D _MainTex;
				sampler2D _UVTex;
				half4  _MaskColor;
				half4 _UVTexColorR;
				half4 _UVTexColorG;
				half4 _UVTexColorB;
				half _Grayscale;

//				half _ContrastAmount;
//				half _BrightnessAmount;

				half _ColorRSaturation;
				half _ColorGSaturation;
				half _ColorBSaturation;

                fixed _RAlpha;
                fixed _GAlpha;
                fixed _BAlpha;
				
				half4 _MainTex_ST;				
				
				struct vertexInput
				{
					half4 vertex : POSITION;
					half2 texcoord : TEXCOORD0;
					fixed4 color : COLOR;
				};
				
				struct vertexOutput
				{
					half4 pos:SV_POSITION;
					half2 uv : TEXCOORD0;	
					fixed4 color : COLOR;
				};
				
				vertexOutput vert(vertexInput i)
				{
					vertexOutput o;
					o.pos = mul(UNITY_MATRIX_MVP,i.vertex);
					o.uv = TRANSFORM_TEX(i.texcoord,_MainTex);
					o.color = i.color;
					return o;
				}
				
				half3 ContrastSaturationBrightness (half3 color, half brt, half sat) 
				{
					
					
					half3 LuminanceCoeff = half3 (0.2125, 0.7154, 0.0721);
					
//					half3 avgLumin = half3 (0.5, 0.5, 0.5);
					half3 brtColor = color * brt;
					half intensityf = dot (brtColor, LuminanceCoeff);
					half3 intensity = half3 (intensityf, intensityf, intensityf);
					
					half3 satColor = lerp (intensity, brtColor, sat);
//					half3 conColor = lerp (avgLumin, satColor, con);
					
					
					return satColor ;
				}

				half4 frag(vertexOutput v):COLOR
				{
					half4 c = tex2D(_MainTex,v.uv);
					half4 tex = tex2D(_UVTex,v.uv);
					half3 colorR= c.rgb;
					half3 colorG = c.rgb;
					half3 colorB = c.rgb;
					
					 colorR = ContrastSaturationBrightness(colorR  *  (1 - (tex.g + tex.b)) , _UVTexColorR.a * 2,_ColorRSaturation);
					 colorG = ContrastSaturationBrightness(colorG  * tex.g , _UVTexColorG.a * 2,_ColorGSaturation);
					 colorB = ContrastSaturationBrightness(colorB  * tex.b , _UVTexColorB.a * 2,_ColorBSaturation);

					c.rgb =(colorG * _UVTexColorG.rgb + colorB *  _UVTexColorB.rgb + colorR  * _UVTexColorR.rgb) * _MaskColor.rgb ;	

					c.a = (tex.r * _RAlpha + tex.g * _GAlpha + tex.b * _BAlpha) * _MaskColor.a;

					c *= v.color;

					return c;
				}				
				ENDCG
			}
		}	
}
