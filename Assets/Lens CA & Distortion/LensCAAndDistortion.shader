Shader "Hidden/LensCAAndDistortion" {
                Properties {
                                _MainTex ("RGB", 2D) = "" {}
                                _BorderTex ("Border", 2D) = "" {}
                }
                
                CGINCLUDE

                #include "UnityCG.cginc"
                

                

                uniform float _RedCyan;
                uniform float _GreenMagenta;
                uniform float _BlueYellow;
                uniform float _Zoom;
                uniform float _BarrelDistortion;
                uniform int _BorderOnOff;
                                                
                sampler2D _BorderTex;
                sampler2D _MainTex;
                

                half4 _MainTex_TexelSize;
                half4 _UV_Transform = half4(1, 0, 0, 1);
                                

                
                float2 barrelDistortion(float2 theUV)
                {
                
                float2 h = theUV.xy - 0.5;
                float r2 = h.x * h.x + h.y * h.y;
                float f = 1 + r2 * (_BarrelDistortion * sqrt(r2));
                
                return f * (1.0+_Zoom) * h + 0.5;
                
                }

                half4 fragLens (v2f_img i) : SV_Target {
                
                                                                half2 zoomUV = i.uv;
                                                                float4 theScreen = tex2D(_MainTex, zoomUV);

                                                                if ((_RedCyan == 0) && (_GreenMagenta == 0) && (_BlueYellow == 0))
                                                                {
                                                                                zoomUV = barrelDistortion(zoomUV);
                                                                                if (_BorderOnOff == 1)
                                                                                {
                                                                                                theScreen.r = tex2D(_MainTex, zoomUV).r * tex2D(_BorderTex, ((zoomUV * 0.9375) + 0.03125)).r;
                                                                                                theScreen.g = tex2D(_MainTex, zoomUV).g * tex2D(_BorderTex, ((zoomUV * 0.9375) + 0.03125)).g;
                                                                                                theScreen.b = tex2D(_MainTex, zoomUV).b * tex2D(_BorderTex, ((zoomUV * 0.9375) + 0.03125)).b;
                                                                                }
                                                                                if (_BorderOnOff == 0)
                                                                                {                                              
                                                                                                theScreen.r = tex2D(_MainTex, zoomUV).r;
                                                                                                theScreen.g = tex2D(_MainTex, zoomUV).g;
                                                                                                theScreen.b = tex2D(_MainTex, zoomUV).b;
                                                                                }
                                                                }
                                                                                
                                                                if ((_RedCyan != 0) || (_GreenMagenta != 0) || (_BlueYellow != 0))
                                                                {              
                                                                                half2 redUV;
                                                                                half2 greUV;
                                                                                half2 bluUV;
                                                                                
                                                                                float redcyan = _RedCyan*0.001;
                                                                                float gremag = _GreenMagenta*0.001;
                                                                                float bluyel = _BlueYellow*0.001;
                                                                                
                                                                                zoomUV = barrelDistortion(zoomUV);
                                                                                redUV = (zoomUV * (1.0+redcyan)) - (redcyan/2.0);
                                                                                greUV = (zoomUV * (1.0+gremag)) - (gremag/2.0);
                                                                                bluUV = (zoomUV * (1.0+bluyel)) - (bluyel/2.0);

                                                                                if (_BorderOnOff == 1)
                                                                                {
                                                                                theScreen.r = tex2D(_MainTex, redUV).r * tex2D(_BorderTex, ((redUV * 0.9375) + 0.03125)).r;
                                                                                theScreen.g = tex2D(_MainTex, greUV).g * tex2D(_BorderTex, ((greUV * 0.9375) + 0.03125)).g;
                                                                                theScreen.b = tex2D(_MainTex, bluUV).b * tex2D(_BorderTex, ((bluUV * 0.9375) + 0.03125)).b;
                                                                                }
                                                                                if (_BorderOnOff == 0)
                                                                                {                                              
                                                                                theScreen.r = tex2D(_MainTex, redUV).r;
                                                                                theScreen.g = tex2D(_MainTex, greUV).g;
                                                                                theScreen.b = tex2D(_MainTex, bluUV).b;
                                                                                }
                                                                }
                                                                return theScreen;
                }              
                                                



                ENDCG 
                
Subshader {
                  ZTest Always Cull Off ZWrite Off
      ColorMask RGB             
                                                

Pass {    

      CGPROGRAM
      #pragma vertex vert_img
      #pragma fragment fragLens
      ENDCG
  }  


}

Fallback off
                
} // shader
